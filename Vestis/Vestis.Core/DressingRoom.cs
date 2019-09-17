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
        internal static string AppDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        public static IEnumerable<string> ListAvailable() => Users.AllUsers();

        public static OneOf<Wardrobe, IFailure> ForUser(string user)
        {
            if (Users.UserExists(user) is false)
                return new MissingOrUnknownUserFailure(user);

            return Wardrobe.CreateForUser(user);
        }

        /// <summary>
        /// Modify the root directory used to hold app and
        /// user settings. When using this library from a
        /// universal app, call this method and pass the path
        /// to a legal storage directory.
        /// </summary>
        /// <param name="directory"></param>
        public static void SetAppDirectory(string directory)
        {
            AppDirectory = directory;
        }
    }
}
