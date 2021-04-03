using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day010EntityFrameworkConsole
{
    public class Person
    {
        //[Key] to specific primary key, but id/Calss name+ Id will be recognized as primary key by convension
        public int Id { get; set; }

        [Required]//not nullable
        [StringLength(50)] // will tell the database to use nvarchar(50)
        public string Name { get; set; }
        
        [Required]
        [Index]//not unique, speed up lookup operations
        public int Age { get; set; }
        /*
        [NotMapped]//not to create a column in database, but just crate in memory only
        public string Comment { get; set; }

        [EnumDataType(typeof(GenderEnum))]
        public GenderEnum Gender { get; set; }
        public enum GenderEnum { Male=1,Female=2,Other=3}//when using enum with database, must assign a int with values
        */
    }
}
