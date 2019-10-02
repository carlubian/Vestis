using System.Collections.Generic;

namespace Vestis.Core.Model
{
    public enum ClothingType
    {
        ShortTShirt,
        LongTShirt,
        ShortShirt,
        LongShirt,
        ShortBlouse,
        LongBlouse,
        ShortPolo,
        LongPolo,
        SportSweater,
        CozySweater,
        Blazer,
        ShortTrouser,
        LongTrouser,
        ShortSkirt,
        LongSkirt,
        YogaPants,
        ShortDress,
        LongDress,
        Sandals,
        Trainers,
        Shoes,
        Boots,
        Jackets,
        Coats,
        Unknown
    }

    public static class ClothingTypeUtil
    {
        public static ClothingType Parse(string text) => (text.ToLowerInvariant()) switch
        {
            "shorttshirt" => ClothingType.ShortTShirt,
            "longtshirt" => ClothingType.LongTShirt,
            "shortshirt" => ClothingType.ShortShirt,
            "longshirt" => ClothingType.LongShirt,
            "shortblouse" => ClothingType.ShortBlouse,
            "longblouse" => ClothingType.LongBlouse,
            "shortpolo" => ClothingType.ShortPolo,
            "longpolo" => ClothingType.LongPolo,
            "sportsweater" => ClothingType.SportSweater,
            "cozysweater" => ClothingType.CozySweater,
            "blazer" => ClothingType.Blazer,
            "shorttrouser" => ClothingType.ShortTrouser,
            "longtrouser" => ClothingType.LongTrouser,
            "shortskirt" => ClothingType.ShortSkirt,
            "longskirt" => ClothingType.LongSkirt,
            "yogapants" => ClothingType.YogaPants,
            "shortdress" => ClothingType.ShortDress,
            "longdress" => ClothingType.LongDress,
            "sandals" => ClothingType.Sandals,
            "trainers" => ClothingType.Trainers,
            "shoes" => ClothingType.Shoes,
            "boots" => ClothingType.Boots,
            "jackets" => ClothingType.Jackets,
            "coats" => ClothingType.Coats,
            _ => ClothingType.Unknown,
        };

