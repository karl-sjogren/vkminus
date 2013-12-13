using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shorthand.VKMinus.Data;
using Shorthand.VKMinus.Data.Models;
using Shorthand.VKMinus.Parser;
using Shorthand.VKMinus.Utilities.Extensions;

namespace Shorthand.VKMinus.TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = VKParser.ParseStartpage();
            Console.WriteLine("-- Data");
            Console.WriteLine("TotalLinks: {0}", data.TotalLinks);
            Console.WriteLine("TotalPlusLinks: {0}", data.TotalPlusLinks);
            Console.WriteLine("TotalLatestNewsLinks: {0}", data.TotalLatestNewsLinks);
            Console.WriteLine("TotalLatestNewsPlusLinks: {0}", data.TotalLatestNewsPlusLinks);
            Console.WriteLine("TotalMainLinks: {0}", data.TotalMainLinks);
            Console.WriteLine("TotalMainPlusLinks: {0}", data.TotalMainPlusLinks);
            Console.WriteLine();
            Console.WriteLine("PercentagePlusLinks: {0}", data.PercentagePlusLinks);
            Console.WriteLine("PercentageMainPlusLinks: {0}", data.PercentageMainPlusLinks);
            Console.WriteLine("PercentageLatestNewsPlusLinks: {0}", data.PercentageLatestNewsPlusLinks);
            Console.WriteLine();

            foreach (var block in data.Blocks)
            {
                Console.WriteLine("Data for block {0}", block.BlockNr);
                Console.WriteLine("  TotalLinks: {0}", block.TotalLinks);
                Console.WriteLine("  TotalPlusLinks: {0}", block.TotalPlusLinks);
                Console.WriteLine("  PercentagePlusLinks: {0}", block.PercentagePlusLinks);
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine("-- Summary");
            new Repository("testdata").Save(data);

            var repo = new Repository();
            var stats = repo.GetByDate(DateTime.Today.AddDays(-1));

            var summary = new DailySummary();
            summary.ForDate = DateTime.Today;

            var numberOfLinksCounted = stats.Sum(s => s.TotalLinks);
            var numberOfPlusLinksCounted = stats.Sum(s => s.TotalPlusLinks);

            summary.PercentageOfPlusLinks = numberOfPlusLinksCounted.PercentageOf(numberOfLinksCounted);
            Console.WriteLine("PercentageOfPlusLinks: {0}", summary.PercentageOfPlusLinks);

            var numberOfLinksCountedInMain = stats.Sum(s => s.TotalMainLinks);
            var numberOfPlusLinksCountedInMain = stats.Sum(s => s.TotalMainPlusLinks);

            summary.PercentageOfPlusLinksInMain = numberOfPlusLinksCountedInMain.PercentageOf(numberOfLinksCountedInMain);
            Console.WriteLine("PercentagePlusLinksInMain: {0}", summary.PercentageOfPlusLinksInMain);

            var numberOfLinksCountedInLatestNews = stats.Sum(s => s.TotalLatestNewsLinks);
            var numberOfPlusLinksCountedInLatestNews = stats.Sum(s => s.TotalLatestNewsPlusLinks);

            summary.PercentageOfPlusLinksInLatestNews = numberOfPlusLinksCountedInLatestNews.PercentageOf(numberOfLinksCountedInLatestNews);
            Console.WriteLine("PercentagePlusLinksInLatestNews: {0}", summary.PercentageOfPlusLinksInLatestNews);

            var blocks =
                stats.SelectMany(m => m.Blocks)
                    .GroupBy(b => b.BlockNr)
                    .Select(s => new BlockDataSummary {BlockNr = s.Key, PercentageOfPlusLinks = (Int32)s.ToList().Average(x=>x.PercentagePlusLinks)}).ToList();

            Console.WriteLine();

            foreach (var block in blocks)
            {
                Console.WriteLine("Data for block {0}", block.BlockNr);
                Console.WriteLine("  PercentageOfPlusLinks: {0}", block.PercentageOfPlusLinks);
                Console.WriteLine();
            }

            summary.Blocks.AddRange(blocks);
            Console.ReadKey();
        }
    }
}
