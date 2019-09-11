using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WcfService
{
    public static class HashGenerator
    {
        public static string GenerateHash()
        {
            string hash = string.Empty;

            for (int i = 0; i < 16; i++)
            {
                Thread.Sleep(1);
                hash = hash.Shuffle();
                if (new Random().Next(2).Equals(0))
                {
                    Thread.Sleep(1);
                    hash += (char)new Random().Next('A', 'Z');
                }
                else
                {
                    Thread.Sleep(1);
                    hash += new Random().Next(0, 9).ToString();
                }
                Thread.Sleep(1);
            }
            return hash.Shuffle();
        }
        private static string Shuffle(this string str)
        {
            char[] array = str.ToCharArray();
            Random rng = new Random();
            int n = array.Length;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var value = array[k];
                array[k] = array[n];
                array[n] = value;
            }
            return new string(array);
        }
    }
}
