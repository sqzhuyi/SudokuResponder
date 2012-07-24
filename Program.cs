using System;
using System.Collections.Generic;

namespace SudokuResponder
{
    class Program
    {
        static void Main(string[] args)
        {
            string s = "000000007,008000400,003801600,804306201,000000000,105407908,007603800,006000100,400000005";

            SudokuService service = new SudokuService(s);
            DateTime start = DateTime.Now;
            string result = service.GetResult();
            TimeSpan span = DateTime.Now - start;
            service.PrintResult();

            Console.WriteLine("计算结束，用时 {0} 毫秒", span.Milliseconds);
            Console.WriteLine("press enter to exit");
            Console.ReadLine();
        }

    }
}
