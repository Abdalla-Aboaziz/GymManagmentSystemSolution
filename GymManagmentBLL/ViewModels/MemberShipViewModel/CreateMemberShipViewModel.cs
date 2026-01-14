using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.ViewModels.MemberShipViewModel
{
    public  class CreateMemberShipViewModel
    {
        [Required]
        [Display(Name ="Member")]
        public int MemberId { get; set; }
        [Required]
        [Display(Name = "Plan")]
        public int PlanId { get; set; }


    }
}
