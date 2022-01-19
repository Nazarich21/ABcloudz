using NUnit.Framework;
using Sum_of_sequence;
using System;

namespace Sum_of_sequence.Test
{
    public class Tests
    {
        private static object[] TestData = new object[]
        {
            new object[] { new string[] { "9.9999e+4", "1.0e+0"}, "1.0e+5" },
            new object[] { new string[] { "5.0e-1", "5.0e-1", "9.0e0", "9.0e1", "1.0e2"}, "2.0e+2"},
            new object[] { new string[] { "5.0e+100", "5.0e-1", "9.0e0", "9.0e1", "1.0e2"}, "5.0e+100"},
        };

        [SetUp]
        public void Setup()
        {
        }

        [Category("Add")]
        [TestCase("2.222e+5", "2.22e+0", "2.2220222e+5")]
        [TestCase("2.222e+5", "0.0e+0", "2.222e+5")]
        public void Adding_Positive_Numbers(string s1, string s2, string expected)
        {
            var firstNumber = new Exponent(s1);
            var secondNumber = new Exponent(s2);
            //Exponent.ToZeroExponent(firstNumber);
            //Exponent.ToZeroExponent(secondNumber);
            Exponent res = firstNumber + secondNumber;
            Assert.AreEqual(expected,(res.ToString()));
            Assert.AreEqual(s1,(firstNumber.ToString()));
            Assert.AreEqual(s2,(secondNumber.ToString()));
        }

        [Category("Add")]
        [Test]
        [TestCaseSource("TestData")]
        public void TestAdd(string[] data, string expected)
        {
            Exponent res = Exponent.Zero;
            
            foreach (string str in data)

            res += new Exponent(str);
            Assert.AreEqual(expected, res.ToString());
          
        }


        [Test]
        public void Pozitive_With_Comma_Check()
        {
            Assert.Throws<InvalidOperationException>(() => new Exponent("999,9209e5"));
        }


        [Test]
        public void Pozitive_With_Letters_Check()
        {
            Assert.Throws<InvalidOperationException>(() => new Exponent("9.FFF999209e5"));
        }


        [Test]
        public void Number_Check()
        {
            Assert.Throws<InvalidOperationException>(() => new Exponent("213213"));
        }
    }
}