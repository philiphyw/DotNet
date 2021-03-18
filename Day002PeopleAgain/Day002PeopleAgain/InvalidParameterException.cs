using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day002PeopleAgain
{
    class InvalidParameterException : Exception
    {
        public InvalidParameterException(string msg) : base(string.Format($"Error found: {msg}"))
        { 
        
        }
    }
}
