using System;
using System.Collections.Generic;
using System.Text;

namespace Vestis.Core.Model
{
    public class PurchaseDate
    {
        public Seasons Season { get; internal set; }
        public int Year { get; internal set; }

        public int AgeInSeasons
        {
            get
            {
                // TODO
                return -1;
            }
        }

        public int AgeInYears
        {
            get
            {
                return ((DateTime.Today.Year * 12) - (Year + 3 * (int)Season)) / 12;
            }
        }

        internal PurchaseDate(string text)
        {
            var parts = text.Split(' ');
            Year = int.Parse(parts[1]);
            Season = SeasonsUtil.Parse(parts[0]);
        }
    }

    public enum Seasons
    {
        Winter = 0,
        Spring = 1,
        Summer = 2,
        Autumn = 3
    }

    internal class SeasonsUtil
    {
        public static Seasons Parse(string text)
        {
            switch (text.ToLowerInvariant())
            {
                case "winter":
                    return Seasons.Winter;
                case "spring":
                    return Seasons.Spring;
                case "summer":
                    return Seasons.Summer;
                case "autumn":
                    return Seasons.Autumn;
            }
            return Seasons.Winter;
        }
    }
}
