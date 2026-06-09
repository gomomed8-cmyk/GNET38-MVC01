using GymManagement.BLL.ViewModels.MemberViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.interfaces
{
    public interface IMemberService
    {
        Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct);
        Task<bool>CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct);
        Task<MemberViewModel?> GetMemberDetailsByIdAsync(int MemberId, CancellationToken ct =default);
        Task<HealthRecordViewModel?> GetMemberHealthRecordByIdAsync(int MemberId, CancellationToken ct = default);
        Task<MemberToUpdateViewModel?> GetMemberToUpdateByIdAsync(int MemberId, CancellationToken ct = default);
        Task<bool> UpdateMemberDetailsAsync(int Id, MemberToUpdateViewModel model, CancellationToken ct=default);
        Task<bool> RemoveMemberAsync(int MemberId, CancellationToken ct = default);
    }
}
