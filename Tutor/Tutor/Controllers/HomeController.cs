using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Tutor.Data;
using Tutor.Models;

namespace Tutor.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index(string category = "Все", decimal? maxPrice = null, DateTime? startDate = null)
        {
            var coursesQuery = _context.Courses.AsQueryable();

            if (!string.IsNullOrEmpty(category) && category != "Все")
            {
                if (Enum.TryParse(typeof(Category), category, out var parsedCategory))
                {
                    coursesQuery = coursesQuery.Where(c => (int)c.Category == (int)parsedCategory);
                }
            }

            if (maxPrice.HasValue)
            {
                coursesQuery = coursesQuery.Where(c => c.Price <= maxPrice.Value);
            }

            if (startDate.HasValue)
            {
                var updateTime = DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc);
                coursesQuery = coursesQuery.Where(c => c.StartDate >= updateTime.Date);
            }

            var courses = await coursesQuery.ToListAsync();

            ViewData["Categories"] = Enum.GetValues(typeof(Category)).Cast<Category>().ToList();
            return View(courses);
        }

        public IActionResult Contact()
        {
            return View();
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
