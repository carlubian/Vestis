using OneOf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vestis.Core.Failures;

[assembly:InternalsVisibleTo("Vestis.Test")]
namespace Vestis.Core
{
    /// <summary>
    /// Entry point for all Vestis-related code.
    /// </summary>
    public static class DressingRoom
    {
        private static string _directory;

        public static IEnumerable<string> ListAvailable() => Users.AllUsers(_directory);

        public static OneOf<Wardrobe, IFailure> ForUser(string user)
        {
            if (Users.UserExists(user) is false)
                return new MissingOrUnknownUserFailure(user);

            return Wardrobe.CreateForUser(user);
        }

        public static void SetAppDirectory(string directory)
        {
            _directory = directory;
        }
    }
}
