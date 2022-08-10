using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyRenderer.Core
{
    class ArvnTime
    {
        public static int GetMiliSecond()
        {
            DateTime dt = DateTime.Now;
            return dt.Millisecond;
        }
    }
}
