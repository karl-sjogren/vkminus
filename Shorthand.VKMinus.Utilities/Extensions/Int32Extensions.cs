using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorthand.VKMinus.Utilities.Extensions
{
    public static class Int32Extensions
    {
        public static Int32 PercentageOf(this Int32 val, Int32 other)
        {
            return (Int32)(((double)val / other) * 100);
        }
    }
}
