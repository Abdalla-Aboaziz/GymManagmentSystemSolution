using GymManagmentDAL.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Reposotories.Interfaces
{
 public interface IUnitOfWork
    {
        public ISessionRepository SessionRepository { get; }

        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity,new(); // for automaic create object fromtype IGenericRepository<neededEntity> ==> work on same object  on  any request (better performance)

        int SaveChange();


    }
}
