#pragma warning disable CS8618

using System.ComponentModel.DataAnnotations;
namespace CRUDelicious.Models;

public class Dish
{
    [Key]
    public int DishId {get; set;}
    [Required]
    [MinLength(2,ErrorMessage ="Dish Name must be 2 characters or more")]
    [MaxLength(30,ErrorMessage ="Dish Name must be 30 characters or less")]
    public string Name { get; set;}
    [Required]
    [MinLength(2,ErrorMessage ="Chef name must be 2 characters or more")]
    [MaxLength(12,ErrorMessage ="Chef name must be 12 characters or less")]
    public string Chef { get; set;}
    [Required(ErrorMessage ="Tastiness is required!")]
    [Range(1,5)]
    public int Tastiness { get; set;}
    [Required(ErrorMessage ="Calories is required!")]
    [Range(1, int.MaxValue)]
    public int Calories { get; set;}
    [Required(ErrorMessage ="A description of the dish is required!")]
    public string Description { get; set;}
    public DateTime CreatedAt { get; set;} = DateTime.Now;
    public DateTime UpdatedAt { get; set;} = DateTime.Now; 
}