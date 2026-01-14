using GymManagmentDAL.Data.Context;
using GymManagmentDAL.Entites;
using GymManagmentDAL.Reposotories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Reposotories.Classes
{
    public class MemberShipRepository : GenericRepository<MemberShip>, IMemberShipReprsitory
    {
        private readonly GymDbContext _context;

        public MemberShipRepository(GymDbContext context):base(context)
        {
            _context = context;
        }
        public IEnumerable<MemberShip> GetAllMembersAndPlans(Func<MemberShip, bool> predicate)   // eager loading
        {
            return _context.MemberShips.Include(ms=>ms.Member).Include(ms => ms.Plan)
                                      .Where(predicate)
                                      .ToList();
        }
    }
}
