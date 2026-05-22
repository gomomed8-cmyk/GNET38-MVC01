using GymCore_Project.DbContexts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GymCore_Project.Controllers
{
    public class PlansController : Controller
    {

        private readonly GymDbContext dbContext;

        public PlansController()
        {
            dbContext = new GymDbContext();
        }

        //index action
        public async Task<IActionResult> Index()
        {
            var Plans =  await dbContext.Plans.ToListAsync();
            return View(Plans);
        }
        // details action
        public async Task<IActionResult> Details(int Id)
        {
            var Plan=await dbContext.Plans.FindAsync(Id);
            if(Plan is  null) 
                return RedirectToAction(nameof(Index));
            else
                return View(Plan);

        }
    }
}
