using GymManagement.BLL.Services.interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PlanService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetAllAsync(ct: ct);
            return plan.Select(p => new PlanViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                DurationDays = p.DurationDays,
                IsActive = p.IsActive,
                Description = p.Description
            });

        }

        public async Task<PlanViewModel?> GetPlanByIdAsync(int PlanId, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(PlanId, ct);
            if (plan == null) return null;
            return new PlanViewModel
            {

                Name = plan.Name,
                Price = plan.Price,
                DurationDays = plan.DurationDays,
                IsActive = plan.IsActive,
                Description = plan.Description
            };

        }

        public async Task<UpdatePlanViewModel?> GetPlanTOUpdateAsync(int PlanId, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(PlanId, ct);
            if (plan is null || !plan.IsActive) return null;
            if (await HasActiveMemberShipsAsync(PlanId, ct))
                return null;
            else
                return new UpdatePlanViewModel
                {
                    PlanName = plan.Name,
                    Price = plan.Price,
                    DurationDays = plan.DurationDays,
                    Description = plan.Description
                };

        }

        public async Task<bool> ToggleActivationAsync(int PlanId, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(PlanId, ct);

            if (plan is null)
                return false;

            plan.IsActive = !plan.IsActive;

             _unitOfWork.GetRepository<Plan>().Update(plan);
            var result= await _unitOfWork.SaveChangesAsync(ct);

            return result > 0;
        }

        public async Task<bool> UpdatePlanAsync(int id, UpdatePlanViewModel model, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(id, ct);
            if(plan is null) return false;
            if(await HasActiveMemberShipsAsync(id,ct))
                return false;
            plan.DurationDays= model.DurationDays;
            plan.Description= model.Description;
            plan.Price= model.Price;
            plan.UpdatedAt= DateTime.Now;
            _unitOfWork.GetRepository<Plan>().Update(plan);
            var reslut = await _unitOfWork.SaveChangesAsync(ct);
            return reslut > 0;

        }


        // Helper method to check if there are active memberships associated with the plan
        private async Task<bool> HasActiveMemberShipsAsync(int planId, CancellationToken ct)
        {
            return await _unitOfWork.GetRepository<MemberShip>().AnyAsync(M => M.PlanId == planId && M.EndDate > DateTime.Now,ct);
           
        }
    }
}
