using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace Day003FirstIndexer
{
    //Tech: Indexer(allow class to return value(s) in a Array-like way. not adding any new function), dicitonary(equivalent of Java Hashmap)
    class Program
    {
        private static Dictionary<int, int> pnDic;

        private static void LoadPrimeNumberDictionary()
        {
           List<string> pnList = File.ReadAllLines("..\\..\\PrimeNumber.txt").ToList();

            for (int i = 0; i < pnList.Count; i++)
            {
                try
                {
                    int.TryParse(pnList[i], out int value);
                    pnDic[i] = value;
                }
                catch (Exception ex) when (ex is ArgumentNullException)
                {
                    Console.WriteLine("Failed to read data from file primenumber.txt");
                }

            }
        }


        
        class PrimeArray
        {
            public int this[int index]
            {
                get
                {
                    if (index <=0)
                    {
                        throw new ArgumentException("Index must be 1 or greater");
                    }
                    return NthPrime(index);
                }
            }


            private int NthPrime(int index)
            {
                Dictionary<int, int> pnDic = new Dictionary<int, int>();

                //LoadPrimeNumberDictionary(pnDic);

                return pnDic[index];
            }qqqqqqqqqqqqw
        }




        static void Main(string[] args)
        {
            // LoadPrimeNumberDictionary();

            Dictionary<int, string> stuList = new Dictionary<int, string>();
            stuList[0] = "Jim";
            stuList[1] = "Sam";
            stuList[2] = "Tom";

            for (int i = 0; i < stuList.Count; i++)
            {
                Console.WriteLine(stuList[i]);
            }
            
            //PrimeArray pa = new PrimeArray();
            //for (int i = 1; i <= 20; i++)
            //{
            //    Console.Write(pa[i] + "  ");
            //}


        }
    }
}
