using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day012MidTerm.DataLayer
{
    public class Passport
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string PassportNo { get; set; }
        

        [Required]
        public byte[] Photo { get; set; }

        [Required]
        public virtual Person Person { get; set; }
    }
}
