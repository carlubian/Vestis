﻿using OneOf;
using System;
using System.Collections.Generic;
using System.Text;
using Vestis.Core.Failures;
using Vestis.Core.Model;

namespace Vestis.Core
{
    /// <summary>
    /// Represents all the clothes owned by a
    /// specific user.
    /// </summary>
    public class Wardrobe
    {
        public IEnumerable<Garment> Garments { get; }

        public string Username { get; }

        public string ProfileColor { get; }

        public string ProfileIcon { get; }

        internal static OneOf<Wardrobe, IFailure> CreateForUser(string user)
        {
            IEnumerable<Garment> grm = null;
            string clr = null;
            string icn = null;

            // Parse clothes for user
            var garments = Users.GarmentsFor(user);
            if (garments.IsT0)
                grm = garments.AsT0;
            else
                return OneOf<Wardrobe, IFailure>.FromT1(garments.AsT1);

            // Parse profile color
            clr = Users.ColorFor(user);

            // Parse profile icon
            icn = Users.IconFor(user);

            return new Wardrobe(grm, user, clr, icn);
        }

        private Wardrobe(IEnumerable<Garment> garments, string user, string color, string icon)
        {
            Garments = garments;
            Username = user;
            ProfileColor = color;
            ProfileIcon = icon;
        }

        public void AddGarment(Garment garment)
        {
            Users.AddGarment(Username, garment);
            (Garments as IList<Garment>).Add(garment);
        }
    }
}
