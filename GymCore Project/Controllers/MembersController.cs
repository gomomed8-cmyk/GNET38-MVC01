using GymManagement.BLL.Services.interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GymCore_Project.Controllers
{
    public class MembersController : Controller
    {
        private readonly IMemberService _memberService;
        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var members = await _memberService.GetAllMembersAsync(ct);
            return View(members);
        }
        [HttpGet]
        public IActionResult Create() => View();

        public async Task<IActionResult> CreateMember(CreateMemberViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Create), model);
            }
            var result = await _memberService.CreateMemberAsync(model, ct);
            if(result)
                TempData["SuccessMessage"] = "Member created successfully!";
            else
                TempData["ErrorMessage"] = "Failed to create member";
            return RedirectToAction(nameof(Index));


        }
    }
}
