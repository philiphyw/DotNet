using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day003FirstIndexer
{
    //Tech: Indexer(allow class to return value(s) in a Array-like way. not adding any new function)
    class Program
    {

        class PrimeArray
        { 
            public bool this[long index]
            {
                get 
                {
                    return isPrime(index);
                }
            }


            private bool isPrime(long index)
            {

                for (int i = 2; i <= index / 2; i++)
                {
                    if (index % i == 0)
                    {
                        return false;
                    }
                }
                return true;
            }
        }




        static void Main(string[] args)
        {

            try
            {
                PrimeArray pa = new PrimeArray();
                for (int i = 2; i < 1000; i++)
                {
                    Console.WriteLine($"Is {i} a prime bumber : {pa[i]}");

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
