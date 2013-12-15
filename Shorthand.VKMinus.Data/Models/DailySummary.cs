using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorthand.VKMinus.Data.Models
{
    public class DailySummary
    {
        public string Id { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime ForDate { get; set; }
        public Int32 PercentageOfPlusLinks { get; set; }
        public Int32 PercentageOfPlusLinksInMain { get; set; }
        public Int32 PercentageOfPlusLinksInLatestNews { get; set; }
        public List<BlockDataSummary> Blocks { get; private set; }

        public DailySummary()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.Now;
            Blocks = new List<BlockDataSummary>();
        }
    }

    public class BlockDataSummary
    {
        public Int32 BlockNr { get; set; }
        public Int32 PercentageOfPlusLinks { get; set; }
    }
}
