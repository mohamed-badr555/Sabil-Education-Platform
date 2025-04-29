using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
