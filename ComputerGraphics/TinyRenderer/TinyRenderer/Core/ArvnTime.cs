using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyRenderer.Core
{
    class ArvnTime
    {
        public static double GetMiliSecond()
        {
            DateTime dt = DateTime.Now;
            DateTime dx = DateTime.MinValue;
            return (dt-dx).TotalMilliseconds;
        }
    }
}
