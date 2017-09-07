using System;
using System.Text.RegularExpressions;

namespace Word_Counter_Strike
{
    internal class Program
    {
        public static void Main()
        {
            Console.WriteLine("Kelime sayısı sayılacak cümleyi yazınız.");
            string pattern = "[^\\w]";
            string input = Console.ReadLine();
            string[] words = null;
            int i = 0;
            int count = 0;
            words = Regex.Split(input, pattern, RegexOptions.IgnoreCase);
            for (i = words.GetLowerBound(0); i <= words.GetUpperBound(0); i++)
            {
                if (words[i].ToString() == string.Empty)
                    count = count - 1;
                count = count + 1;
            }
            Console.WriteLine("Kelime Sayısı: " + count.ToString());

            Console.ReadKey();
        }
    }
}