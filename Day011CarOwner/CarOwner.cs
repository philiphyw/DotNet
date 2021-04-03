using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day011CarOwner
{
    public class Owner
    {
        [NotMapped]
        CarOwnerDbContext ctx;

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        public byte[] Image { get; set; }

        ICollection<Car>CarsInGarage { get; set; }

        [NotMapped]
       public int CarsNumber { get {
                ctx = new CarOwnerDbContext();
                int value = ctx.Cars.Where(c => c.Owner.Id == this.Id).Count();
                return value; 
            } }
    }


    public class Car
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(30)]
        public string MakeModel { get; set; }
        [Required]
        public Owner Owner { get; set; }

    }
}
