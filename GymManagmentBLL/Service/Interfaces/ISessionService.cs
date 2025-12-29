using GymManagmentBLL.ViewModels.SessionViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Service.Interfaces
{
  public interface ISessionService
    {

        IEnumerable<SessionViewModel> GetAllSessions();
        SessionViewModel? GetSessionById(int SessionId); 

        bool CreateSession(CreateSessionViewModel createSession);

        UpdateSessionViewModel? GetSessionToUpdate(int SessionId);
        bool UpdateSession(int SessionId, UpdateSessionViewModel sessionToUpdate);

        bool RemoveSession(int SessionId);

        IEnumerable<TrainerSelectViewModel> GetAllTrainersForSelect();
        IEnumerable<CategorySelectViewModel> GetAllCategoriesForSelect();

    }
}
