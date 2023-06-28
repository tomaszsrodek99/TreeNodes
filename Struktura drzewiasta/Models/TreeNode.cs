using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Struktura_drzewiasta.Models
{
    public class TreeNode
    {
        public int Id { get; set; } 
        public string Name { get; set; } 
        public int? ParentId { get; set; } // Identyfikator węzła-rodzica (może być null)
        public TreeNode Parent { get; set; } // Referencja do węzła-rodzica
        public List<TreeNode> Children { get; set; } // Lista dzieci węzła
        public TreeNode() 
        {
            Children = new List<TreeNode>();
        }
        public TreeNode(int id, string name, int? parentId = null) // Inicjalizacja właściwości węzła
        {
            Id = id;
            Name = name;
            ParentId = parentId;
            Children = new List<TreeNode>();
        }       
    }       
}
