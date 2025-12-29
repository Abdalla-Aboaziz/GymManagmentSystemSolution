using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModel;
using GymManagmentDAL.Entites;
using GymManagmentDAL.Reposotories.Interfaces;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TrainerService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public bool CreateTrainer(CreateTrainerViewModel createTrainer)
        {
            try
            {
                var Repo = _unitOfWork.GetRepository<Trainer>();
                if (IsEmailExist(createTrainer.Email) || IsPhoneExist(createTrainer.Phone)) return false;

                // mapping from CreateTrainerViewModel to Trainer entity


                #region Before Auto Mapper 
                //var Trainer = new Trainer()
                //{
                //    Name = createTrainer.Name,
                //    Email = createTrainer.Email,
                //    Phone = createTrainer.Phone,
                //    Specialies = createTrainer.Specialies,
                //    Gender = createTrainer.Gender,
                //    DateofBirth = createTrainer.DateOfBirth,
                //    Address = new Address()
                //    {
                //        BuildingNumber = createTrainer.BuildingNumber,
                //        City = createTrainer.City,
                //        Street = createTrainer.Street,
                //    }
                //}; 
                #endregion

                var TrainerEntity = _mapper.Map<CreateTrainerViewModel, Trainer>(createTrainer);

                Repo.Add(TrainerEntity);
                return _unitOfWork.SaveChange() > 0;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<TrainerViewModel> GetAllTrainer()
        {
            var Trainer = _unitOfWork.GetRepository<Trainer>().GetAll();
            if (Trainer is null || !Trainer.Any()) return [];
            // mapping from Trainer entity to TrainerViewModel

            #region Before Auto Mapper 
            //return Trainer.Select(X => new TrainerViewModel()
            //{
            //    Name = X.Name,
            //    Email = X.Email,
            //    Phone = X.Phone,
            //    Id = X.Id,
            //    Specialties = X.Specialies.ToString()
            //}); 
            #endregion

            return _mapper.Map<IEnumerable<Trainer>, IEnumerable<TrainerViewModel>>(Trainer);

        }

        public TrainerViewModel? GetTrainerDetails(int trainerId)
        {
            var Trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);
            if (Trainer is null ) return null;

            // Mapping from Trainer entity to TrainerViewModel

            #region Before Auto Mapper 
            //return new TrainerViewModel()
            //{
            //    Name = Trainer.Name,
            //    Email = Trainer.Email,
            //    Phone = Trainer.Phone,
            //    Specialties = Trainer.Specialies.ToString(),
            //    DateOfBirth = Trainer.DateofBirth.ToShortDateString(),
            //    Address = $"{Trainer.Address.BuildingNumber} - {Trainer.Address.Street} - {Trainer.Address.City}"
            //}; 
            #endregion

            return _mapper.Map<Trainer, TrainerViewModel>(Trainer);

        }

        public TrainerToUpdateViewModel? GetTrainerToUpdate(int trainerId)
        {
            var Trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);
            if (Trainer is null) return null;

            // mapping from Trainer entity to TrainerToUpdateViewModel

            #region Before Auto Mapper 
            //return new TrainerToUpdateViewModel()
            //{
            //    Name = Trainer.Name,
            //    Email = Trainer.Email,
            //    Phone = Trainer.Phone,
            //    City = Trainer.Address.City,
            //    Street = Trainer.Address.Street,
            //    BuildingNumber = Trainer.Address.BuildingNumber,
            //    Specialties = Trainer.Specialies
            //}; 
            #endregion

            return _mapper.Map<Trainer, TrainerToUpdateViewModel>(Trainer);

        }

        public bool RemoveTrainer(int trainerId)
        {
            var Repo = _unitOfWork.GetRepository<Trainer>();
            var TrainerToRemove = Repo.GetById(trainerId);
            if (TrainerToRemove is null || HasActiveSessions(trainerId)) return false;
            Repo.Delete(TrainerToRemove);
            return _unitOfWork.SaveChange() > 0;
        }



        public bool UpdateTrainerDetails(UpdateTrainerViewModel updateTrainer, int trainerId)
        {
            var Repo = _unitOfWork.GetRepository<Trainer>();
            var TrainerToUpdate = Repo.GetById(trainerId);

            var EmailExist = _unitOfWork.GetRepository<Trainer>().GetAll(
                m => m.Email == updateTrainer.Email && m.Id != trainerId).Any();

            var PhoneExist = _unitOfWork.GetRepository<Trainer>().GetAll(
                m => m.Phone == updateTrainer.Phone && m.Id != trainerId).Any();

            if (TrainerToUpdate is null || EmailExist || PhoneExist) return false;


            #region Before Auto Mapper 

            //TrainerToUpdate.Email = updateTrainer.Email;
            //TrainerToUpdate.Phone = updateTrainer.Phone;
            //TrainerToUpdate.Address.BuildingNumber = updateTrainer.BuildNumber;
            //TrainerToUpdate.Address.City = updateTrainer.City;
            //TrainerToUpdate.Address.Street = updateTrainer.Street;
            //TrainerToUpdate.Specialies = updateTrainer.Specialies;
            //TrainerToUpdate.UpdatedAt = DateTime.Now; 
            #endregion

            _mapper.Map(updateTrainer, TrainerToUpdate); // Auto Mapper from UpdateTrainerViewModel to Trainer entity

            Repo.Update(TrainerToUpdate);

            return _unitOfWork.SaveChange() > 0;
        }

        #region Helper Methods

        private bool IsEmailExist(string Email)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(X => X.Email == Email).Any();
        }
        private bool IsPhoneExist(string Phone)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(X => X.Phone == Phone).Any();
        }


        private bool HasActiveSessions(int trainerId)
        {
            return _unitOfWork.GetRepository<Session>()
                .GetAll(X => X.TrainerId == trainerId && X.Description == "Active").Any();        }
        #endregion
    }
}
