using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Entites
{
    public class MemberShip:BaseEntity
    {
        //this class for relation m:m between Member ,plan
        // startDate ==> CreatedAt from Base
        // ignore id that inherit from base Entity in fluent api
        public DateTime EndDate { get; set; }
        public string Status
        {
            get
            {
                if (EndDate > DateTime.Now) return "Expired";
                else return "Active";
            }
        }
        public Member Member { get; set; } = null!;

        public int MemberId { get; set; } // fk

        public Plan Plan { get; set; } = null!;
        public int PlanId { get; set; } // fk
    }
}
