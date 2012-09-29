// 
// Copyright (c) 2004-2011 Jaroslaw Kowalski <jaak@jkowalski.net>
// 
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions 
// are met:
// 
// * Redistributions of source code must retain the above copyright notice, 
//   this list of conditions and the following disclaimer. 
// 
// * Redistributions in binary form must reproduce the above copyright notice,
//   this list of conditions and the following disclaimer in the documentation
//   and/or other materials provided with the distribution. 
// 
// * Neither the name of Jaroslaw Kowalski nor the names of its 
//   contributors may be used to endorse or promote products derived from this
//   software without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF 
// THE POSSIBILITY OF SUCH DAMAGE.
// 

using System;
using System.Xml;
using System.Reflection;

using NLog;
using NLog.Config;

using NUnit.Framework;

#if !NUNIT
    using SetUp = Microsoft.VisualStudio.TestTools.UnitTesting.TestInitializeAttribute;
    using TestFixture = Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute;
    using Test = Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute;
    using TearDown =  Microsoft.VisualStudio.TestTools.UnitTesting.TestCleanupAttribute;
#endif

namespace NLog.UnitTests.LayoutRenderers
{
    using System.Globalization;
    using NLog.LayoutRenderers;

    [TestFixture]
    public class DateTests : NLogTestBase
    {
        [Test]
        public void DefaultDateTest()
        {
            LogManager.Configuration = CreateConfigurationFromString(@"
            <nlog>
                <targets><target name='debug' type='Debug' layout='${date}' /></targets>
                <rules>
                    <logger name='*' minlevel='Debug' writeTo='debug' />
                </rules>
            </nlog>");

            LogManager.GetLogger("d").Debug("zzz");
            DateTime dt = DateTime.ParseExact(GetDebugLastMessage("debug"), "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            DateTime now = DateTime.Now;

            Assert.IsTrue(Math.Abs((dt - now).TotalSeconds) < 5);
        }

        [Test]
        public void UniversalTimeTest()
        {
            var dt = new DateLayoutRenderer();
            dt.UniversalTime = true;
            dt.Format = "R";

            var ei = new LogEventInfo(LogLevel.Info, "logger", "msg");
            Assert.AreEqual(ei.TimeStamp.ToUniversalTime().ToString("R"), dt.Render(ei));
        }

        [Test]
        public void LocalTimeTest()
        {
            var dt = new DateLayoutRenderer();
            dt.UniversalTime = false;
            dt.Format = "R";

            var ei = new LogEventInfo(LogLevel.Info, "logger", "msg");
            Assert.AreEqual(ei.TimeStamp.ToString("R"), dt.Render(ei));
        }

        [Test]
        public void FormattedDateTest()
        {
            LogManager.Configuration = CreateConfigurationFromString(@"
            <nlog>
                <targets><target name='debug' type='Debug' layout='${date:format=yyyy-MM-dd}' /></targets>
                <rules>
                    <logger name='*' minlevel='Debug' writeTo='debug' />
                </rules>
            </nlog>");

            LogManager.GetLogger("d").Debug("zzz");
            AssertDebugLastMessage("debug", DateTime.Now.ToString("yyyy-MM-dd"));
        }
    }
}