using GymManagmentDAL.Entites.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Entites
{
    public class Trainer:GymUser
    {

        // hire date ==> CreatedAt in BaseEntity handeled with fluent Api

        public Specialies Specialies { get; set; }

        #region Traion-Session
        public ICollection<Session> TrainerSession { get; set; } = null!;
        #endregion

    }
}
