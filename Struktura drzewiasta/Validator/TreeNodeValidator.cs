using FluentValidation;
using Struktura_drzewiasta.Dtos;
using Struktura_drzewiasta.Services;
using System.Threading;
using System.Threading.Tasks;

namespace Struktura_drzewiasta.Validator
{
    public class TreeNodeValidator : AbstractValidator<TreeNodeDto>
    {
        private readonly TreeNodeService _treeNodeService;

        public TreeNodeValidator(TreeNodeService treeNodeService)
        {
            _treeNodeService = treeNodeService;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nazwa węzła jest wymagana.")
                .MustAsync(BeUniqueName).WithMessage("Węzeł o podanej nazwie na danym poziomie już istnieje.");
        }

        private async Task<bool> BeUniqueName(TreeNodeDto node, string name, CancellationToken cancellationToken)
        {
            return await _treeNodeService.IsNodeNameUniqueForParent((int)node.ParentId, name);
        }
    }
}
