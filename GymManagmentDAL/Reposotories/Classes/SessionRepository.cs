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
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDbContext _dbContext;

        public SessionRepository(GymDbContext dbContext) : base(dbContext) 
        {
           _dbContext = dbContext;
        }

        public IEnumerable<Session> GetAllSessionWithTrainerAndCategory()
        {
           return _dbContext.Sessions.Include(x=>x.SessionTrainer).Include(x=>x.SessionCategory).ToList();
        }

        public int GetCountOFBookSlot(int sessionid)
        {
            return _dbContext.MemberSessions.Where(x=>x.SessionId == sessionid).Count();
        }
    }
}
