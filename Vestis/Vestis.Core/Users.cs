using ConfigAdapter.Xml;
using OneOf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Vestis.Core.Failures;
using Vestis.Core.Model;

namespace Vestis.Core
{
    /// <summary>
    /// Manages users for Vestis.
    /// </summary>
    internal static class Users
    {
        internal static bool UserExists(string user)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            path = Path.Combine(path, "Vestis");
            path = Path.Combine(path, "GlobalSettings.xml");

            var config = XmlConfig.From(path);
            foreach (var profile in config.SettingsIn("UserProfiles"))
                if (profile.Key.Equals(user))
                    return true;

            return false;
        }

        internal static OneOf<IEnumerable<Garment>, IFailure> GarmentsOf(string user)
        {
            var result = new List<Garment>();

            var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            path = Path.Combine(path, "Vestis");
            path = Path.Combine(path, "Profiles");
            path = Path.Combine(path, $"{user}.Wardrobe.xml");

            var config = XmlConfig.From(path);
            foreach (var garment in config.Read("Clothes").Split(' '))
                // TODO Validate properties
                result.Add(new Garment
                {
                    Name = config.Read($"{garment}:Name")
                });

            return result;
        }
    }
}
