using System;

namespace Faktoriyel
{
    class Program
    {
        static void Main(string[] args)
        {
            AnaGorev();
        }

        static void AnaGorev()
        {
            int i, sayı, faktoriyel = 1;
            Console.Write("Sayı gir = ");
            sayı = Convert.ToInt16(Console.ReadLine().ToString());
            for (i = 1; i <= sayı; i++)
            {
                faktoriyel = faktoriyel * i;
            }
            Console.Write("Sonuç = " + faktoriyel);
            Console.ReadLine();
        }
    }
}
