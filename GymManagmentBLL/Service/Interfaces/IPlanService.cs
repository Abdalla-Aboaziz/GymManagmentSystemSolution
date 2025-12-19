using GymManagmentBLL.ViewModels.PlanViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Service.Interfaces
{
    internal interface IPlanService
    {
        IEnumerable<PlanViewModel> GetAllPlans();

        PlanViewModel? GetPlanById(int Planid);

        UpdatePlanViewModel ? GetPlanToUpdate (int Planid);

        bool UpdatePlan(int Planid, UpdatePlanViewModel PlanToUpdate);

        bool ToggleState (int Planid);

    }
}
