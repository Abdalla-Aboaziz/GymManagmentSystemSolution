#region Before UnitOfWork
//using GymManagmentDAL.Data.Context;
//using GymManagmentDAL.Entites;
//using GymManagmentDAL.Reposotories.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace GymManagmentDAL.Reposotories.Classes
//{
//    public class MemberReposotiry : GenericRepository<Member>
//    {


//        //private readonly GymDbContext _dbcontext=new GymDbContext(); 

//        #region Why  Dependency Injection.
//        // //  Wrong approach:
//        // Creating DbContext here makes the class tightly coupled to GymDbContext.
//        // This means:
//        // - We cannot easily change the DbContext implementation.
//        // - Unit Testing becomes very hard (cannot mock DbContext).
//        // - It breaks Dependency Injection principle.

//        //  Correct approach:
//        // DbContext should be injected through the constructor using Dependency Injection.
//        // This allows:
//        // - Loose coupling between layers.
//        // - Easier unit testing.
//        // - Better control over DbContext lifetime.
//        // - Cleaner and more maintainable code.

//        #endregion

//        private readonly GymDbContext _dbcontext;
//        // ask clr inject object from GymDbcontext
//        public MemberReposotiry(GymDbContext dbcontext) :base(dbcontext) 
//        {

//        }
//        #region Before Genaric Class&Interface
//        //public int Add(Member member)
//        //{
//        //    _dbcontext.Members.Add(member);
//        //    return _dbcontext.SaveChanges();
//        //}



//        //public int Delete(Member entity)
//        //{
//        //    _dbcontext.Members.Remove(entity);
//        //    return _dbcontext.SaveChanges();
//        //}

//        //public IEnumerable<Member> GetAll() => _dbcontext.Members.ToList();


//        //public Member? GetById(int id)=>_dbcontext.Members.Find(id);


//        //public int Update(Member member)
//        //{
//        //    _dbcontext.Members.Update(member);
//        //    return _dbcontext.SaveChanges();
//        //} 
//        #endregion
//    }
//} 
#endregion
