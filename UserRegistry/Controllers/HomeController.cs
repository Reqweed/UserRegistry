using Microsoft.AspNetCore.Mvc;

namespace UserRegistry.Controllers;

public class HomeController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}