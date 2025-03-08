using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Tutor.Data;
using Tutor.Models;

namespace Tutor.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoursesController(ApplicationDbContext context)
        {
            _context = context;
        }



        public async Task<IActionResult> Index(string category = "Все", decimal? maxPrice = null, DateTime? startDate = null)
        {
            var coursesQuery = _context.Courses.AsQueryable();


            if (category != "Все")
            {
                coursesQuery = coursesQuery.Where(c => c.Category.ToString() == category);
            }


            if (maxPrice.HasValue)
            {
                coursesQuery = coursesQuery.Where(c => c.Price <= maxPrice.Value);
            }

            if (startDate.HasValue)
            {
                coursesQuery = coursesQuery.Where(c => c.StartDate >= startDate.Value);
            }

            var courses = await coursesQuery.ToListAsync();

            ViewData["Categories"] = Enum.GetValues(typeof(Category)).Cast<Category>().ToList();

            return View(courses);
        }
    


        public async Task<IActionResult> Details(long id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();
            return View(course);
        }

        [HttpPost]
        public async Task<IActionResult> Enroll(long courseId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var application = new Application
            {
                UserId = userId,
                CourseId = courseId,
                ApplicationDate = DateTime.UtcNow,
                Status = "Pending"
            };

            _context.Applications.Add(application);

            var notification = new Notification
            {
                UserId = userId,
                Message = $"Вы успешно записались на курс '{(await _context.Courses.FindAsync(courseId))?.Name}'!",
                CreatedAt = DateTime.UtcNow
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Заявка отправлена. Проверьте уведомления.";
            return RedirectToAction("Index", "Notifications");
        }


    }
}
