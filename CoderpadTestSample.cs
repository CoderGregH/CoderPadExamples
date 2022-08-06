using NUnit.Framework;
using NUnitLite;
using System;
using System.Reflection;
using FluentAssertions;


namespace CodepadTestSample
{
    class Program
    {
        static int Main(string[] args)
        {

            return new AutoRun(Assembly.GetCallingAssembly()).Execute(new String[] { "--labels=All" });
        }

        [TestFixture]
        public class MySimpleTests
        {
            [Test]
            public void TestCheckBoolean()
            {
                Assert.IsTrue(true);
            }

            [Test]
            public void TestCompareNumbers()
            {
                Assert.AreEqual(2, 2);
            }

            [Test]
            public void TestCompareStrings()
            {
                Assert.AreEqual("abc", "abc");
            }

            [Test]
            public void TestCompareLists()
            {
                Assert.AreEqual(new int[] { 1, 2, 3 }, new int[] { 1, 2, 3 });
            }

            [Test]
            public void TestCompareListsUsingFluent()
            {
                var actual = 1;
                actual.Should().Be(1);
            }
        }
    }
}
