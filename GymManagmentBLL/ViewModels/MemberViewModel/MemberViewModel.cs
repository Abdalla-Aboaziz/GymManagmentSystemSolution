using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.ViewModels.MemberViewModel
{
    public class MemberViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ? Phote {  get; set; }
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;

        public string Gender { get; set; } = null!;

        public string? PlanName { get; set; }
        public string? DataOfBirth { get; set; }
        public string? MemberShipStartData { get; set; }
        public string? MemberShipEndData { get; set; }
        public string? Addres { get; set; }



    }
}
