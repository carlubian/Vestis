using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OneOf;
using System;
using System.Collections.Generic;
using System.Text;
using Vestis.Core;
using Vestis.Core.Failures;
using Vestis.Test.Util;

namespace Vestis.Test
{
    [TestClass]
    public class TestWardrobe
    {
        [TestMethod]
        public void TestUserWardrobe()
        {
            OneOf<Wardrobe, IFailure> result = new UninitializedVariable();
            Action act = () => result = DressingRoom.ForUser("TestUserOne");

            act.Should().NotThrow();
            var wardrobe = result.AsT0;
            wardrobe.Should().NotBeNull();

            act = () => result = DressingRoom.ForUser("TestUserTwo");

            act.Should().NotThrow();
            var failure = result.AsT1;
            failure.Should().NotBeNull();
        }
    }
}
