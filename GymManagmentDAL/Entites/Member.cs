using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Entites
{
    public class Member:GymUser
    {
        // join date == createdAt of BaseEntity  // handeled with fluent Api

        public string Phote { get; set; } = null!;

        #region Member-HealthRecord
        public HealthRecord HealthRecord { get; set; } = null!; // for relation 1:1 total from both (Member-HealthRecord)

        #endregion
        #region Member -Membership
        public ICollection<MemberShip> MemberShips { get; set; } = null!;
        #endregion
        #region Member -MemberSession
        public ICollection<MemberSession> MemberSessions { get; set; } = null!;
        #endregion
    }
}
