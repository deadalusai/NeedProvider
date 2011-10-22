using System;

namespace NeedyTypeTest
{
    class Program
    {
        public static void Main(string[] args)
        {
            NeedyTests.Run();

            Console.ReadKey(intercept: true);
        }
    }
}
