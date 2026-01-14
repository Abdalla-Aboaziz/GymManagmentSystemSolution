using GymManagmentBLL.ViewModels.MemberShipViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Service.Interfaces
{
    public interface IMemberShipServices
    {

        IEnumerable<MemberShipViewModel> GetActiveMembersAndPlans();
        IEnumerable<MemberSelectViewModel> GetAllMemberforSelect();
        IEnumerable<PlanSelectViewModel> GetplansForSelect();
        bool CancelMemberShip(int memberId, int planId);
        bool CreateMemberShip(CreateMemberShipViewModel createMemberShip);




    }
}
