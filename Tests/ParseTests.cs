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
        private Parser Prs;
        [SetUp]
        public void CreateFields()
        {
            Dictionary<string, int> op = new Dictionary<string, int>();
            op.Add("+", 6);
            op.Add("-", 5);
            op.Add("*", 3);
            op.Add("/", 4);
            op.Add("^", 2);
            op.Add("!", 1);
            Prs = new Parser(op);
        }
        [Test]
        public void SimpleSum()
        {
            string expression = "x+y";
            Dictionary<string, double> vals = new Dictionary<string, double>();
            vals.Add("x", 5);
            vals.Add("y", 2);
            double expected = 7;
            double actual = Prs.ProceedParse(expression, vals);
            Assert.AreEqual(expected, actual);
        }
    }
}
