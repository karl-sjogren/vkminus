using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorthand.VKMinus.Data.Models
{
    public class DailyLinkCollection
    {
        public string Id { get; private set; }
        public DateTime ForDate { get; set; }
        public Dictionary<string, bool> Links { get; private set; }

        public DailyLinkCollection()
        {
            Id = Guid.NewGuid().ToString();
            Links = new Dictionary<string, bool>();
        }
    }
}
