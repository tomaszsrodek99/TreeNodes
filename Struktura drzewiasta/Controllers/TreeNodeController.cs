using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Struktura_drzewiasta.Dtos;
using Struktura_drzewiasta.Exceptions;
using Struktura_drzewiasta.Extensions;
using Struktura_drzewiasta.Models;
using Struktura_drzewiasta.Services;
using Struktura_drzewiasta.Validator;
using System;
using System.Threading.Tasks;

namespace Struktura_drzewiasta.Controllers
{
    public class TreeNodeController : Controller
    {
        private readonly TreeNodeService _treeNodeService;
        private readonly IMapper _mapper;

        public TreeNodeController(TreeNodeService treeNodeService, IMapper mapper)
        {
            _treeNodeService = treeNodeService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var treeNodes = await _treeNodeService.GetAllTreeNodes();
                return View(treeNodes);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        [HttpGet]
        [Route("Create", Name = "Create")]
        public IActionResult Create(string? message)
        {
            try
            {
                var parentsList = _treeNodeService.GetAllTreeNodesSync(); // Stwórz listę wyboru do selecta

                ViewBag.ParentsList = new SelectList(parentsList, "Id", "Name"); // Stwórz selectList

                if (message != null) // Jeżeli akcja tworzenia zwróci nam wyjątek z błedem walidacji to wyświetlamy go pod odpowiednim polu w <span>
                    ModelState.AddModelError("Name", message);

                return View();
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        [HttpGet]
        [Route("CreateFromContextMenu/{parentId}", Name = "CreateFromContextMenu")]
        public async Task<IActionResult> CreateFromContextMenu(int parentId, string nodeName) // Tworzenie z context menu
        {
            var validator = new TreeNodeValidator(_treeNodeService); // Tworzenie instancji do walidacji

            var treeNodeDto = new TreeNodeDto()
            {
                Name = nodeName,
                ParentId = parentId
            };

            var validationResult = await validator.ValidateAsync(treeNodeDto);
            try
            {
                if (!validationResult.IsValid)
                {
                    foreach (var error in validationResult.Errors)
                    {
                        ViewBag.ErrorMessage = error.ErrorMessage;
                    }
                    return View("Error");
                }

                var treeNode = _mapper.Map<TreeNodeDto, TreeNode>(treeNodeDto);
                await _treeNodeService.AddTreeNode(treeNode);
                return RedirectToAction("Index");
            }
            catch (MessageException message)
            {
                return RedirectToAction("Create", "TreeNode", new { message = message.Message });
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateFromForm(TreeNodeDto treeNodeDto) // Tworzenie poprzez formularz
        {
            var validator = new TreeNodeValidator(_treeNodeService);

            var validationResult = await validator.ValidateAsync(treeNodeDto);

            try
            {
                if (validationResult.IsValid)
                {
                    var treeNode = _mapper.Map<TreeNodeDto, TreeNode>(treeNodeDto);

                    await _treeNodeService.AddTreeNode(treeNode);
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in validationResult.Errors) // Jeżeli walidacja się nie powiedzie to wyrzuć wyjątek
                    {
                        throw new MessageException(error.ErrorMessage);
                    }
                }
            }
            catch (MessageException message) // Po wyrzuceniu wyjątku prześlij komunikat walidacji do innej akcji i wyświetl
            {
                return RedirectToAction("Create", "TreeNode", new { message = message.Message });
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
            return RedirectToAction("Create", treeNodeDto);
        }
        public async Task<IActionResult> Edit(int id, string? message)
        {
            try
            {
                var parentsList = _treeNodeService.GetAllTreeNodesSync(); // Problem przy pobieraniu asynchronicznym: Unable to cast object of type
                var currentTreeNode = await _treeNodeService.GetTreeNodeById(id);
                var model = _mapper.Map<TreeNode, TreeNodeDto>(currentTreeNode);

                ViewBag.ParentsList = new SelectList(parentsList, "Id", "Name"); // Utwórz SelectList dla listy rodziców

                if (message != null)
                    ModelState.AddModelError("ParentId", message);

                return View(model);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        public async Task<IActionResult> EditTreeNode(TreeNodeDto treeNodeDto)
        {
            var validator = new TreeNodeValidator(_treeNodeService);
            var validationResult = await validator.ValidateAsync(treeNodeDto);
            try
            {
                if (validationResult.IsValid)
                {
                    await _treeNodeService.UpdateTreeNode(treeNodeDto);
                    return RedirectToAction("Index");
                }
            }
            catch (MessageException message)
            {
                // Anonimowy obiekt z dodatkowymi parametrami
                return RedirectToAction("Edit", "TreeNode", new { id = treeNodeDto.Id, message = message.Message });
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
            return RedirectToAction("Edit", "TreeNode", treeNodeDto, null);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _treeNodeService.DeleteTreeNode(id);
            return RedirectToAction("Index");
        }

        [Route("Reverse", Name = "Reverse")]
        public async Task<IActionResult> Reverse()
        {
            // Ustawiamy flagę true/false aby raz odwracać, a potem przywracać sortowanie do stanu początkowego
            bool reverseDirection = HttpContext.Session.GetBool("ReverseDirection");

            switch (reverseDirection)
            {
                case false:
                    var nodes = await _treeNodeService.GetAllNodesReversed();
                    reverseDirection = true;
                    // Zapisz flagę reverseDirection w sesji
                    HttpContext.Session.SetBool("ReverseDirection", reverseDirection);
                    return View("Index", nodes);
                case true:
                    reverseDirection = false;
                    HttpContext.Session.SetBool("ReverseDirection", reverseDirection);
                    return RedirectToAction("Index");
            }
        }
        public async Task<IActionResult> MoveNode(int nodeId, int targetNodeId)
        {
            var treeNode = await _treeNodeService.GetTreeNodeById(nodeId);
            treeNode.ParentId = targetNodeId;
            var model = _mapper.Map<TreeNode, TreeNodeDto>(treeNode);
            return RedirectToAction("EditTreeNode", model);
        }

        [HttpPost]
        public async Task<IActionResult> Move(int id, int? newParentId)
        {
            await _treeNodeService.MoveTreeNode(id, newParentId);
            return RedirectToAction("Index");
        }
    }
}
