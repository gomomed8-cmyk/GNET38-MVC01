using GymManagement.BLL.Services.interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MemberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct)
        {
            var emailExists = await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.Email == model.Email, ct);
            var phoneExists = await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.Phone == model.Phone, ct);
            if (emailExists || phoneExists) return false;

            var member = new Member()
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                Address = new Address()
                {
                    BuildingNumber = model.BuildingNumber,
                    Street = model.Street,
                    City = model.City,

                },
                HealthRecord = new HealthRecord()
                {
                    BloodType = model.HealthRecordViewModel.BloodType,
                    Weight = model.HealthRecordViewModel.Weight,
                    Height = model.HealthRecordViewModel.Height,
                    Note = model.HealthRecordViewModel.Note
                }
            };
            _unitOfWork.GetRepository<Member>().Add(member);//add locally
            var reslut = await _unitOfWork.SaveChangesAsync(ct);
            return reslut > 0;
        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct)
        {
            var members = await _unitOfWork.GetRepository<Member>().GetAllAsync(ct: ct);
            if (!members.Any()) return [];
            var MemberViewModels = members.Select(m => new MemberViewModel()
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                Phone = m.Phone,
                Photo = m.Photo,
                Gender = m.Gender.ToString()

            });
            return MemberViewModels;
        }

        public async Task<MemberViewModel?> GetMemberDetailsByIdAsync(int MemberId, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(MemberId, ct);
            if (member == null) return null;
            var memberViewModel = new MemberViewModel()
            {

                Name = member.Name,
                Phone = member.Phone,
                DateOfBirth = member.DateOfBirth.ToShortDateString(),
                Gender = member.Gender.ToString(),
                Address = $"{member.Address.BuildingNumber} - {member.Address.Street} - {member.Address.City}",

            };

            var activeMembership = await _unitOfWork.GetRepository<MemberShip>().FirstOrDefaultAsync(m => m.MemberId == MemberId && m.EndDate > DateTime.Now);
            if (activeMembership is not null)
            {
                    var activeplan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(activeMembership.PlanId, ct);
                memberViewModel.PlanName = activeplan?.Name;
                memberViewModel.MembershipStartDate= activeMembership.CreatedAt.ToString();
                    memberViewModel.MembershipEndDate = activeMembership.EndDate.ToString();
            }
            return memberViewModel;
        }

        public async Task<HealthRecordViewModel?> GetMemberHealthRecordByIdAsync(int MemberId, CancellationToken ct = default)
        {
            var healthRecord = await _unitOfWork.GetRepository<HealthRecord>().FirstOrDefaultAsync(m=>m.MemberId==MemberId, ct:ct);
            if(healthRecord == null) return null;
           return new HealthRecordViewModel()
            {
                Weight= healthRecord.Weight,
                Height= healthRecord.Height,
                BloodType= healthRecord.BloodType,
                Note= healthRecord.Note
            };
            
        }

        public async Task<MemberToUpdateViewModel?> GetMemberToUpdateByIdAsync(int MemberId, CancellationToken ct = default)
        {
            var member =await _unitOfWork.GetRepository<Member>().GetByIdAsync(MemberId, ct);
            if (member is null) return null;
            else
                return new MemberToUpdateViewModel()
                {
                    Name = member.Name,
                    Phone = member.Phone,
                    Email = member.Email,
                    BuildingNumber = member.Address.BuildingNumber,
                    Street = member.Address.Street,
                    City = member.Address.City,
                    Photo = member.Photo

                };
        }

        public async Task<bool> UpdateMemberDetailsAsync(int Id, MemberToUpdateViewModel model, CancellationToken ct = default)
        {
            var member= await _unitOfWork.GetRepository<Member>().GetByIdAsync(Id,ct);
            if (member is null) return false;

            var emailExists = await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.Email == model.Email && m.Id != Id);
            var phoneExists = await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.Phone == model.Phone && m.Id != Id);
            if(emailExists || phoneExists) return false;
            model.Email = model.Email;
            member.Phone = model.Phone;
            member.Address.BuildingNumber = model.BuildingNumber;
            member.Address.Street = model.Street;
            member.Address.City = model.City;
            model.UpdatedAt= DateTime.Now;


             _unitOfWork.GetRepository<Member>().Update(member);
            var result= await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;

        }
        public async Task<bool> RemoveMemberAsync(int MemberId, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(MemberId, ct);
            if (member is null) return false;

            var hasFetureBookings = await _unitOfWork.GetRepository<Booking>().AnyAsync(b => b.MemberId == MemberId && b.Session.StartDate > DateTime.Now,ct);
            if (hasFetureBookings) return false;

             _unitOfWork.GetRepository<Member>().Delete(member);
            var result= await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }
    }
}

