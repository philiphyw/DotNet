using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayMidtermMulti
{
    class InvalidDataException:Exception
    {
        public InvalidDataException(string msg) : base(string.Format($"InvalidDataException: {msg}"))
        {

        }
    }
}
