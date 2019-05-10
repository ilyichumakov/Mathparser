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
        private Dictionary<string, int> op;
        private Dictionary<string, double> vals;
        [SetUp]
        public void CreateFields()
        {
            op = new Dictionary<string, int>();
            op.Add("+", 6);
            op.Add("-", 5);
            op.Add("*", 3);
            op.Add("/", 4);
            op.Add("^", 2);
            op.Add("!", 1);
            Prs = new Parser(op);
            vals = new Dictionary<string, double>();
            vals.Clear();
        }
        [Test]
        public void SimpleSum()
        {
            string expression = "x+y";           
            vals.Add("x", 5);
            vals.Add("y", 2);
            double expected = 7;
            double actual = Prs.ProceedParse(expression, vals);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SimpleMul()
        {
            string expression = "x*y";            
            vals.Add("x", 5);
            vals.Add("y", 2);
            double expected = 10;
            double actual = Prs.ProceedParse(expression, vals);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SimplePow()
        {
            string expression = "x^y";
            vals.Add("x", 3);
            vals.Add("y", 4);
            double expected = 81;
            double actual = Prs.ProceedParse(expression, vals);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SimpleDiv()
        {
            string expression = "x/y";            
            vals.Add("x", 5);
            vals.Add("y", 2);
            double expected = 2.5;
            double actual = Prs.ProceedParse(expression, vals);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SimpleSub()
        {
            string expression = "x-y";            
            vals.Add("x", 5);
            vals.Add("y", 2);
            double expected = 3;
            double actual = Prs.ProceedParse(expression, vals);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MixedPriority()
        {
            string expression = "x*y-z+3*y*z/x";           
            vals.Add("x", 6);
            vals.Add("y", 2);
            vals.Add("z", 8);
            vals.Add("3", 3);
            double expected = 12;
            double actual = Prs.ProceedParse(expression, vals);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IWantBrackets()
        {
            string expression = "(2-x)";
            List<string> actual = Prs.ParseExpression(expression);
            List<string> expected = new List<string>() { "2", "-", "x" };
            Assert.AreEqual(expected, actual);
        }
    }
}
