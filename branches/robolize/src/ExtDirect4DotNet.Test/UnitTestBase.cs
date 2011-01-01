using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace ExtDirect4DotNet.Test {
    /// <summary>
    /// A base class for unit tests.
    /// </summary>
    public class UnitTestBase {
        [SetUp]
        public virtual void SetUp() {
        }

        [TearDown]
        public virtual void TearDown() {
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup() {
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown() {
        }

        /// <summary>
        /// Prints a message for Assert failures.
        /// </summary>
        /// <param name="jTokenName">Name of the <see cref="JToken"/>.</param>
        /// <returns></returns>
        public static string CannotReadJTokenNamed(string jTokenName) {
            return string.Format("Cannot read the JToken named \"{0}\".", jTokenName);
        }
    }
}