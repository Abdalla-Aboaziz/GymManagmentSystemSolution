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
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDbContext _dbContext;
        private readonly Dictionary<Type, object> _repositories = new();

        public UnitOfWork(GymDbContext dbContext, ISessionRepository sessionRepository, IMemberShipReprsitory memberShipReprsitory)
        {
            _dbContext = dbContext;
            SessionRepository = sessionRepository;
            MemberShipReprsitory = memberShipReprsitory;
        }

        public ISessionRepository SessionRepository {  get;  }

        public IMemberShipReprsitory MemberShipReprsitory { get; }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            // [Key]==> Entity Type (Member)
            // [object]=> new GenericRepository<Entity Type>(dbcontext)

            //Entity ==>Member

     

            var EntityType = typeof(TEntity); // get type of entity 

            if(_repositories.TryGetValue(EntityType,out var repo))       // if you create object return same object donot create new one 
            {
                return (IGenericRepository<TEntity>)repo;
            }
            // else 

            var NewRepo = new GenericRepository<TEntity>(_dbContext);  
            _repositories[EntityType]=NewRepo;
            return NewRepo;

        }

        public int SaveChange()
        {
            return _dbContext.SaveChanges();
        }
    }
}
