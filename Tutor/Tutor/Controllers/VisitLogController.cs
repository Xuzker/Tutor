using Microsoft.AspNetCore.Mvc;
using Tutor.Data;
using System.Linq;
using System.Threading.Tasks;
using Tutor.Models;
using Tutor.VisitService;

namespace Tutor.Controllers
{
    public class VisitLogController : Controller
    {
        private readonly VisitLogService _visitLogService;

        public VisitLogController(VisitLogService visitLogService)
        {
            _visitLogService = visitLogService;
        }

        public async Task<IActionResult> Index(string period = "week")
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

            var visitsByDay = visitLogs
                .GroupBy(v => v.VisitDate.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .ToList();

            return View(visitsByDay);
        }
    }
}
