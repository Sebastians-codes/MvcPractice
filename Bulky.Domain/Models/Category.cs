using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bulky.Domain.Models;

public class Category
{
    public int Id { get; set; }
    [MaxLength(30)] [Required] public string Name { get; set; }

    [Required]
    [Range(1, 100)]
    [Display(Name = "Display Order")]
    public int DisplayOrder { get; set; }
}