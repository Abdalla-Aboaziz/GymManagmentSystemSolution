using GymManagmentDAL.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Reposotories.Interfaces
{
    public  interface ISessionRepository :IGenericRepository<Session>
    {
        IEnumerable<Session> GetAllSessionWithTrainerAndCategory();

        int GetCountOFBookSlot(int sessionid);

        Session? GetSessionWithTrainerAndCategory(int Sessionid);

    }

}
