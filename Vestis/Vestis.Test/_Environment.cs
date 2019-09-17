using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Vestis.Test
{
    [TestClass]
    public class _Environment
    {
        [AssemblyInitialize]
        public static void SetUp(TestContext _)
        {
            // Replace actual files with test files
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            directory = Path.Combine(directory, "Vestis");

            // Global settings
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var path = Path.Combine(directory, "GlobalSettings.xml");
            if (File.Exists(path))
                File.Move(path, Path.Combine(directory, "GlobalSettings.Real.xml"));

            File.Copy("GlobalSettings.xml", path);

            // User settings
            directory = Path.Combine(directory, "Profiles");

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            path = Path.Combine(directory, "TestUserOne.Wardrobe.xml");
            if (File.Exists(path))
                File.Move(path, Path.Combine(directory, "TestUserOne.Wardrobe.Real.xml"));

            File.Copy("TestUserOne.Wardrobe.xml", path);
        }

        [AssemblyCleanup]
        public static void TearDown()
        {
            // Restore actual files
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            directory = Path.Combine(directory, "Vestis");

            // Global settings
            var path = Path.Combine(directory, "GlobalSettings.xml");
            File.Delete(path);

            if (File.Exists(Path.Combine(directory, "GlobalSettings.Real.xml")))
                File.Move(Path.Combine(directory, "GlobalSettings.Real.xml"), path);

            // User settings
            directory = Path.Combine(directory, "Profiles");

            path = Path.Combine(directory, "TestUserOne.Wardrobe.xml");
            File.Delete(path);

            if (File.Exists(Path.Combine(directory, "TestUserOne.Wardrobe.Real.xml")))
                File.Move(Path.Combine(directory, "TestUserOne.Wardrobe.Real.xml"), path);
        }
    }
}
