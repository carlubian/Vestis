using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Vestis.Core.Model;

namespace Vestis.Test
{
    [TestClass]
    public class TestGarment
    {
        [TestMethod]
        public void TestAgeInYears()
        {
            var date1 = new PurchaseDate("Summer 2019");
            var age1 = date1.AgeInYears;

            var date2 = new PurchaseDate("Spring 2019");
            var age2 = date2.AgeInYears;

            var date3 = new PurchaseDate("Winter 2019");
            var age3 = date3.AgeInYears;

            var date4 = new PurchaseDate("Autumn 2018");
            var age4 = date4.AgeInYears;
        }
    }
}
