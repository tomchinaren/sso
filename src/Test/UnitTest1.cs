using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using sso;

namespace Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestGetNewSessionString()
        {
            var session = new UserSession();
            var sessionString = session.CreateSessionString();
            Assert.IsTrue(!string.IsNullOrWhiteSpace(sessionString), "session非空");
            Assert.IsTrue(!string.IsNullOrWhiteSpace(session.Token),"token非空");
            Console.WriteLine("sessionString:{0}", sessionString);
            Assert.IsTrue(sessionString.Length>=16,"验证:长度>=16,实际{0}", sessionString.Length);
        }

        [TestMethod]
        public void TestDecryptSessionString()
        {
            var session = new UserSession();
            var sessionString = session.CreateSessionString();
            Assert.IsTrue(!string.IsNullOrWhiteSpace(sessionString),"session非空");
            Assert.IsTrue(!string.IsNullOrWhiteSpace(session.Token),"token非空");

            var session2 = new UserSession(sessionString);
            Assert.AreEqual(session.Token, session2.Token);

        }
    }
}
