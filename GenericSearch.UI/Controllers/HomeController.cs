using System.Diagnostics;
using GenericSearch.UI.Models;
using Microsoft.AspNetCore.Mvc;

namespace GenericSearch.UI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return this.RedirectToAction(nameof(DefaultSearchController.Index), "DefaultSearch");
        }

        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
