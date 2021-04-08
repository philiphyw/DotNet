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

        public Owner()
        {
			CarsInGarage = new List<Car>();
            //ctx = new CarOwnerDbContext();
            //CarsInGarage = ctx.Cars.Where(c => c.Owner.Id == this.Id).ToList<Car>();
        }


        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        public byte[] Image { get; set; }

        [NotMapped]
        // lazy loading by default, we need to ask for earger loading
        public virtual List<Car> CarsInGarage { get; set; } 

        [NotMapped]
		 public int CarsNumber { get => CarsInGarage.Count(); }
		/*
		 [NotMapped]
        CarOwnerDbContext ctx;
		 [NotMapped]
		
       public int CarsNumber { get {
                ctx = new CarOwnerDbContext();
                int value = ctx.Cars.Where(c => c.Owner.Id == this.Id).Count();
                return value;
                //if (this.CarsInGarage == null || this.CarsInGarage.Count==0)
                //{
                //    return 0;
                //}

                //return CarsInGarage.Count();

            }
        }
		*/
		public override string ToString()
		{
			return $"{Id},{Name}";
			
		}

        /*
        public string toDataString()
        {
            string cars = "";
            List<Car> carList = ctx.Cars.Where(car => car.Owner.Id == this.Id).Cast<Car>().ToList();
            if (carList.Count > 0)
            {
                foreach (Car car in carList)
                {
                    cars += $"({car.Id})--{car.MakeModel}" + ",";
                }
                cars = cars.Substring(0, cars.Length - 1);
            }
            return $"{Id};{Name};{cars}";
        }
        */
    }


    public class Car
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(30)]
        public string MakeModel { get; set; }
		public int OwnerId{get; set;}
        
        public virtual Owner Owner { get; set; }
		
		public override string ToString()
        {
            return $"{Id}, {MakeModel}";
        }

    }
}
