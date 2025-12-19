using GymManagmentBLL.Service.Interfaces;
using GymManagmentBLL.ViewModels.SessionViewModel;
using GymManagmentDAL.Entites;
using GymManagmentDAL.Reposotories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Service.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SessionService(IUnitOfWork unitOfWork)
        {
             _unitOfWork = unitOfWork;
        }
        public IEnumerable<SessionViewModel> GetAllSession()
        {
            var Sessions = _unitOfWork.SessionRepository.GetAllSessionWithTrainerAndCategory();

            if (!Sessions.Any()) return [];

            return Sessions.Select(x => new SessionViewModel
            {
                Id = x.Id,
                Description = x.Description,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Capacity = x.Capacity,
                TrainerName = x.SessionTrainer.Name,  // related data 
                CategoryName = x.SessionCategory.CategoryName.ToString(),// related data 
                                                                         // AvilableSlot => Capacity -Count Of Booking per Session

                AvilableSlot = x.Capacity - _unitOfWork.SessionRepository.GetCountOFBookSlot(x.Id)

            });
        }
    }
}
