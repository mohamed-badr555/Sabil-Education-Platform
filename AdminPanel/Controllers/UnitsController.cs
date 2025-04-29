using DAL.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers
{
    public class UnitsController : Controller
    {
        public IActionResult Index(int courseId)
        {


            var model = new CourseUnit();


            return View(model);
        }
    }
}
