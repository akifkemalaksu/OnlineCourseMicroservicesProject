using FreeCourse.Web.Models;
using FreeCourse.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FreeCourse.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICatalogService _catalogService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ICatalogService catalogService)
        {
            _catalogService = catalogService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _catalogService.GetAllCoursesAsync();
            return View(courses);
        }

        public async Task<IActionResult> Detail(string id)
        {
            var course = await _catalogService.GetCourseByIdAsync(id);
            return View(course);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}