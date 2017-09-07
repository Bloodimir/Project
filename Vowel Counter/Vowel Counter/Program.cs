using System;
using System.Linq;

namespace Vowel_Counter
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Ünlü harf sayısı sayılacak kelimeyi giriniz.");
            string word = Console.ReadLine();
            char[] vowels = {'a', 'e', 'i', 'o', 'u'};
            int vowelCount = word.Count(x => vowels.Contains(Char.ToLower(x)));
            Console.WriteLine("Ünlü Harf Sayısı");
            Console.WriteLine(vowelCount);
            Console.ReadLine();
        }
    }
}