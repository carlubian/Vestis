using OneOf;
using System;
using System.Collections.Generic;
using System.Text;
using Vestis.Core.Failures;

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
        Sandals,
        Trainers,
        Shoes,
        Boots,
        Jackets,
        Coats,
        Unknown
    }

    static class ClothingTypeUtil
    {
        public static ClothingType Parse(string text)
        {
            switch (text.ToLowerInvariant())
            {
                case "shorttshirt":
                    return ClothingType.ShortTShirt;
                case "longtshirt":
                    return ClothingType.LongTShirt;
                case "shortshirt":
                    return ClothingType.ShortShirt;
                case "longshirt":
                    return ClothingType.LongShirt;
                case "shortblouse":
                    return ClothingType.ShortBlouse;
                case "longblouse":
                    return ClothingType.LongBlouse;
                case "shortpolo":
                    return ClothingType.ShortPolo;
                case "longpolo":
                    return ClothingType.LongPolo;
                case "sportsweater":
                    return ClothingType.SportSweater;
                case "cozysweater":
                    return ClothingType.CozySweater;
                case "blazer":
                    return ClothingType.Blazer;
                case "shorttrouser":
                    return ClothingType.ShortTrouser;
                case "longtrouser":
                    return ClothingType.LongTrouser;
                case "shortskirt":
                    return ClothingType.ShortSkirt;
                case "longskirt":
                    return ClothingType.LongSkirt;
                case "yogapants":
                    return ClothingType.YogaPants;
                case "sandals":
                    return ClothingType.Sandals;
                case "trainers":
                    return ClothingType.Trainers;
                case "shoes":
                    return ClothingType.Shoes;
                case "boots":
                    return ClothingType.Boots;
                case "jackets":
                    return ClothingType.Jackets;
                case "coats":
                    return ClothingType.Coats;
            }
            return ClothingType.Unknown;
        }
    }
}
