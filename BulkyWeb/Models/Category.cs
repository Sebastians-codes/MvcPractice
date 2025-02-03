using System.ComponentModel.DataAnnotations;

namespace BulkyWeb.Models;

public class Category
{
    public int Id { get; set; }
    [MaxLength(30)] [Required] public string Name { get; set; }
    [Range(1, 100)] [Required] public int DisplayOrder { get; set; }
}