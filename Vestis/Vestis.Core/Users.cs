using ConfigAdapter.Xml;
using OneOf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        internal static IEnumerable<string> AllUsers(string directory)
        {
            //var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            //path = Path.Combine(path, "Vestis");
            var path = Path.Combine(directory, "GlobalSettings.xml");

            var config = XmlConfig.From(path);
            foreach (var profile in config.SettingsIn("UserProfiles"))
                yield return profile.Key;
        }

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

            if (!File.Exists(path))
                return new UserHasNoWardrobeFailure(user);

            var config = XmlConfig.From(path);
            foreach (var garment in config.Read("Clothes").Split(' '))
            {
                result.Add(new Garment
                {
                    ID = garment,
                    Name = config.Read($"{garment}:Name"),
                    Type = ClothingTypeUtil.Parse(config.Read($"{garment}:Type")),
                    PurchaseDate = new PurchaseDate(config.Read($"{garment}:PurchaseDate")),
                    ColorTags = new ReadOnlyCollection<string>(config.Read($"{garment}:ColorTags").Split(' ')),
                    StyleTags = new ReadOnlyCollection<string>(config.Read($"{garment}:StyleTags").Split(' ')),
                    UserTags = new ReadOnlyCollection<string>(config.Read($"{garment}:UserTags").Split(' '))
                });
            }

            return result;
        }
    }
}
