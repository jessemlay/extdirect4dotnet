using System;
using System.Collections.Generic;
using System.Linq;
using ExtDirect4DotNet.Test.TestDirectActions;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace ExtDirect4DotNet.Test {
    [TestFixture]
    public class DirectProvider_Test : UnitTestBase {
        [Test]
        [Description("Tests the overall functionality of the DirectProvider when configured with IEnumerable<Type>.")]
        public void Can_provide() {
            string providerName = "Ext.app.REMOTING_API";
            string providerUrl = "http://www.SomeUrl.com/WebApp";
            DirectProvider provider = new DirectProvider(providerName, providerUrl);
            List<Type> types = new List<Type> {
                typeof (DirectActionMock)
            };
            provider.Configure(types);
            string result = provider.ToString();

            Assert.IsNotNullOrEmpty(result);
            Assert.IsTrue(result.StartsWith(string.Format("{0} = {{", providerName)));
            Assert.IsTrue(result.EndsWith(";"));

            string resultContentJson = result.Substring(result.IndexOf('{')).Trim(';');
            JObject resultContentJObject = JObject.Parse(resultContentJson);

            Assert.AreEqual("remoting", (string) resultContentJObject["type"], CannotReadJTokenNamed("type"));
            Assert.AreEqual(providerUrl, (string) resultContentJObject["url"], CannotReadJTokenNamed("url"));

            IEnumerable<JToken> methodObjects = from p in resultContentJObject["actions"]["DirectActionMock"].Children()
                                                select p;

            JToken firstMethodObject = methodObjects.First();
            Assert.AreEqual("I_take_and_return_a_person_object", (string) firstMethodObject["name"], CannotReadJTokenNamed("name"));
            Assert.AreEqual(4, (int) firstMethodObject["len"], CannotReadJTokenNamed("len"));
            Assert.AreEqual(false, (bool) firstMethodObject["formHandler"], CannotReadJTokenNamed("formHandler"));

            JToken secondMethodObject = methodObjects.Skip(1).First();
            Assert.AreEqual("SayHelloTo", (string) secondMethodObject["name"], CannotReadJTokenNamed("name"));
            Assert.AreEqual(1, (int) secondMethodObject["len"], CannotReadJTokenNamed("len"));
            Assert.AreEqual(false, (bool) secondMethodObject["formHandler"], CannotReadJTokenNamed("formHandler"));
        }
    }
}