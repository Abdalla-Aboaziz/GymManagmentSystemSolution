using AutoMapper;
using GymManagementBLL.ViewModels.TrainerViewModel;
using GymManagmentBLL.ViewModels.MemberViewModel;
using GymManagmentBLL.ViewModels.PlanViewModel;
using GymManagmentBLL.ViewModels.SessionViewModel;
using GymManagmentDAL.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            MapTrainer ();
            MapMember();
            MapPlan();
            MapSession();


        }

        private void MapTrainer()
        {
            CreateMap<CreateTrainerViewModel, Trainer>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
                {
                    BuildingNumber = src.BuildingNumber,
                    City = src.City,
                    Street = src.Street
                }));


            CreateMap<Trainer, TrainerViewModel>()
                .ForMember(dest => dest.Address,
                opt => opt.MapFrom(src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"));

            CreateMap<Trainer, TrainerToUpdateViewModel>()
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber));

            CreateMap<TrainerToUpdateViewModel, Trainer>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.Address.Street = src.Street;
                    dest.Address.City = src.City;
                    dest.Address.BuildingNumber = src.BuildingNumber;
                    dest.UpdatedAt = DateTime.Now;
                });

        }

        private void MapSession()
        {
            CreateMap<Session, SessionViewModel>()
             .ForMember(dest => dest.CategoryName, option => option.MapFrom(src => src.SessionCategory.CategoryName))
             .ForMember(dest => dest.TrainerName, option => option.MapFrom(src => src.SessionTrainer.Name)) // for load related data
             .ForMember(dest => dest.AvilableSlot, option => option.Ignore());

            CreateMap<CreateSessionViewModel, Session>(); // No Additional Configuration Needed

            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();

            CreateMap<Trainer, TrainerSelectViewModel>();
            CreateMap<Category, CategorySelectViewModel>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CategoryName));

        }


        private void MapMember()
        {
            CreateMap<CreateMemberViewModel, Member>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
                {
                    BuildingNumber = src.BuildingNumber,
                    City = src.City,
                    Street = src.Street
                })).ForMember(dest => dest.HealthRecord, opt => opt.MapFrom(src => src.HealthRecord));

             
            CreateMap<HealthRecordViewModel,HealthRecord>().ReverseMap();

            CreateMap<Member,MemberViewModel>()
                .ForMember(dest=>dest.Gender,opt=>opt.MapFrom(src=>src.Gender.ToString()))
                .ForMember(dest=>dest.DataOfBirth,opt=>opt.MapFrom(src=>src.DateofBirth.ToShortDateString()))
                .ForMember(dest=>dest.Addres,opt=>opt.MapFrom(src=>$"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"));

            CreateMap<Member, MemberToUpdateViewModel>()
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber));
              

            CreateMap<MemberToUpdateViewModel, Member>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.Phote, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.Address.Street = src.Street;
                    dest.Address.City = src.City;
                    dest.Address.BuildingNumber = src.BuildingNumber;
                    dest.UpdatedAt = DateTime.Now;
                });


        }


        private void MapPlan()
        {
           CreateMap<Plan, PlanViewModel>();
            CreateMap<Plan,UpdatePlanViewModel>().ForMember(dest=>dest.PlanName,opt=>opt.MapFrom(src=>src.Name));
            CreateMap<UpdatePlanViewModel,Plan>()
                .ForMember(dest=>dest.Name,opt=>opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

        }
    }
}
