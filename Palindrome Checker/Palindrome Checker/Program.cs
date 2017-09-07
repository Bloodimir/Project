using System;

namespace Palindrome_Checker
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string s, revs = "";
            Console.WriteLine(" String giriniz.");
            s = Console.ReadLine();
            for (int i = s.Length - 1; i >= 0; i--)
            {
                revs += s[i].ToString();
            }
            if (revs == s)
            {
                Console.WriteLine("String Palindromdur");
            }
            else
            {
                Console.WriteLine("String Palindrom Değil");
            }
            Console.ReadLine();
        }
    }
}