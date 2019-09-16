using System;
using System.Collections.Generic;
using System.Text;

namespace Vestis.Core.Model
{
    public class Garment
    {
        public string Name { get; internal set; }
        public ClothingType Type { get; internal set; }
        public PurchaseDate PurchaseDate { get; internal set; }
        public IEnumerable<string> ColorTags { get; internal set; }
        public IEnumerable<string> StyleTags { get; internal set; }
    }
}
