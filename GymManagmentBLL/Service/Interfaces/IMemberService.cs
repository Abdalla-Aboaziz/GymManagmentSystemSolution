using GymManagmentBLL.ViewModels.MemberViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Service.Interfaces
{
    internal interface IMemberService
    {
        IEnumerable<MemberViewModel>GetAllMember();
        bool GreateMember(CreateMemberViewModel createMember);
        MemberViewModel? GetMemberDetails(int Memberid);

        HealthRecordViewModel? GetMemberHealthRecordDetails(int Memberid);

        MemberToUpdateViewModel? GetMemberToUpdate(int Memberid);
        bool UpdateMember(int Memberid, MemberToUpdateViewModel memberToUpdate);

        bool RemoveMember (int Memberid);


    }
}