        public static IEnumerable<string> CompatibleWith(string type) => type.ToLowerInvariant() switch
        {
            "shorttshirt" => new string[] { "LongShirt", "LongPolo", "SportSweater",
                                                "CozySweater", "Blazer", "ShortTrouser",
                                                "LongTrouser", "ShortSkirt", "LongSkirt",
                                                "YogaPants", "Sandals", "Trainers",
                                                "Shoes", "Boots", "Jackets", "Coats" },

            "longtshirt" => new string[] { "LongShirt", "LongPolo", "SportSweater",
                                                "CozySweater", "Blazer", "ShortTrouser",
                                                "LongTrouser", "ShortSkirt", "LongSkirt",
                                                "YogaPants", "Sandals", "Trainers",
                                                "Shoes", "Boots", "Jackets", "Coats" },

            "shortshirt" => new string[] {  "ShortTrouser", "LongTrouser", "ShortSkirt",
                                                "LongSkirt", "YogaPants", "Sandals",
                                                "Trainers", "Shoes", "Boots", "Jackets",
                                                "Coats" },

            "longshirt" => new string[] {   "ShortTShirt", "LongTShirt", "LongPolo",
                                                "Blazer", "ShortTrouser", "LongTrouser",
                                                "ShortSkirt", "LongSkirt", "YogaPants",
                                                "Trainers", "Shoes", "Boots",
                                                "Jackets", "Coats" },

            "shortblouse" => new string[] { "CozySweater", "Blazer", "ShortTrouser",
                                                "LongTrouser", "ShortSkirt", "LongSkirt",
                                                "YogaPants", "Sandals", "Trainers",
                                                "Shoes", "Boots", "Jackets", "Coats" },

            "longblouse" => new string[] {  "ShortTShirt", "CozySweater", "Blazer",
                                                "ShortTrouser", "LongTrouser", "ShortSkirt",
                                                "LongSkirt","YogaPants", "Sandals",
                                                "Trainers", "Shoes", "Boots",
                                                "Jackets", "Coats" },

            "shortpolo" => new string[] {   "ShortTrouser", "LongTrouser", "ShortSkirt",
                                                "LongSkirt", "YogaPants", "Sandals",
                                                "Trainers", "Shoes", "Boots", "Jackets",
                                                "Coats" },

            "longpolo" => new string[] {    "ShortTShirt", "ShortTrouser", "LongTrouser",
                                                "ShortSkirt", "LongSkirt", "Trainers",
                                                "Shoes", "Boots", "Jackets",
                                                "Coats" },

            "sportsweater" => new string[] { "ShortTShirt", "LongTShirt", "ShortTrouser",
                                                "LongTrouser", "ShortSkirt", "LongSkirt",
                                                "YogaPants", "Sandals", "Trainers",
                                                "Shoes", "Boots", "Coats" },

            "cozysweater" => new string[] { "ShortTShirt", "LongTShirt", "ShortShirt",
                                                "LongShirt", "ShortTrouser", "LongTrouser",
                                                "ShortSkirt", "LongSkirt", "Trainers",
                                                "Shoes", "Boots", "Jackets",
                                                "Coats" },

            "blazer" => new string[] {      "LongTShirt", "LongShirt", "ShortTrouser",
                                                "LongTrouser", "ShortSkirt", "LongSkirt",
                                                "YogaPants", "Trainers", "Shoes",
                                                "Boots", "Jackets", "Coats" },

            "shorttrouser" => new string[] { "ShortTShirt", "LongTSHirt", "ShortShirt",
                                                "LongShirt", "ShortPolo", "SportSweater",
                                                "CozySweater", "Blazer", "Sandals",
                                                "Trainers", "Shoes", "Boots",
                                                "Jackets", "Coats" },

            "longtrouser" => new string[] { "ShortTShirt", "LongTSHirt", "ShortShirt",
                                                "LongShirt", "ShortPolo", "LongPolo",
                                                "SportSweater", "CozySweater", "Blazer",
                                                "Trainers",  "Shoes", "Boots",
                                                "Jackets",  "Coats" },

            "shortskirt" => new string[] {  "ShortTShirt", "LongTSHirt", "ShortShirt",
                                                "LongShirt", "ShortPolo", "SportSweater",
                                                "CozySweater", "Blazer", "Sandals",
                                                "Trainers", "Shoes", "Boots",
                                                "Jackets", "Coats" },

            "longskirt" => new string[] {   "ShortTShirt", "LongTSHirt", "ShortShirt",
                                                "LongShirt", "ShortPolo", "LongPolo",
                                                "SportSweater", "CozySweater", "Blazer",
                                                "Sandals", "Trainers", "Shoes",
                                                "Boots", "Jackets", "Coats" },

            "yogapants" => new string[] {   "ShortTShirt", "LongTSHirt", "ShortShirt",
                                                "LongShirt", "Sandals", "Trainers",
                                                "Shoes", "Boots", "Jackets",
                                                "Coats" },

            "shortdress" => new string[] {  "Sandals", "Trainers", "Shoes",
                                                "Boots", "Jackets", "Coats" },

            "longdress" => new string[] {   "Sandals", "Trainers", "Shoes",
                                                "Boots", "Jackets", "Coats" },

            "sandals" => new string[] {     "ShortTShirt", "LongTShirt", "ShortShirt",
                                                "ShortBlouse", "LongBlouse", "ShortPolo",
                                                "SportSweater", "ShortTrouser", "ShortSkirt",
                                                "LongSkirt", "YogaPants", "ShortDress",
                                                "LongDress", "Jackets", "Coats" },

            "trainers" => new string[] {    "ShortTShirt", "LongTShirt", "ShortShirt",
                                                "LongShort", "ShortBlouse", "LongBlouse",
                                                "ShortPolo", "LongPolo", "SportSweater",
                                                "CozySweater", "Blazer", "ShortTrouser",
                                                "LongTrouser", "ShortSkirt", "LongSkirt",
                                                "YogaPants", "ShortDress", "LongDress",
                                                "Jackets", "Coats" },

            "shoes" => new string[] {       "ShortTShirt", "LongTShirt", "ShortShirt",
                                                "LongShort", "ShortBlouse", "LongBlouse",
                                                "ShortPolo", "LongPolo", "SportSweater",
                                                "CozySweater", "Blazer", "ShortTrouser",
                                                "LongTrouser", "ShortSkirt", "LongSkirt",
                                                "YogaPants", "ShortDress", "LongDress",
                                                "Jackets", "Coats" },

            "boots" => new string[] {       "ShortTShirt", "LongTShirt", "ShortShirt",
                                                "LongShort", "ShortBlouse", "LongBlouse",
                                                "ShortPolo", "LongPolo", "SportSweater",
                                                "CozySweater", "Blazer", "ShortTrouser",
                                                "LongTrouser", "ShortSkirt", "LongSkirt",
                                                "YogaPants", "ShortDress", "LongDress",
                                                "Jackets", "Coats" },

            "jackets" => new string[] {     "ShortTShirt", "LongTShirt", "ShortShirt",
                                                "LongShort", "ShortBlouse", "LongBlouse",
                                                "CozySweater", "Blazer", "ShortTrouser",
                                                "LongTrouser", "ShortSkirt", "LongSkirt",
                                                "YogaPants", "ShortDress", "LongDress",
                                                "Sandals", "Trainers", "Shoes",
                                                "Boots" },

            "coats" => new string[] {       "ShortTShirt", "LongTShirt", "ShortShirt",
                                                "LongShort", "ShortBlouse", "LongBlouse",
                                                "ShortPolo", "LongPolo", "SportSweater",
                                                "CozySweater", "Blazer", "LongTrouser",
                                                "ShortSkirt", "LongSkirt", "YogaPants",
                                                "ShortDress", "LongDress", "Sandals",
                                                "Trainers", "Shoes", "Boots" },

            _ => new string[] { "Unknown" }
        };
    }
}
