using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CRUDelicious.Models;

namespace CRUDelicious.Controllers;

public class DishController : Controller
{    
    private readonly ILogger<DishController> _logger;
    // Add a private variable of type MyContext (or whatever you named your context file)
    private MyContext _context;         
    // Here we can "inject" our context service into the constructor 
    // The "logger" was something that was already in our code, we're just adding around it   
    public DishController(ILogger<DishController> logger, MyContext context)    
    {        
        _logger = logger;
        // When our HomeController is instantiated, it will fill in _context with context
        // Remember that when context is initialized, it brings in everything we need from DbContext
        // which comes from Entity Framework Core
        _context = context;    
    }

    [HttpGet("new")]
    public ViewResult NewDish()
    {
        return View();
    }

    [HttpPost("new/create")]
    public IActionResult CreateDish(Dish newDish)
    {
        if(!ModelState.IsValid)
        {
            return View("NewDish");
        }
        _context.Add(newDish);
        _context.SaveChanges();
        return RedirectToAction("Index","Home");
    }
    [HttpGet("dishes")]
    public ViewResult AllDishes()
    {
        List<Dish> Dishes = _context.Dish.OrderByDescending(d => d.CreatedAt).ToList();
        return View("AllDishes",Dishes);
    }

    [HttpGet("dishes/{dishId}")]
    public IActionResult ViewDish(int dishId)
    {
        Dish? SingleDish = _context.Dish.FirstOrDefault(d => d.DishId == dishId);
        if(SingleDish == null)
        {
            return RedirectToAction("AllDishes");
        }
        return View(SingleDish);
    }

    [HttpPost("dish/{dishId}/delete")]
    public IActionResult DeleteDish(int dishId)
    {
        Dish? ToBeDeleted = _context.Dish.FirstOrDefault(d => d.DishId == dishId);
        if(ToBeDeleted != null)
        {
            _context.Remove(ToBeDeleted);
            _context.SaveChanges();
        }
        return RedirectToAction("AllDishes");
    }

    [HttpGet("dish/{dishId}/edit")]
    public IActionResult EditDish(int dishId)
    {
        Dish? ToBeEdited = _context.Dish.FirstOrDefault(d => d.DishId == dishId);
        if(ToBeEdited == null)
        {
            return RedirectToAction("AllDishes");
        }
        return View(ToBeEdited);
    }

    [HttpPost("dish/{dishId}/update")]
    public IActionResult UpdateDish(int dishId, Dish editedDish)
    {
        Dish? ToBeUpdated = _context.Dish.FirstOrDefault(d => d.DishId == dishId);
        if(!ModelState.IsValid || ToBeUpdated == null)
        {
            return View("EditDish", editedDish);
        }
        ToBeUpdated.Name = editedDish.Name;
        ToBeUpdated.Chef = editedDish.Chef;
        ToBeUpdated.Tastiness = editedDish.Tastiness;
        ToBeUpdated.Calories = editedDish.Calories;
        ToBeUpdated.Description = editedDish.Description;
        ToBeUpdated.UpdatedAt = DateTime.Now;
        _context.SaveChanges();

        return RedirectToAction("AllDishes");
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}