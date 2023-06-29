using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Struktura_drzewiasta.Context;
using Struktura_drzewiasta.Dtos;
using Struktura_drzewiasta.Exceptions;
using Struktura_drzewiasta.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Struktura_drzewiasta.Services
{
    public class TreeNodeService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public TreeNodeService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<List<TreeNode>> GetAllTreeNodes() //Pobierz wszystkie węzły z dziećmi
        {
            var treeNodesList = await _dbContext.TreeNodes.ToListAsync();
            return treeNodesList;
        }
        public List<TreeNode> GetAllTreeNodesSync()
        {
            return _dbContext.TreeNodes.ToList();
        }
        public async Task<TreeNode> GetTreeNodeById(int id) //Pobierz konkretny węzeł
        {
            var treeNode = await _dbContext.TreeNodes.SingleAsync(n => n.Id == id); // Pobierz pierwszy element, który zgadza się z id, zwróć wyjątek jezeli l.elem. > 1
            return treeNode;
        }
        public async Task AddTreeNode(TreeNode treeNode) //Dodaj węzeł
        {
            _dbContext.TreeNodes.Add(treeNode);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateTreeNode(TreeNodeDto editedNode)
        {
            // Węzeł edytowany
            var nodeAfterChanges = _mapper.Map<TreeNodeDto, TreeNode>(editedNode);
            // Obecny stan w bazie danych dla edytowanego węzła
            var nodeBeforeChanges = await _dbContext.TreeNodes.FindAsync(editedNode.Id);

            // Zabezpieczenie przed edycją korzenia
            if (nodeBeforeChanges.Name.ToLower() == "root")
            {
                throw new MessageException("Nie można zmieniać nazwy i rodzica dla węzła głównego.");
            }

            // Zabezpieczenie przed edycją korzenia
            if (nodeBeforeChanges.ParentId == null)
            {
                throw new MessageException("Nie można utworzyć węzła głównego.");
            }

            //Zabezpiecznie przed duplikowaniem nazwy na danym poziomie drzewa
            if (await IsNodeNameUniqueForParent((int)nodeBeforeChanges.ParentId, nodeAfterChanges.Name))
            {
                throw new MessageException("Nie można utworzyć węzła o podanej nazwie na danym poziomie.");
            }

            // Pobranie obecnego rodzica z bazy danych na podstawie identyfikatora rodzica
            var nodesParentBeforeChanges = await _dbContext.TreeNodes.FindAsync(nodeBeforeChanges.ParentId);

            // Czy nie robimy odwołania do samego siebie
            if (nodeAfterChanges.ParentId != nodeBeforeChanges.ParentId)
            {
                // Sprawdź, czy nowy rodzic istnieje w bazie danych
                var nodesParentAfterChanges = await _dbContext.TreeNodes.FindAsync(nodeAfterChanges.ParentId);

                if (nodesParentAfterChanges != null && nodesParentAfterChanges.Id != nodeAfterChanges.Id) // Dodano warunek sprawdzający, czy nowy rodzic nie jest samym sobą i czy istnieje
                {
                    // Jeżeli nowy rodzic jest rodzicem dla obecnego węzła, zamień miejscami rodzica z dzieckiem
                    if (nodeAfterChanges.Id == nodesParentAfterChanges.ParentId) // Jeżeli id rodzica jest parentId dziecka
                    {
                        var childrenBeforeChanges = await _dbContext.TreeNodes.Where(t => t.ParentId == nodeBeforeChanges.Id).ToListAsync();
                        var childrenAfterChanges = await _dbContext.TreeNodes.Where(t => t.ParentId == nodeAfterChanges.Id).ToListAsync();

                        foreach (var child in childrenBeforeChanges)
                        {
                            child.ParentId = nodeBeforeChanges.Id;
                            _dbContext.TreeNodes.Update(child);
                        }

                        // Zamień miejscami rodzica oraz dziecko i przypisz byłego rodzica-rodzica do dziecka
                        nodesParentAfterChanges.ParentId = nodeBeforeChanges.ParentId;
                        _dbContext.Entry(nodeBeforeChanges).State = EntityState.Detached; // "Wyrzucenie z DbContext" aby nie było błędu z wystepowaniem obiektu o tym samym Id
                        _dbContext.TreeNodes.Update(nodesParentAfterChanges);
                        _dbContext.TreeNodes.Update(nodeAfterChanges);
                        await _dbContext.SaveChangesAsync();
                        return;
                    }
                }
                else
                {
                    // Jeśli próbujemy dodać węzeł jako swojego własnego rodzica, zatrzymaj aktualizację i wyrzuć wyjątek
                    throw new MessageException("Nie można być rodzicem dla samego siebie.");
                }
            }
            nodeBeforeChanges.Name = nodeAfterChanges.Name;
            nodeBeforeChanges.ParentId = nodeAfterChanges.ParentId;
            _dbContext.TreeNodes.Update(nodeBeforeChanges);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteTreeNode(int id)
        {
            var treeNode = await _dbContext.TreeNodes.FindAsync(id);

            if (treeNode != null)
            {
                await DeleteChildNodes(treeNode);

                _dbContext.TreeNodes.Remove(treeNode);
                await _dbContext.SaveChangesAsync();
            }
        }
        private async Task DeleteChildNodes(TreeNode parentNode) // Rekurencyjny alogrytm usuwania dzieci węzła
        {
            var childNodes = await _dbContext.TreeNodes.Where(node => node.ParentId == parentNode.Id).ToListAsync();

            foreach (var childNode in childNodes)
            {
                await DeleteChildNodes(childNode);
                _dbContext.TreeNodes.Remove(childNode);
            }
        }
        public async Task<List<TreeNode>> GetChildNodes(int? parentId) //Pobierz dzieci dla węzła
        {
            var childNodes = await _dbContext.TreeNodes.Where(t => t.ParentId == parentId).ToListAsync();
            return childNodes;
        }

        public async Task<bool> IsNodeNameUniqueForParent(int parentId, string nodeName) // Metoda do walidacji unikalności nazwy na tym samym poziomie
        {
            var isUnique = await _dbContext.TreeNodes.AnyAsync(node => node.ParentId == parentId && node.Name == nodeName);
            return !isUnique;
        }
        public async Task<List<TreeNode>> GetAllNodesReversed() // Rekurencyjne odwracanie drzewa
        {
            var nodes = await _dbContext.TreeNodes.ToListAsync();

            foreach (var node in nodes) // Dla każdego węzła
            {
                ReverseTree(node);
            }

            return nodes;
        }
        private void ReverseTree(TreeNode node)
        {
            if (node.Children.Count == 0)
            {
                return; // Zakończ kiedy nie ma już dzieci
            }

            // Odwróć kolejność dzieci
            node.Children.Reverse();

            // Wywołaj rekurencyjnie odwracanie dla każdego dziecka
            foreach (var child in node.Children)
            {
                ReverseTree(child);
            }
        }
        public async Task MoveTreeNode(int nodeId, int? newParentId) //Przenieś węzeł
        {
            var treeNode = await _dbContext.TreeNodes.FindAsync(nodeId);

            if (treeNode != null)
            {
                treeNode.ParentId = newParentId;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
