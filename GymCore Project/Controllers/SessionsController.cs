using GymManagement.BLL.Services.interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GymCore_Project.Controllers
{
    public class SessionsController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionsController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var Sessions= await _sessionService.GetAllSessionsAsync(ct);
            return View(Sessions);
        }
    }
}
