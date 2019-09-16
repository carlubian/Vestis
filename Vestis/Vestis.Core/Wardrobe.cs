using OneOf;
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
        private IEnumerable<Garment> Garments { get; }

        internal static OneOf<Wardrobe, IFailure> CreateForUser(string user)
        {
            // Parse clothes for user
            var garments = Users.GarmentsOf(user);
            if (garments.IsT0)
                return new Wardrobe(garments.AsT0);
            else
                return OneOf<Wardrobe, IFailure>.FromT1(garments.AsT1);
        }

        private Wardrobe(IEnumerable<Garment> garments)
        {
            Garments = garments;
        }
    }
}
