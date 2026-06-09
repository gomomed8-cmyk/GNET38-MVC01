using GymManagement.BLL.ViewModels.TrainerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.interfaces
{
    public interface ITrainerServicecs
    {
        Task<IEnumerable<TrainerViewModel>> GetAllTrainerAsync(CancellationToken ct=default);
        Task<TrainerViewModel?> GetTrainerDetailsAsync(int TrainerId, CancellationToken ct = default);
        Task<TrainerToUpdateViewModel?> GetTrainerToUpdateAsync(int TrainerId, CancellationToken ct = default);
        Task<bool> CreateTrainerAsync( CreateTrainerViewModel model, CancellationToken ct = default);
        Task<bool> UpdateTrainerDetailsAsync(int TrainerId, TrainerToUpdateViewModel model, CancellationToken ct = default);
        Task<bool> RemoveTrainerAsync(int TrainerId, CancellationToken ct = default);

    }
}
