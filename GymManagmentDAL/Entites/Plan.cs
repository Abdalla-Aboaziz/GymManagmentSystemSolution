using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Entites
{
    public class Plan:BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int DurationDays { get; set; }
         
        public decimal Price { get; set; }
        public bool IsActive { get; set; }


        #region Plan -MemberShip
        public ICollection<Plan> PlanMember { get; set; } = null!;
        #endregion
    }
}
