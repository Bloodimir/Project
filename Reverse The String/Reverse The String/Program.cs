using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
namespace Reverse_The_String
{
    public static class Reverser
    {
        private static IEnumerable<string> GraphemeClusters(this string s)
        {
            var enumerator = StringInfo.GetTextElementEnumerator(s);
            while (enumerator.MoveNext())
            {
                yield return (string) enumerator.Current;
            }
        }

        private static string ReverseGraphemeClusters(this string s)
        {
            return string.Join("", s.GraphemeClusters().Reverse().ToArray());
        }

        public static void Main()
        {
            Console.WriteLine("Ters Çevrilecek String'i Giriniz.");
            string s = Console.ReadLine();
            Console.Write("Girilen string'in ters hali: ", s);

            var r = s.ReverseGraphemeClusters();
            Console.WriteLine(r);
            Console.ReadLine();
        }
    }
}