using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tutor.Data;
using Tutor.Email;
using Tutor.Models;
using Tutor.VisitService;

namespace Tutor.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly VisitLogService _visitLogService;
        public AdminController(ApplicationDbContext context, UserManager<User> userManager, IEmailSender emailSender, VisitLogService visitLogService)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
            _visitLogService = visitLogService;
        }


        // Главная страница админки
        public IActionResult Index()
        {
            return View();
        }

        // =================== КУРСЫ ===================

        public async Task<IActionResult> Courses()
        {
            var courses = await _context.Courses.ToListAsync();
            return View(courses);
        }

        [HttpGet]
        public async Task<IActionResult> ViewTraffic(string period = "week")
        {
            List<VisitLog> visitLogs;

            switch (period.ToLower())
            {
                case "month":
                    visitLogs = await _visitLogService.GetVisitsForLastMonthAsync();
                    break;
                case "year":
                    visitLogs = await _visitLogService.GetVisitsForLastYearAsync();
                    break;
                case "week":
                default:
                    visitLogs = await _visitLogService.GetVisitsForLastWeekAsync();
                    break;
            }

            visitLogs = visitLogs ?? new List<VisitLog>();

            var visitsByDay = visitLogs
                .GroupBy(v => v.VisitDate.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .OrderBy(v => v.Date)
                .ToList();

            return Json(new
            {
                labels = visitsByDay.Select(v => v.Date.ToString("dd.MM.yyyy")),
                counts = visitsByDay.Select(v => v.Count)
            });
        }


        public IActionResult CreateCourse()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse(Course course)
        {
            if (ModelState.IsValid)
            {
                course.StartDate = DateTime.SpecifyKind(course.StartDate, DateTimeKind.Utc);
                _context.Courses.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Courses));
            }
            return View(course);
        }


        public async Task<IActionResult> EditCourse(long id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();
            return View(course);
        }

        [HttpPost]
        public async Task<IActionResult> EditCourse(Course course)
        {
            if (ModelState.IsValid)
            {

                course.StartDate = DateTime.SpecifyKind(course.StartDate, DateTimeKind.Utc);
                _context.Courses.Update(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Courses));
            }
            return View(course);
        }

        public async Task<IActionResult> DeleteCourse(long id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Courses));
        }

        // =================== ЗАЯВКИ ===================

        public async Task<IActionResult> Requests()
        {
            var applications = await _context.Applications
                .Include(a => a.User)
                .Include(a => a.Course)
                .Where(a => a.Status == "Pending")
                .ToListAsync();

            return View(applications);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveRequest(long id)
        {
            var application = await _context.Applications
                .Include(a => a.User)
                .Include(a => a.Course)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (application == null) return NotFound();

            application.Status = "Approved";
            await _context.SaveChangesAsync();

            // Отправка email пользователю
            var subject = "Заявка на курс одобрена";
            var message = $"Здравствуйте, {application.User.FullName}! Ваша заявка на курс \"{application.Course.Name}\" была одобрена. Скоро с вами свяжется преподаватель.";

            await _emailSender.SendEmailAsync(application.User.Email, subject, message);

            return RedirectToAction(nameof(Requests));
        }


        [HttpPost]
        public async Task<IActionResult> RejectRequest(long id)
        {
            var application = await _context.Applications.FindAsync(id);
            if (application == null) return NotFound();

            application.Status = "Rejected";
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Requests));
        }

        // =================== ПОЛЬЗОВАТЕЛИ ===================

        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> BlockUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            user.LockoutEnd = DateTime.UtcNow.AddYears(100);
            await _userManager.UpdateAsync(user);
            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        public async Task<IActionResult> UnblockUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            user.LockoutEnd = null;
            await _userManager.UpdateAsync(user);
            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Users));
        }
    }
}
