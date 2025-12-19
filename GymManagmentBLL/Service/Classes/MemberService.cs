using GymManagmentBLL.Service.Interfaces;
using GymManagmentBLL.ViewModels.MemberViewModel;
using GymManagmentDAL.Entites;
using GymManagmentDAL.Reposotories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Service.Classes
{
    internal class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork; // register Service in Program.cs
        #region Before UnitOfWork
        //private readonly IGenericRepository<Member> _memberRepository;  // object from repo to accses database 
        //private readonly IGenericRepository<MemberShip> _membershipRepository;
        //private readonly IPlanRepository _planRepository;
        //private readonly IGenericRepository<HealthRecord> _healthrecordRepository;
        //private readonly IGenericRepository<MemberSession> _memberSessionRepository;

        //public MemberService(IGenericRepository<Member> memberRepository,
        //    IGenericRepository<MemberShip> membershipRepository, // to accsess membership ==> by Genratic repo  for StartAt EndDate
        //    IPlanRepository planRepository ,  // for access planName   //  tell clr in main to create object
        //    IGenericRepository<HealthRecord> healthrecordRepository,
        //    IGenericRepository<MemberSession> memberSessionRepository
        //    ) 
        //{
        //    _memberRepository = memberRepository;
        //    _membershipRepository = membershipRepository;
        //     _planRepository = planRepository;
        //    _healthrecordRepository = healthrecordRepository;
        //    _memberSessionRepository = memberSessionRepository;
        //} 
        #endregion

        public MemberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<MemberViewModel> GetAllMember()
        {
            var Members = _unitOfWork.GetRepository<Member>().GetAll() ; // return members
            if (Members is null || !Members.Any()) return []; //Empity Object

            // Manual Mapping from Member to MemberViewModel

            #region Way01 Mapping
            //var MemberviewModel = new List<MemberViewModel>();
            //foreach (var Member in MemberviewModel)
            //{
            //    var memberviewModel = new MemberViewModel()
            //    {
            //        Id = Member.Id,
            //        Name = Member.Name,
            //        Email = Member.Email,
            //        Phone = Member.Phone,
            //        Photo = Member.Photo,
            //        Gender = Member.Gender.ToString()
            //    };
            //    MemberviewModel.Add(memberviewModel);
            //} 
            #endregion

            #region Way02 Mapping
            var MemberviewModel = Members.Select(member => new MemberViewModel
            {
                Id = member.Id,
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Phote = member.Phote,
                Gender = member.Gender.ToString()

            }); 
            #endregion

            return MemberviewModel;
        }

      

        public bool GreateMember(CreateMemberViewModel createMember)
        {
            if (IsEmailExist(createMember.Email) || IsPhoneExist(createMember.Phone)) return false;

            try
            {
                //// check is Email is Exist
                //var emailexist = _memberRepository.GetAll(x => x.Email == createMember.Email).Any();

                //// check is Phone is Exist
                //var phoneexist = _memberRepository.GetAll(x => x.Phone == createMember.Phone).Any();

                //// if one of them is exist return false 

                //if (emailexist || phoneexist) return false;

                // mapping from creatememberviewmodel  to member
                var member = new Member()
                {
                    Name = createMember.Name,
                    Email = createMember.Email,
                    Phone = createMember.Phone,
                    Gender = createMember.Gender,
                    DateofBirth = createMember.DateOfBirth,
                    Address = new Address()
                    {
                        City = createMember.City,
                        Street = createMember.Street,
                        BuildingNumber = createMember.BuildingNumber,
                    },
                    HealthRecord = new HealthRecord()
                    {
                        Height = createMember.HealthRecord.Height,
                        Weight = createMember.HealthRecord.Weight,
                        BloodType = createMember.HealthRecord.BloodType,
                        Note = createMember.HealthRecord.Note,
                    }
                };

              //   return _memberRepository.Add(member) > 0;

                _unitOfWork.GetRepository<Member>().Add(member);
              return _unitOfWork.SaveChange()>0;

            }
            catch (Exception)
            {

                return false;
            }

        }

        public MemberViewModel? GetMemberDetails(int Memberid)
        {
            var Member = _unitOfWork.GetRepository<Member>().GetById(Memberid);
            if (Member == null) return null;

            // mapping from member to view model 
            var viewmodel = new MemberViewModel()
            {
                Name = Member.Name,
                Email = Member.Email,
                Phone = Member.Phone,
                Gender = Member.Gender.ToString(),
                DataOfBirth = Member.DateofBirth.ToShortDateString(),
                Addres = $"{Member.Address.BuildingNumber},{Member.Address.Street},{Member.Address.City}",
                Phote =Member.Phote,

            };

            // Active MemberShip   ==>GenricMemberShip 

            var ActiveMemberShip = _unitOfWork.GetRepository<MemberShip>().GetAll(x => x.MemberId == Memberid && x.Status == "Active").FirstOrDefault();
            
            if (ActiveMemberShip is not null)
            {
                viewmodel.MemberShipStartData =ActiveMemberShip.CreatedAt.ToShortDateString();
                viewmodel.MemberShipStartData =ActiveMemberShip.EndDate.ToShortDateString();

                var plan = _unitOfWork.GetRepository<Plan>().GetById(ActiveMemberShip.PlanId);
                viewmodel.PlanName =plan?.Name;
            }
            return viewmodel;


        }

        public HealthRecordViewModel? GetMemberHealthRecordDetails(int Memberid)
        {
            var Memberhealthrecord = _unitOfWork.GetRepository<HealthRecord>().GetById(Memberid);
            if (Memberhealthrecord is  null)  return null;
            // mapping form healthRecord  to healthRecordViewModel 
            return new HealthRecordViewModel()
            {
                BloodType = Memberhealthrecord.BloodType,
                Height = Memberhealthrecord.Height, 
                Weight= Memberhealthrecord.Weight,
                Note= Memberhealthrecord.Note,

            };
            
        }

        public MemberToUpdateViewModel? GetMemberToUpdate(int Memberid)
        {
           var member = _unitOfWork.GetRepository<Member>().GetById(Memberid);
            if(member is null ) return null;
            // mapping from member to  MemberToUpdateViewModel
            return new MemberToUpdateViewModel()
            {
                Email = member.Email,
                Name = member.Name,
                Phone = member.Phone,
                Photo = member.Phote,
                BuildingNumber=member.Address.BuildingNumber,
                Street = member.Address.Street,
                City = member.Address.City,


            };



        }

        public bool UpdateMember(int Memberid, MemberToUpdateViewModel memberToUpdate)
        {
            if (IsEmailExist(memberToUpdate.Email) || IsPhoneExist(memberToUpdate.Phone))return false;
                try
            {
               //var EmailExist = _memberRepository.GetAll(x=>x.Email==memberToUpdate.Email).Any(); // check if Email Exist 
               // var PhoneExist = _memberRepository.GetAll(x => x.Phone == memberToUpdate.Phone).Any();

              var MemberRepo = _unitOfWork.GetRepository<Member>(); 

                var Member = MemberRepo.GetById(Memberid);
                if (Member is null ) return false;
                Member.Email = memberToUpdate.Email;
                Member.Phone= memberToUpdate.Phone;
                Member.Address.City = memberToUpdate.City;
                Member.Address.Street = memberToUpdate.Street;
                Member.Address.BuildingNumber= memberToUpdate.BuildingNumber;
                Member.UpdatedAt= DateTime.Now;

                MemberRepo.Update(Member);
                return _unitOfWork.SaveChange()> 0;

            }
            catch 
            {
                return false;   
            }
        }


        public bool RemoveMember(int Memberid)
        {

            var MemberRepo = _unitOfWork.GetRepository<Member>();
            


            var  member = MemberRepo.GetById(Memberid);
            if (member is null ) return false;

            var HasActiveMemberSession = _unitOfWork.GetRepository<MemberSession>().GetAll(x=>x.MemberId==Memberid&&x.Session.StartDate>DateTime.Now).Any();

            if (HasActiveMemberSession ) return false;

            // before delete member ==> should delete membership he inrolled
            var MemberShipRepo = _unitOfWork.GetRepository<MemberShip>();


            var Membership = MemberShipRepo.GetAll(x => x.MemberId == Memberid);

            try
            {
                if (Membership.Any())
                {
                    foreach (var membership in Membership)
                    {
                        MemberShipRepo.Delete(membership);
                    }
                }

                //  return _memberRepository.Delete(member) > 0;

                MemberRepo.Delete(member);
                return _unitOfWork.SaveChange() > 0;

            }
            catch 
            {

                return false;
            }



         //   _unitOfWork.GetRepository<MemberShip>();

        }

        #region Helper Method 
        private bool IsEmailExist(string email)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(x => x.Email == email).Any();
                
        }
        private bool IsPhoneExist(string phone) 
        {
            return _unitOfWork.GetRepository<Member>().GetAll(x=>x.Phone==phone).Any();
        }

       
        #endregion

    }
}
