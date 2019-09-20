using System;
using System.Collections.Generic;
using System.Text;

namespace Vestis.Core.Model
{
    public static class ClothingStyles
    {
        public static IEnumerable<string> All() => new string[]
        {
            // Visual design
            "Plain", "Stripes", "Squares", "Dots", "Abstract", "Logo",
            // Buckle type
            "Buttons", "Velcro", "Zip", "Tie", "Elastic", "Belt",
            // Pockets
            "ShallowPockets", "DeepPockets", "FakePockets",
        };
    }
}
