using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuntionParser;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class ParseTests
    {
        [SetUp]
        public void CreateFields()
        {

        }
        [Test]
        public void SimpleSum()
        {
            string expression = "x+y";
            Dictionary<string, double> vals = new Dictionary<string, double>();
            vals.Add("x", 5);
            vals.Add("y", 2);
            double expected = 7;
            double actual = 
        }
    }
}
