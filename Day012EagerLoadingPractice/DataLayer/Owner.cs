using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day012EagerLoadingPractice.DataLayer
{
    public class Owner
    {
        public int Id { get; set; }
        public string Name { get; set; }


        [NotMapped]
        public int CarNumber { get {

                return this.CarList.Count;
            }  }
    
        
        public virtual List<Car> CarList { get; set; } = new List<Car>();
    }
}
