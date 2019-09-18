using ConfigAdapter.Xml;
using Ionic.Zip;
using OneOf;
using System;
using System.Collections.Generic;
using System.IO;
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
        public static string AppDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        public static IEnumerable<string> ListAvailable() => Users.AllUsers();

        public static string ColorFor(string user) => Users.ColorFor(user);

        public static string IconFor(string user) => Users.IconFor(user);

        public static OneOf<Wardrobe, IFailure> ForUser(string user)
        {
            if (Users.UserExists(user) is false)
                return new MissingOrUnknownUserFailure(user);

            return Wardrobe.CreateForUser(user);
        }

        public static void CreateNew(string user, string color, string icon) => Users.AddUser(user, color, icon);

        public static bool WelcomePageSeen()
        {
            var path = Path.Combine(AppDirectory, "Vestis");
            path = Path.Combine(path, "GlobalSettings.xml");

            var config = XmlConfig.From(path);
            return config.Read("WelcomePageSeen") != null;
        }

        public static void SeeWelcomePage()
        {
            var path = Path.Combine(AppDirectory, "Vestis");
            path = Path.Combine(path, "GlobalSettings.xml");

            var config = XmlConfig.From(path);
            config.Write("WelcomePageSeen", "True");
        }

        public static void RestoreSettings()
        {
            var path = Path.Combine(AppDirectory, "Vestis");
            File.Delete(Path.Combine(path, "GlobalSettings.xml"));

            if (Directory.Exists(Path.Combine(path, "Profiles")))
                Directory.Delete(Path.Combine(path, "Profiles"), true);
        }

        public static string MakeExportPackage()
        {
            var path = Path.Combine(AppDirectory, "Vestis");

            using (var zip = new ZipFile())
            {
                zip.AddFile(Path.Combine(path, "GlobalSettings.xml"), "");
                if (Directory.Exists(Path.Combine(path, "Profiles")))
                    zip.AddDirectory(Path.Combine(path, "Profiles"), "Profiles");
                zip.Save(Path.Combine(path, "export.zip"));
            }

            return Path.Combine(path, "export.zip");
        }

        public static void ImportPackage(string package)
        {
            var path = Path.Combine(AppDirectory, "Vestis");

            using (var zip = new ZipFile(Path.Combine(path, package)))
            {
                zip.ExtractAll(Path.Combine(path, "import"), ExtractExistingFileAction.OverwriteSilently);
            }

            var import = Path.Combine(path, "import");

            File.Delete(Path.Combine(path, "GlobalSettings.xml"));
            File.Move(Path.Combine(import, "GlobalSettings.xml"), Path.Combine(path, "GlobalSettings.xml"));

            if (Directory.Exists(Path.Combine(path, "Profiles")))
                Directory.Delete(Path.Combine(path, "Profiles"), true);

            path = Path.Combine(path, "Profiles");
            var profiles = Path.Combine(import, "Profiles");

            if (Directory.Exists(profiles))
                Directory.Move(profiles, path);

            path = Path.Combine(AppDirectory, "Vestis");
            Directory.Delete(Path.Combine(path, "import"), true);
            File.Delete(Path.Combine(path, package));
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
