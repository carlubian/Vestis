using System;
using System.Collections.Generic;
using System.Text;

namespace Vestis.Core.Model
{
    public static class ClothingColors
    {
        public static IEnumerable<string> All() => new string[]
        {
            // Red
            "144:0:32", "200:16:35", "210:44:33",
            // Gold
            "255:51:2", "255:96:0", "255:140:0",
            // Light green
            "228:208:10", "198:206:0", "117:179:19",
            // Dark green
            "34:139:34", "53:104:45", "0:106:78",
            // Light blue
            "67:179:174", "0:221:243", "29:172:214",
            // Dark blue
            "48:108:194", "0:60:146", "9:31:146",
            // Purple
            "37:32:111", "76:40:130", "78:0:65",
            // Pink
            "128:0:128", "245:0:135", "235:99:98",
            // Brown
            "255:222:173", "128:64:0", "89:31:11",
            // Gray
            "214:214:214", "128:128:128", "25:25:25"
        };
    }
}
