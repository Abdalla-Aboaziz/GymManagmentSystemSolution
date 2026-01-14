using AutoMapper;
using GymManagmentBLL.Service.Interfaces;
using GymManagmentBLL.ViewModels.MemberShipViewModel;
using GymManagmentDAL.Entites;
using GymManagmentDAL.Reposotories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Service.Classes
{
    public class MemberShipServices : IMemberShipServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MemberShipServices(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public bool CancelMemberShip(int memberId, int planId)
        {
           var memberShip=_unitOfWork.GetRepository<MemberShip>().GetAll(ms=> ms.MemberId == memberId && ms.PlanId == planId).FirstOrDefault();
            if (memberShip is null) return false;
            _unitOfWork.GetRepository<MemberShip>().Delete(memberShip);
            return _unitOfWork.SaveChange()>0;

        }

        public bool CreateMemberShip(CreateMemberShipViewModel createMemberShip)
        {
            try
            {
                // check if member exists
                var member = _unitOfWork.GetRepository<Member>().GetById(createMemberShip.MemberId);
                if (member is null) return false;
                // check if plan exists
                var plan = _unitOfWork.GetRepository<Plan>().GetById(createMemberShip.PlanId);
                if (plan is null || !plan.IsActive) return false;
                // create memberShip

                var memberShip = new MemberShip
                {
                    MemberId = createMemberShip.MemberId,
                    PlanId = createMemberShip.PlanId,
                    CreatedAt = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(plan.DurationDays),
                    UpdatedAt = DateTime.Now
                };
                _unitOfWork.GetRepository<MemberShip>().Add(memberShip);
                return _unitOfWork.SaveChange() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Create Membership Failed: {ex}");

                return false;
            }


        }

        public IEnumerable<MemberSelectViewModel> GetAllMemberforSelect()
        {
           var members= _unitOfWork.GetRepository<Member>().GetAll();
            if (members is null || !members.Any()) return [];
            var memberSelects= _mapper.Map<IEnumerable<MemberSelectViewModel>>(members);
            return memberSelects;
        }

        public IEnumerable<MemberShipViewModel> GetActiveMembersAndPlans()
        {
            var memberShip = _unitOfWork.MemberShipReprsitory.GetAllMembersAndPlans(ms => ms.Status == "Active");
            if (memberShip is null || !memberShip.Any()) return [];
            var memberShipViewModels = _mapper.Map<IEnumerable<MemberShipViewModel>>(memberShip);
            return memberShipViewModels;
        }

        public IEnumerable<PlanSelectViewModel> GetplansForSelect()
        {
           var plans = _unitOfWork.GetRepository<Plan>().GetAll(p=>p.IsActive);
            if (plans is null || !plans.Any()) return [];
            var planSelects= _mapper.Map<IEnumerable<PlanSelectViewModel>>(plans);
            return planSelects;
        }

     
    }
}
