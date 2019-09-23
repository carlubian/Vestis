﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Vestis.Core.Model
{
    public class PurchaseDate
    {
        public Seasons Season { get; internal set; }
        public int Year { get; internal set; }

        /// <summary>
        /// Returns the number of seasons that have occured
        /// since the garment was purchased. Current season
        /// will return 0; Same season last year will return 4.
        /// </summary>
        public int AgeInSeasons
        {
            get
            {
                return ((DateTime.Today.Year - 1) * 12 + DateTime.Today.Month
                        -
                        ((Year - 1) * 12 + 3 * (int)Season)
                    ) / 3;
            }
        }

        /// <summary>
        /// Returns the number of years since the garment was
        /// purchased. If the garment has been purchased less than
        /// four seasons ago (meaning less than a full year) the
        /// result will be 0. Otherwise, an integer number measuring
        /// the full years of ownership.
        /// </summary>
        public int AgeInYears
        {
            get
            {
                return AgeInSeasons / 4;
            }
        }

        public PurchaseDate(string text)
        {
            var parts = text.Split(' ');
            Year = int.Parse(parts[1]);
            Season = SeasonsUtil.Parse(parts[0]);
        }

        public override string ToString() => $"{Season.ToString()} {Year}";
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
