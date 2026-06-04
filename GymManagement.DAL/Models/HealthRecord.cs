using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Models
{
    public class HealthRecord : BaseEntity
    {

        public decimal Height { get; set; }   // in cm
        public decimal Weight { get; set; }   // in kg
        public string BloodType { get; set; } = null!;   // e.g. "A+", "O-"
        public string? Note { get; set; }
       
    }
}
