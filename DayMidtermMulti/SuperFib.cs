using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayMidtermMulti
{
    class SuperFib
    {
 
        // key is N, value is the Nth prime number, sorty by N
        private SortedDictionary<int, long> cache = new SortedDictionary<int, long>();
        public long this[int wantedNth]
        {
            get
            {
                if (cache.ContainsKey(wantedNth))
                {
                    return cache[wantedNth];
                }

                int count = 0; // how Super-Fibonacci numbers have I found so far
                long currNum = 0;
                while (true)
                {
                    
                    if (isSuperFib(currNum))
                    {
                        count++; // found another prime number
                        cache[count] = currNum; // save in dictionary
                        if (count == wantedNth) return currNum;
                    }
                    currNum++;
                }
            }
        }

        private bool isSuperFib(long num)
        {
            long n1, n2, n3, n4;
            n1 = 0;
            n2 = 1;
            n3 = 1;

            if (num == n1 || num==n2 || num==n3)
            {
                return true;
            }
            
            while (n3 < num)
            {
                n4 = n3 + n2 + n1;
                n1 = n2;
                n2 = n3;
                n3 = n4;
            }

            if (n3 == num)
            {
                return true;
            }

            return false;
                 

        }

    }
}
