using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tutor.Data;

namespace Tutor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesApiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoursesApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCourses([FromQuery] string category, [FromQuery] decimal? maxPrice)
        {
            var query = _context.Courses.AsQueryable();

            if (!string.IsNullOrEmpty(category) && category != "Все")
            {
                query = query.Where(c => c.Category.ToString() == category);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(c => c.Price <= maxPrice.Value);
            }

            var courses = await query.ToListAsync();
            return Ok(courses);
        }
    }
}
