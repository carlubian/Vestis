using ConfigAdapter.Xml;
using DotNet.Misc.Extensions.Linq;
using OneOf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
            foreach (var garment in config.Read("Clothes").Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries))
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

        internal static void DeleteUser(string name)
        {
            var path = Path.Combine(DressingRoom.AppDirectory, "Vestis");
            var config = XmlConfig.From(Path.Combine(path, "GlobalSettings.xml"));

            config.DeleteKey($"UserProfiles:{name}");

            if (Directory.Exists(Path.Combine(path, "Profiles")))
            {
                path = Path.Combine(path, "Profiles");
                if (File.Exists(Path.Combine(path, $"{name}.Wardrobe.xml")))
                    File.Delete(Path.Combine(path, $"{name}.Wardrobe.xml"));
            }
        }

        internal static void EditUser(string oldName, string newName, string color, string icon)
        {
            var path = Path.Combine(DressingRoom.AppDirectory, "Vestis");

            if (!oldName.Equals(newName))
            {
                // Change profile name
                var config = XmlConfig.From(Path.Combine(path, "GlobalSettings.xml"));
                config.DeleteKey($"UserProfiles:{oldName}");
                config.Write($"UserProfiles:{newName}", "Local");

                if (Directory.Exists(Path.Combine(path, "Profiles")))
                {
                    path = Path.Combine(path, "Profiles");
                    if (File.Exists(Path.Combine(path, $"{oldName}.Wardrobe.xml")))
                    {
                        File.Move(Path.Combine(path, $"{oldName}.Wardrobe.xml"), Path.Combine(path, $"{newName}.Wardrobe.xml"));
                        config = XmlConfig.From(Path.Combine(path, $"{newName}.Wardrobe.xml"));
                        config.Write("UserProfile", newName);
                    }
                }
            }

            path = Path.Combine(DressingRoom.AppDirectory, "Vestis");
            // Update profile preferences
            if (Directory.Exists(Path.Combine(path, "Profiles")))
            {
                path = Path.Combine(path, "Profiles");
                if (File.Exists(Path.Combine(path, $"{newName}.Wardrobe.xml")))
                {
                    var config = XmlConfig.From(Path.Combine(path, $"{newName}.Wardrobe.xml"));
                    config.Write("ProfileColor", color);
                    config.Write("ProfileIcon", icon);
                }
            }
        }

        internal static string AddGarment(string user, Garment garment)
        {
            var path = Path.Combine(DressingRoom.AppDirectory, "Vestis");
            path = Path.Combine(path, "Profiles");
            var config = XmlConfig.From(Path.Combine(path, $"{user}.Wardrobe.xml"));

            // Create garment ID
            var clothes = config.Read("Clothes");
            var id = "";

            if (clothes is "" || clothes is null)
                id = "Garment_01";
            else
            {
                var each = clothes.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                var lastID = each.Reverse().First().Split('_')[1];
                id = $"Garment_{int.Parse(lastID) + 1}";
            }

            config.Write("Clothes", $"{clothes} {id}");

            // Add garment section
            config.Write($"{id}:Name", garment.Name);
            config.Write($"{id}:Type", garment.Type.ToString());
            config.Write($"{id}:PurchaseDate", garment.PurchaseDate.ToString());
            config.Write($"{id}:ColorTags", garment.ColorTags.Stringify(n => n, " "));
            config.Write($"{id}:StyleTags", garment.StyleTags.Stringify(n => n, " "));
            config.Write($"{id}:UserTags", " "); //TODO

            return id;
        }

        internal static void RemoveGarment(string user, string garment)
        {
            var path = Path.Combine(DressingRoom.AppDirectory, "Vestis");
            path = Path.Combine(path, "Profiles");
            var config = XmlConfig.From(Path.Combine(path, $"{user}.Wardrobe.xml"));

            var clothes = config.Read("Clothes");
            var each = clothes.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)
                .Except(garment.Enumerate());
            config.Write("Clothes", each.Stringify(n => n, " "));

            config.DeleteSection(garment);
        }

        internal static void UpdateGarment(string user, Garment garment)
        {
            var path = Path.Combine(DressingRoom.AppDirectory, "Vestis");
            path = Path.Combine(path, "Profiles");
            var config = XmlConfig.From(Path.Combine(path, $"{user}.Wardrobe.xml"));
            var id = garment.ID;

            // Overwrite garment values
            config.Write($"{id}:Name", garment.Name);
            config.Write($"{id}:Type", garment.Type.ToString());
            config.Write($"{id}:PurchaseDate", garment.PurchaseDate.ToString());
            config.Write($"{id}:ColorTags", garment.ColorTags.Stringify(n => n, " "));
            config.Write($"{id}:StyleTags", garment.StyleTags.Stringify(n => n, " "));
            config.Write($"{id}:UserTags", " "); //TODO
        }
    }
}
