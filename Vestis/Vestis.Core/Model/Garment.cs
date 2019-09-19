using System;
using System.Collections.Generic;
using System.Text;

namespace Vestis.Core.Model
{
    public class Garment
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public ClothingType Type { get; set; }
        public PurchaseDate PurchaseDate { get; set; }
        public IEnumerable<string> ColorTags { get; set; }
        public IEnumerable<string> StyleTags { get; set; }
        public IEnumerable<string> UserTags { get; set; }
    }
}
