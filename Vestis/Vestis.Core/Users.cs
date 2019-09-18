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
        internal static IEnumerable<string> AllUsers()
        {
            var path = Path.Combine(DressingRoom.AppDirectory, "Vestis");
            path = Path.Combine(path, "GlobalSettings.xml");

            var config = XmlConfig.From(path);
            foreach (var profile in config.SettingsIn("UserProfiles"))
                yield return profile.Key;
        }

        internal static bool UserExists(string user)
        {
            var path = Path.Combine(DressingRoom.AppDirectory, "Vestis");
            path = Path.Combine(path, "GlobalSettings.xml");

            var config = XmlConfig.From(path);
            foreach (var profile in config.SettingsIn("UserProfiles"))
                if (profile.Key.Equals(user))
                    return true;

            return false;
        }

        internal static OneOf<IEnumerable<Garment>, IFailure> GarmentsFor(string user)
        {
            var result = new List<Garment>();

            var path = Path.Combine(DressingRoom.AppDirectory, "Vestis");
            path = Path.Combine(path, "Profiles");
            path = Path.Combine(path, $"{user}.Wardrobe.xml");

            if (!File.Exists(path))
                return new UserHasNoWardrobeFailure(user);

            var config = XmlConfig.From(path);
            if (config.Read("Clothes") is null)
                return result;
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

        internal static string ColorFor(string user)
        {
            var path = Path.Combine(DressingRoom.AppDirectory, "Vestis");
            path = Path.Combine(path, "Profiles");
            path = Path.Combine(path, $"{user}.Wardrobe.xml");
            var config = XmlConfig.From(path);

            return config.Read("ProfileColor");
        }

        internal static string IconFor(string user)
        {
            var path = Path.Combine(DressingRoom.AppDirectory, "Vestis");
            path = Path.Combine(path, "Profiles");
            path = Path.Combine(path, $"{user}.Wardrobe.xml");
            var config = XmlConfig.From(path);

            return config.Read("ProfileIcon");
        }

        internal static void AddUser(string name, string color, string icon)
        {
            // All validation criteria are presumed to be successful.
            var path = Path.Combine(DressingRoom.AppDirectory, "Vestis");
            var config = XmlConfig.From(Path.Combine(path, "GlobalSettings.xml"));

            config.Write($"UserProfiles:{name}", "Local");

            if (!Directory.Exists(Path.Combine(path, "Profiles")))
                Directory.CreateDirectory(Path.Combine(path, "Profiles"));

            path = Path.Combine(path, "Profiles");
            config = XmlConfig.From(Path.Combine(path, $"{name}.Wardrobe.xml"));

            config.Write("UserProfile", name);
            config.Write("ProfileColor", color);
            config.Write("ProfileIcon", icon);
        }
    }
}
