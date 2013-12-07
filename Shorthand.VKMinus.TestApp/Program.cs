using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shorthand.VKMinus.Parser;

namespace Shorthand.VKMinus.TestApp
{
    class Program
    {
        static void Main(string[] args) {
            var data = VKParser.ParseStartpage();
            Console.WriteLine("TotalLinks: {0}", data.TotalLinks);
            Console.WriteLine("TotalPlusLinks: {0}", data.TotalPlusLinks);
            Console.WriteLine("TotalLatestNewsLinks: {0}", data.TotalLatestNewsLinks);
            Console.WriteLine("TotalLatestNewsPlusLinks: {0}", data.TotalLatestNewsPlusLinks);
            Console.WriteLine("TotalMainLinks: {0}", data.TotalMainLinks);
            Console.WriteLine("TotalMainPlusLinks: {0}", data.TotalMainPlusLinks);
            Console.ReadKey();
        }
    }
}
