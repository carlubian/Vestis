using OneOf;
using System;
using Vestis.Core.Failures;

namespace Vestis.Core
{
    /// <summary>
    /// Entry point for all Vestis-related code.
    /// </summary>
    public static class DressingRoom
    {
        public static OneOf<Wardrobe, IFailure> ForUser(string user)
        {
            if (Users.UserExists(user) is false)
                return new MissingOrUnknownUserFailure(user);

            return Wardrobe.CreateForUser(user);
        }
    }
}
