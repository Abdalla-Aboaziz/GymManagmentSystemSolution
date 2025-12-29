using AutoMapper;
using GymManagmentBLL.Service.Interfaces;
using GymManagmentBLL.ViewModels.PlanViewModel;
using GymManagmentDAL.Entites;
using GymManagmentDAL.Reposotories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Service.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PlanService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
             _mapper = mapper;
        }

        public IEnumerable<PlanViewModel> GetAllPlans()
        {
           var Plans=_unitOfWork.GetRepository<Plan>().GetAll();
            if (Plans is null || !Plans.Any()) return [];


            //mapping from plan to planviewmodel
            #region Before Auto Mapper
            //return Plans.Select(p => new PlanViewModel
            //{
            //    Id = p.Id,
            //    Name = p.Name,
            //    Price = p.Price,
            //    Description = p.Description,
            //    DurationDays = p.DurationDays,
            //    IsActive= p.IsActive,

            //}); 
            return _mapper.Map<IEnumerable<Plan>, IEnumerable<PlanViewModel>>(Plans);

            #endregion

        }

        public PlanViewModel? GetPlanById(int Planid)
        {
           var Plan = _unitOfWork.GetRepository<Plan>().GetById(Planid);
            if (Plan is null ) return null;
            // mapping from plan to planviewmodel
            #region Before Auto Mapper
            //return new PlanViewModel
            //{
            //    Id = Plan.Id,
            //    Name = Plan.Name,
            //    Price = Plan.Price,
            //    Description = Plan.Description,
            //    DurationDays = Plan.DurationDays,
            //    IsActive = Plan.IsActive,


            //}; 
            #endregion

            return _mapper.Map<Plan, PlanViewModel>(Plan); // Auto Mapper from Plan to PlanViewModel


        }

        public UpdatePlanViewModel? GetPlanToUpdate(int Planid)
        {
            // cannot update on plan has active membership 
            var Plan = _unitOfWork.GetRepository<Plan>().GetById(Planid);
            if (Plan is null||HasActiveMemberShip(Planid) ) return null;
            // mapping from plan to updateplanviewmodel
            #region Before Auto Mapper
            //return new UpdatePlanViewModel
            //{
            //    Description = Plan.Description,
            //    DurationDays = Plan.DurationDays,
            //    Price = Plan.Price,
            //    PlanName = Plan.Name,
            //};

            #endregion

            return _mapper.Map<Plan, UpdatePlanViewModel>(Plan); // Auto Mapper from Plan to UpdatePlanViewModel

        }

        public bool UpdatePlan(int Planid, UpdatePlanViewModel PlanToUpdate)
        {
            var Plan = _unitOfWork.GetRepository<Plan>().GetById(Planid);
            if (Plan is null || HasActiveMemberShip(Planid)) return false;

            (Plan.Description, Plan.DurationDays, Plan.Price, Plan.Name) = 
              (PlanToUpdate.Description,PlanToUpdate.DurationDays,PlanToUpdate.Price,PlanToUpdate.PlanName);

            _unitOfWork.GetRepository<Plan>().Update(Plan);
            return _unitOfWork.SaveChange()>0;
        }

        public bool ToggleState(int Planid)
        {
            var repo = _unitOfWork.GetRepository<Plan>();
            var Plan = repo.GetById(Planid);

            if (Plan is null ||HasActiveMemberShip (Planid)) return false;

            Plan.IsActive=Plan.IsActive ==true?false:true;  // reversed 

            Plan.UpdatedAt=DateTime.Now;
            try
            {
                repo.Update(Plan);
                return _unitOfWork.SaveChange()>0;

            }
            catch 
            {

               return false;
            }
           
        }

      


        #region Helper


        private bool HasActiveMemberShip (int PlanId)
        {
            var ActiveMemberShip= _unitOfWork.GetRepository<MemberShip>().GetAll(x=>x.PlanId==PlanId&& x.Status=="Active");
            return ActiveMemberShip.Any();
        }
        #endregion
    }
}
