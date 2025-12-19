using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Entites
{
    public class MemberSession:BaseEntity
    {
        // bookingdate ==> CreatedAt 
        public bool IsAttended { get; set; }
        public Member Member { get; set; } = null!;
        public int MemberId { get; set; } // fk

        public Session Session { get; set; } = null!;
        public int SessionId { get; set; } // fK
    }
}
