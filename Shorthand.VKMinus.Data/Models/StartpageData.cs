using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorthand.VKMinus.Data.Models
{
    public class StartpageData
    {
        public string Id { get; set; }
        public DateTime CreatedAt { get; private set; }
        public Int32 TotalLinks { get; set; }
        public Int32 TotalArticleLink { get; set; }
        public Int32 TotalPlusLinks { get; set; }
        public Int32 TotalLatestNewsLinks { get; set; }
        public Int32 TotalLatestNewsPlusLinks { get; set; }
        public Int32 TotalMainLinks { get; set; }
        public Int32 TotalMainPlusLinks { get; set; }
        public List<BlockData> Blocks { get; private set; } 

        public StartpageData()
        {
            CreatedAt = DateTime.Now;
            Blocks = new List<BlockData>();
        }
    }

    public class BlockData
    {
        public Int32 BlockNr { get; set; }
        public Int32 TotalLinks { get; set; }
        public Int32 TotalPlusLinks { get; set; }
    }
}
