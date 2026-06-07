using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.Classes;
using GymManagement.DAL.Repositories.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymCore_Project.Controllers
{
    public class PlansController : Controller
    {

        private readonly IGenericRepository<Plan> planRepository;

        public PlansController(IGenericRepository<Plan> planRepository)
        {
            this.planRepository = planRepository;
        }
        //index action
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var Plans =  await planRepository.GetAllAsync(false,ct);
            return View(Plans);
        }
        // details action
        public async Task<IActionResult> Details(int id , CancellationToken ct)
        {
            var Plan=await planRepository.GetByIdAsync(id , ct);
            if(Plan is  null) 
                return RedirectToAction(nameof(Index));
            else
                return View(Plan);

        }
    }
}
