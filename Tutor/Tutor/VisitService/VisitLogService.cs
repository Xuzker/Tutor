using Microsoft.EntityFrameworkCore;
using Tutor.Data;
using Tutor.Models;

namespace Tutor.VisitService
{
    public class VisitLogService
    {
        private readonly ApplicationDbContext _context;

        public VisitLogService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<VisitLog>> GetVisitsForLastWeekAsync()
        {
            var lastWeek = DateTime.UtcNow.AddDays(-7);
            return await _context.VisitLogs
                .Where(v => v.VisitDate >= lastWeek)
                .OrderBy(v => v.VisitDate)
                .ToListAsync();
        }

        public async Task<List<VisitLog>> GetVisitsForLastMonthAsync()
        {
            var lastMonth = DateTime.UtcNow.AddMonths(-1);
            return await _context.VisitLogs
                .Where(v => v.VisitDate >= lastMonth)
                .OrderBy(v => v.VisitDate)
                .ToListAsync();
        }

        public async Task<List<VisitLog>> GetVisitsForLastYearAsync()
        {
            var lastYear = DateTime.UtcNow.AddYears(-1);
            return await _context.VisitLogs
                .Where(v => v.VisitDate >= lastYear)
                .OrderBy(v => v.VisitDate)
                .ToListAsync();
        }
    }

}
