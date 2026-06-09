using GymManagement.BLL.ViewModels.MemberViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.interfaces
{
    public interface IPlanService
    {
        Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct=default);
        Task<PlanViewModel?> GetPlanByIdAsync(int PlanId, CancellationToken ct = default);
        Task<UpdatePlanViewModel?> GetPlanTOUpdateAsync(int PlanId, CancellationToken ct = default);
        Task<bool> ToggleActivationAsync(int PlanId, CancellationToken ct = default);
        Task<bool> UpdatePlanAsync(int id, UpdatePlanViewModel model, CancellationToken ct = default);



    }
}
