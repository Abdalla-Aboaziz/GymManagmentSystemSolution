using GymManagmentDAL.Data.Context;
using GymManagmentDAL.Entites;
using GymManagmentDAL.Reposotories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Reposotories.Classes
{
    public class PlaneRepository : IPlanRepository
    {
        private readonly GymDbContext _dbContext;

        public PlaneRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IEnumerable<Plan> GetAll()=> _dbContext.Plans.ToList();



        public Plan? GetById(int id) => _dbContext.Plans.Find(id);



        public int Update(Plan plan)
        {
            _dbContext.Plans.Update(plan);
            return _dbContext.SaveChanges();
        }
    }
}
