using GymManagmentDAL.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Reposotories.Interfaces
{
    public interface IMemberShipReprsitory:IGenericRepository<MemberShip>
    {
        // has 5 methods from IGenericRepository

        IEnumerable<MemberShip> GetAllMembersAndPlans(Func<MemberShip, bool> predicate); // Eger loading 


    }
}
