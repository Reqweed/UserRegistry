using Microsoft.AspNetCore.Mvc;
using UserRegistry.Models;
using UserRegistry.Services.Contracts;
using UserRegistry.ViewModels;

namespace UserRegistry.Controllers;

public class HomeController(IDataGenerator generator, IExportService exportService) : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult GenerateFakeUsers([FromBody] GeneratorSettings settings)
    {
        return Json(generator.GenerateUsers(settings));
    }
    
    [HttpPost]
    public IActionResult ExportToCsv([FromBody] IEnumerable<User> users)
    {

        return File(exportService.GetCsvFile(users), "text/csv", "users.csv");
    }
}