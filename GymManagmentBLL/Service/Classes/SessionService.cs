using AutoMapper;
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
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork,IMapper mapper)
        {
             _unitOfWork = unitOfWork;
           _mapper = mapper;
        }

        public bool CreateSession(CreateSessionViewModel createSession)
        {



            try
            {

                // check if Trainer Exists
                if (!IsTrainerExists(createSession.TrainerId)) return false;
                // check if Category Exists
                if (!IsCategoryExists(createSession.CategoryId)) return false;
                // check if StartDate is before EndDate
                if (!IsDateTimeValid(createSession.StartDate, createSession.EndDate)) return false;
                // check if Capacity is between 0 and 25
                if (createSession.Capacity > 25 || createSession.Capacity < 0) return false;

                var sessionentity = _mapper.Map<CreateSessionViewModel, Session>(createSession); // create from vm to entity(Session)

                _unitOfWork.GetRepository<Session>().Add(sessionentity); // add to dbcontext

                return _unitOfWork.SaveChange() > 0; // save changes to database
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Create Session Failed {ex}");
                return false;
            }
        }

        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var Sessions = _unitOfWork.SessionRepository.GetAllSessionWithTrainerAndCategory();

            if (!Sessions.Any()) return [];

            #region Before Auto Mapper
            //return Sessions.Select(x => new SessionViewModel
            //{
            //    Id = x.Id,
            //    Description = x.Description,
            //    StartDate = x.StartDate,
            //    EndDate = x.EndDate,
            //    Capacity = x.Capacity,
            //    TrainerName = x.SessionTrainer.Name,  // related data 
            //    CategoryName = x.SessionCategory.CategoryName.ToString(),// related data 
            //                                                             // AvilableSlot => Capacity -Count Of Booking per Session

            //    AvilableSlot = x.Capacity - _unitOfWork.SessionRepository.GetCountOFBookSlot(x.Id)

            //}); 
            #endregion

            var MappedSessions= _mapper.Map<IEnumerable<Session>,IEnumerable<SessionViewModel>>(Sessions);
            foreach (var Session in MappedSessions)
            {
                Session.AvilableSlot=Session.Capacity - _unitOfWork.SessionRepository.GetCountOFBookSlot(Session.Id);
               
            }
            return MappedSessions;

        }

        public IEnumerable<TrainerSelectViewModel> GetAllTrainersForSelect()
        {
            var trainers = _unitOfWork.GetRepository<Trainer>().GetAll();
            return _mapper.Map<IEnumerable<Trainer>, IEnumerable<TrainerSelectViewModel>>(trainers); // Auto Mapper from Trainer to TrainerSelectViewModel
        } 

        public IEnumerable<CategorySelectViewModel> GetAllCategoriesForSelect()
        {
            var categories = _unitOfWork.GetRepository<Category>().GetAll();
            return _mapper.Map<IEnumerable<Category>, IEnumerable<CategorySelectViewModel>>(categories); // Auto Mapper from Category to CategorySelectViewModel
        }

        public SessionViewModel? GetSessionById(int SessionId)
        {
            var session =_unitOfWork.SessionRepository.GetSessionWithTrainerAndCategory(SessionId);
            if (session is  null) return null;

            #region Before Auto Mapper
            //return new SessionViewModel
            //{

            //    Description = session.Description,
            //    StartDate = session.StartDate,
            //    EndDate = session.EndDate,

            //    TrainerName = session.SessionTrainer.Name,  // related data 
            //    CategoryName = session.SessionCategory.CategoryName.ToString(),// related data 
            //                                                                    // AvilableSlot => Capacity -Count Of Booking per Session
            //    AvilableSlot = session.Capacity - _unitOfWork.SessionRepository.GetCountOFBookSlot(session.Id)
            //}; 
            #endregion

            var MappedSession = _mapper.Map<Session,SessionViewModel>(session);
            MappedSession.AvilableSlot= MappedSession.Capacity - _unitOfWork.SessionRepository.GetCountOFBookSlot(MappedSession.Id);
            return MappedSession;
        }

        public UpdateSessionViewModel? GetSessionToUpdate(int SessionId)
        {
            
            var Session = _unitOfWork.SessionRepository.GetById(SessionId);
            if (!IsSessionAvilableToUpdate(Session!)) return null; // cannot update if session not exists or completed or started or has active booking
            return _mapper.Map<Session, UpdateSessionViewModel>(Session!);


        }

        public bool UpdateSession(int SessionId, UpdateSessionViewModel sessionToUpdate)
        {
            try
            {
                 var Session = _unitOfWork.SessionRepository.GetById(SessionId);
                 if (!IsSessionAvilableToUpdate(Session!)) return false; // cannot update if session not exists or completed or started or has active booking
                // Check if Trainer Exists
               

                 if (!IsTrainerExists(sessionToUpdate.TrainerId)) return false;
                // Check if Date valid 
                if (!IsDateTimeValid(sessionToUpdate.StartDate, sessionToUpdate.EndDate)) return false;

                _mapper.Map(sessionToUpdate, Session); // map updated data from vm to entity
                Session!.UpdatedAt=DateTime.Now;

                _unitOfWork.GetRepository<Session>().Update(Session);
                return _unitOfWork.SaveChange() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to update Session  {ex}");
                return false;
            }
        }

        public bool RemoveSession(int SessionId)
        {
            try
            {
                var Session =_unitOfWork.SessionRepository.GetById(SessionId);
                if (!IsSessionAvilableToRemove(Session!)) return false; // cannot remove if session not exists or has active booking or upcoming or started
                _unitOfWork.GetRepository<Session>().Delete(Session!);
                return _unitOfWork.SaveChange() > 0;

            }
            catch (Exception  ex)
            {
                Console.WriteLine($"Remove Session Failed {ex} ");
                return false;
            }
        }

        #region Helper 
        private bool IsTrainerExists(int trainerId)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);
            return trainer is not null;
        }

        private bool IsCategoryExists(int categoryId)
        {
            var category = _unitOfWork.GetRepository<Category>().GetById(categoryId);
            return category is not null;
        }

        private bool IsDateTimeValid(DateTime StartDate,DateTime EndDate)
        {
            return StartDate < EndDate;
        }

        private bool IsSessionAvilableToUpdate(Session session)
        {
            if (session is null ) return false;
            if(session.EndDate < DateTime.Now) return false; // cannot update completed session
            if (session.EndDate<= DateTime.Now) return false; //  Session Started cannot update
            var HasActiveBooking = _unitOfWork.SessionRepository.GetCountOFBookSlot(session.Id) > 0;
            if (HasActiveBooking) return false; // cannot update session has active booking
            return true;
        }


        private bool IsSessionAvilableToRemove(Session session)
        {
            if (session is null) return false;

            // if Session Started 
          
            if (session.StartDate <= DateTime.Now&&session.EndDate>DateTime.Now) return false; //  Session Started and not completed

            // if Session is UpComing

            if (session.StartDate>DateTime.Now) return false; //  Session is UpComing


            var HasActiveBooking = _unitOfWork.SessionRepository.GetCountOFBookSlot(session.Id) > 0;
            if (HasActiveBooking) return false; // cannot remove session has active booking
            return true;


        }

     







        #endregion
    }
}
