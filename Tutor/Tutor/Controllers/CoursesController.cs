using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Tutor.Data;
using Tutor.Kafka;
using Tutor.Models;

namespace Tutor.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IKafkaProducer _kafkaProducer;

        public CoursesController(IKafkaProducer kafkaProducer, ApplicationDbContext context)
        {
            _kafkaProducer = kafkaProducer;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _context.Courses.ToListAsync();
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
            
            var existingApplication = await _context.Applications
                .FirstOrDefaultAsync(a => a.UserId == userId && a.CourseId == courseId);

            if (existingApplication != null)
            {
                TempData["ErrorMessage"] = "Вы уже подавали заявку на этот курс";
                return RedirectToAction("Details", new { id = courseId });
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
            
            await _kafkaProducer.ProduceAsync("enrollments",
                $"User {userId} enrolled in course {courseId} at {DateTime.UtcNow}");

            return RedirectToAction("Index", "Notifications");
        }

        public async Task<IActionResult> Assignments(long id)
        {
            var course = await _context.Courses
                .Include(c => c.Applications)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isEnrolled = course.Applications.Any(a => a.UserId == userId && a.Status == "Approved");

            if (!isEnrolled)
                return Forbid();

            var assignments = await _context.Assignments
                .Where(a => a.CourseId == id)
                .ToListAsync();

            var model = new CourseAssignmentsViewModel
            {
                CourseName = course.Name,
                Assignments = assignments
            };

            return View(model);
        }

    }
}
