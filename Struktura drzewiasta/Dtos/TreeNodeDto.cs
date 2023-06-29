using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Struktura_drzewiasta.Dtos
{
    public class TreeNodeDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Pole 'Name' jest wymagane.")]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [DisplayName("Rodzic")]
        public int? ParentId { get; set; }
        public List<TreeNodeDto> Children { get; set; }
    }
}
