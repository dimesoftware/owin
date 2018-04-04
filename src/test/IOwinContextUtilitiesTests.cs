using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Owin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Dime.Owin.Tests
{
    [TestClass]
    public class IOwinContextUtilitiesTests
    {
        [TestMethod]
        public void OwinContextUtilities_SupportsEncoding_HasHeader_HasEncoding_ShouldReturnTrue()
        {
            ConcurrentDictionary<string, string[]> dict = new ConcurrentDictionary<string, string[]>(
                new Dictionary<string, string[]>()
                {
                    {
                        "Accept-Encoding",  new[] { "how-zekers" }
                    },
                });

            HeaderDictionary headerDictionary = new HeaderDictionary(
                new Dictionary<string, string[]>()
                {
                    {
                        "Accept-Encoding",  new[] { "how-zekers" }
                    },
                });

            FormCollection form = new FormCollection(dict);
            var owinRequestMock = new Mock<IOwinRequest>();
            owinRequestMock.Setup(x => x.Query).Returns(form);
            owinRequestMock.Setup(x => x.Headers).Returns(headerDictionary);
            IOwinRequest owinRequest = owinRequestMock.Object;

            Mock<IOwinContext> mock = new Mock<IOwinContext>(MockBehavior.Strict);
            mock.Setup(c => c.Request).Returns(owinRequest);
            Assert.IsTrue(mock.Object.SupportsEncoding("how-zekers"));
        }

        [TestMethod]
        public void OwinContextUtilities_SupportsEncoding_HasHeader_HasNoEncoding_ShouldReturnFalse()
        {
            ConcurrentDictionary<string, string[]> dict = new ConcurrentDictionary<string, string[]>(
                new Dictionary<string, string[]>()
                {
                    {
                        "Accept-Encoding",  new[] { "how-zekers" }
                    },
                });

            HeaderDictionary headerDictionary = new HeaderDictionary(
                new Dictionary<string, string[]>()
                {
                    {
                        "Accept-Encoding",  new[] { "paul-schampers-en-berten-maillot" }
                    },
                });

            FormCollection form = new FormCollection(dict);
            var owinRequestMock = new Mock<IOwinRequest>();
            owinRequestMock.Setup(x => x.Query).Returns(form);
            owinRequestMock.Setup(x => x.Headers).Returns(headerDictionary);
            IOwinRequest owinRequest = owinRequestMock.Object;

            Mock<IOwinContext> mock = new Mock<IOwinContext>(MockBehavior.Strict);
            mock.Setup(c => c.Request).Returns(owinRequest);
            Assert.IsTrue(!mock.Object.SupportsEncoding("how-zekers"));
        }

        [TestMethod]
        public void OwinContextUtilities_IsAjaxRequest_HasXMLHttpRequest_ShouldReturnTrue()
        {
            ConcurrentDictionary<string, string[]> dict = new ConcurrentDictionary<string, string[]>(
                new Dictionary<string, string[]>()
                {
                    {
                        "X-Requested-With",  new[] { "XMLHttpRequest" }
                    },
                });

            FormCollection form = new FormCollection(dict);
            var owinRequestMock = new Mock<IOwinRequest>();
            owinRequestMock.Setup(x => x.Query).Returns(form);
            owinRequestMock.Setup(x => x.Headers).Returns(new Mock<IHeaderDictionary>().Object);
            IOwinRequest owinRequest = owinRequestMock.Object;

            Mock<IOwinContext> mock = new Mock<IOwinContext>(MockBehavior.Strict);
            mock.Setup(c => c.Request).Returns(owinRequest);

            Assert.IsTrue(!mock.Object.IsAjaxRequest());
        }

        [TestMethod]
        public void OwinContextUtilities_IsAjaxRequest_HasNoXMLHttpRequest_ShouldReturnFalse()
        {
            ConcurrentDictionary<string, string[]> dict = new ConcurrentDictionary<string, string[]>(
                new Dictionary<string, string[]>()
                {
                    {
                        "X-Requested-With",  new[] { "He's ﻿a long-legged pissed-off puerto rican" }
                    },
                });

            HeaderDictionary header = new HeaderDictionary(
                new Dictionary<string, string[]>()
                {
                    {
                        "NOT-X-Requested-With",  new[] { "False" }
                    },
                });

            FormCollection form = new FormCollection(dict);
            var owinRequestMock = new Mock<IOwinRequest>();
            owinRequestMock.Setup(x => x.Query).Returns(form);
            owinRequestMock.Setup(x => x.Headers).Returns(header);
            IOwinRequest owinRequest = owinRequestMock.Object;

            Mock<IOwinContext> mock = new Mock<IOwinContext>(MockBehavior.Strict);
            mock.Setup(c => c.Request).Returns(owinRequest);
        
            Assert.IsTrue(!mock.Object.IsAjaxRequest());
        }

        [TestMethod]
        public void OwinContextUtilities_IsWebSocketRequest_HasSignalR_ShouldReturnTrue()
        {
            var owinRequestMock = new Mock<IOwinRequest>(MockBehavior.Strict);
            owinRequestMock.Setup(x => x.Path).Returns(new PathString("/alainprovist/signalr/"));
            IOwinRequest owinRequest = owinRequestMock.Object;

            Mock<IOwinContext> mock = new Mock<IOwinContext>(MockBehavior.Strict);
            mock.Setup(c => c.Request).Returns(owinRequest);
            Assert.IsTrue(mock.Object.IsWebSocketRequest());
        }

        [TestMethod]
        public void OwinContextUtilities_IsWebSocketRequest_HasNoSignalR_ShouldReturnFalse()
        {
            var owinRequestMock = new Mock<IOwinRequest>(MockBehavior.Strict);
            owinRequestMock.Setup(x => x.Path).Returns(new PathString("/alainprovist/"));
            IOwinRequest owinRequest = owinRequestMock.Object;

            Mock<IOwinContext> mock = new Mock<IOwinContext>(MockBehavior.Strict);
            mock.Setup(c => c.Request).Returns(owinRequest);
            Assert.IsTrue(!mock.Object.IsWebSocketRequest());
        }

        [TestMethod]
        public void OwinContextUtilities_IsFileRequest_HasFileExtension_ShouldReturnTrue()
        {
            var owinRequestMock = new Mock<IOwinRequest>(MockBehavior.Strict);
            owinRequestMock.Setup(x => x.Path).Returns(new PathString("/alainprovist/signalr.jpeg"));
            IOwinRequest owinRequest = owinRequestMock.Object;

            Mock<IOwinContext> mock = new Mock<IOwinContext>(MockBehavior.Strict);
            mock.Setup(c => c.Request).Returns(owinRequest);
            Assert.IsTrue(mock.Object.IsFileRequest());
        }

        [TestMethod]
        public void OwinContextUtilities_IsFileRequest_HasNoFileExtension_ShouldReturnFalse()
        {
            var owinRequestMock = new Mock<IOwinRequest>(MockBehavior.Strict);
            owinRequestMock.Setup(x => x.Path).Returns(new PathString("/alainprovist/signalr"));
            IOwinRequest owinRequest = owinRequestMock.Object;

            Mock<IOwinContext> mock = new Mock<IOwinContext>(MockBehavior.Strict);
            mock.Setup(c => c.Request).Returns(owinRequest);
            Assert.IsTrue(!mock.Object.IsFileRequest());
        }
    }
}
