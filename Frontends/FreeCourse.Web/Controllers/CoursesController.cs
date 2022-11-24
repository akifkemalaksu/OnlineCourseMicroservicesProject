using AutoMapper;
using FreeCourse.Shared.Services;
using FreeCourse.Web.Models.Catalogs;
using FreeCourse.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FreeCourse.Web.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private readonly ICatalogService _catalogService;
        private readonly ISharedIdentityService _sharedIdentityService;
        private readonly IMapper _mapper;
        public CoursesController(ICatalogService catalogService, ISharedIdentityService sharedIdentityService, IMapper mapper)
        {
            _catalogService = catalogService;
            _sharedIdentityService = sharedIdentityService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _catalogService.GetAllCoursesByUserIdAsync(_sharedIdentityService.GetUserId));
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _catalogService.GetAllCategoriesAsync();
            ViewBag.categoryList = new SelectList(categories, "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseCreateInput courseCreateInput)
        {
            var categories = await _catalogService.GetAllCategoriesAsync();
            ViewBag.categoryList = new SelectList(categories, "Id", "Name");

            if (!ModelState.IsValid)
                return View();

            courseCreateInput.UserId = _sharedIdentityService.GetUserId;

            //todo kontrol yap
            await _catalogService.CreateCourseAsync(courseCreateInput);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(string id)
        {
            var course = await _catalogService.GetCourseByIdAsync(id);

            if (course is null)
                //todo uyarı göster
                return RedirectToAction(nameof(Index));

            var categories = await _catalogService.GetAllCategoriesAsync();
            ViewBag.categoryList = new SelectList(categories, "Id", "Name", course.CategoryId);

            //todo controller ın şişmemesi adına bu servisleri baz alacak ayrı bir servis geliştir.
            var courseUpdateInput = _mapper.Map<CourseUpdateInput>(course);
            await _catalogService.UpdateCourseAsync(courseUpdateInput);

            return View(courseUpdateInput);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CourseUpdateInput courseUpdateInput)
        {
            var categories = await _catalogService.GetAllCategoriesAsync();
            ViewBag.categoryList = new SelectList(categories, "Id", "Name", courseUpdateInput.CategoryId);

            if (!ModelState.IsValid)
                //todo uyarı geliştir
                return View();

            await _catalogService.UpdateCourseAsync(courseUpdateInput);

            //todo başarılı sonuç ver
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _catalogService.DeleteCourseAsync(id);
            //todo sonucu client a dön
            return RedirectToAction(nameof(Index));
        }
    }
}
