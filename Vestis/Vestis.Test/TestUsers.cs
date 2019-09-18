using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using Vestis.Core;
using Vestis.Core.Failures;
using Vestis.Core.Model;
using Vestis.Test.Util;

namespace Vestis.Test
{
    [TestClass]
    public class TestUsers
    {
        [TestMethod]
        public void TestListUsers()
        {
            IEnumerable<string> users = null;
            Action act = () => users = Users.AllUsers();

            act.Should().NotThrow();
            users.Should().NotBeNull();
            users.Should().HaveCount(2);
            users.Should().ContainInOrder("TestUserOne", "TestUserTwo");
        }

        [TestMethod]
        public void TestUserExists()
        {
            Users.UserExists("TestUserOne").Should().BeTrue();
            Users.UserExists("TestUserThree").Should().BeFalse();
            Users.UserExists("TestUserTwo").Should().BeTrue();
        }

        [TestMethod]
        public void TestUserGarments()
        {
            OneOf<IEnumerable<Garment>, IFailure> result = new UninitializedVariable();

            Action act = () => result = Users.GarmentsFor("TestUserOne");
            act.Should().NotThrow();

            var garments = result.AsT0;
            garments.Should().NotBeNull();
            garments.Should().HaveCount(2);
            garments.Select(g => g.ID).Should().ContainInOrder("Garment01", "Garment02");
            garments.Select(g => g.Name).Should().ContainInOrder("Blue and white polo", "Plain green t-shirt");

            act = () => result = Users.GarmentsFor("TestUserTwo");
            act.Should().NotThrow();
            var failure = result.AsT1;
            failure.Should().NotBeNull();
        }
    }
}
