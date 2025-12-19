using GymManagmentDAL.Entites.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Entites
{
    public abstract class GymUser :BaseEntity
    {
        // Set Common Property in Member ,Trainer

        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;

        public DateOnly DateofBirth { get; set; }

        public Gender Gender { get; set; }

        public Address Address { get; set; } = null!;


    }
    [Owned]
  public  class Address
    {
        public int BuildingNumber { get; set; }
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
    }

}
