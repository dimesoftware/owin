using Microsoft.Owin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Dime.Owin.Tests
{
    [TestClass]
    public class IOwinRequestUtilitiesTests
    {
        [TestMethod]
        public void OwinRequestUtilities_IsBearerTokenRequest_HasHeader_WithBearer_ShouldReturnTrue()
        {
            HeaderDictionary dict = new HeaderDictionary(
                new Dictionary<string, string[]>()
                {
                    {
                        "Authorization",  new[] { "Bearer i-am-so-lonely-i-paid-a-hobo-to-spoon-with-me" }
                    },
                });

            FormCollection form = new FormCollection(dict);
            var owinRequestMock = new Mock<IOwinRequest>();

            owinRequestMock.Setup(x => x.Headers).Returns(dict);
            IOwinRequest owinRequest = owinRequestMock.Object;

            Assert.IsTrue(owinRequest.IsBearerTokenRequest());
        }

        [TestMethod]
        public void OwinRequestUtilities_IsBearerTokenRequest_HasHeader_WithoutBearer_ShouldReturnFalse()
        {
            HeaderDictionary dict = new HeaderDictionary(
                new Dictionary<string, string[]>()
                {
                    {
                        "Authorization",  new[] { "i-am-so-lonely-i-paid-a-hobo-to-spoon-with-me" }
                    },
                });

            FormCollection form = new FormCollection(dict);
            var owinRequestMock = new Mock<IOwinRequest>();

            owinRequestMock.Setup(x => x.Headers).Returns(dict);
            IOwinRequest owinRequest = owinRequestMock.Object;

            Assert.IsTrue(!owinRequest.IsBearerTokenRequest());
        }

        [TestMethod]
        public void OwinRequestUtilities_IsBearerTokenRequest_HasNoHeader_WithoutBearer_ShouldReturnFalse()
        {
            HeaderDictionary dict = new HeaderDictionary(
                new Dictionary<string, string[]>()
                {
                    {
                        "Authorization-WRONG",  new[] { "i-am-so-lonely-i-paid-a-hobo-to-spoon-with-me" }
                    },
                });

            FormCollection form = new FormCollection(dict);
            var owinRequestMock = new Mock<IOwinRequest>();

            owinRequestMock.Setup(x => x.Headers).Returns(dict);
            IOwinRequest owinRequest = owinRequestMock.Object;

            Assert.IsTrue(!owinRequest.IsBearerTokenRequest());
        }

        [TestMethod]
        public void OwinRequestUtilities_IsSuppressedRequest_HasHeader_IsTrue_ShouldReturnTrue()
        {
            HeaderDictionary dict = new HeaderDictionary(
                new Dictionary<string, string[]>()
                {
                    {
                        "Suppress-Redirect",  new[] { "True" }
                    },
                });

            FormCollection form = new FormCollection(dict);
            var owinRequestMock = new Mock<IOwinRequest>();

            owinRequestMock.Setup(x => x.Headers).Returns(dict);
            IOwinRequest owinRequest = owinRequestMock.Object;

            Assert.IsTrue(owinRequest.IsSuppressedRequest());
        }

        [TestMethod]
        public void OwinRequestUtilities_IsSuppressedRequest_HasHeader_IsFalse_ShouldReturnFalse()
        {
            HeaderDictionary dict = new HeaderDictionary(
                new Dictionary<string, string[]>()
                {
                    {
                        "Suppress-Redirect",  new[] { "False" }
                    },
                });

            var owinRequestMock = new Mock<IOwinRequest>();
            owinRequestMock.Setup(x => x.Headers).Returns(dict);
            IOwinRequest owinRequest = owinRequestMock.Object;

            Assert.IsTrue(!owinRequest.IsSuppressedRequest());
        }

        [TestMethod]
        public void OwinRequestUtilities_IsSuppressedRequest_HasNoHeader_IsFalse_ShouldReturnFalse()
        {
            HeaderDictionary dict = null;
            var owinRequestMock = new Mock<IOwinRequest>();
            owinRequestMock.Setup(x => x.Headers).Returns(dict);
            IOwinRequest owinRequest = owinRequestMock.Object;

            Assert.IsTrue(!owinRequest.IsSuppressedRequest());
        }

        [TestMethod]
        public void OwinRequestUtilities_IsSuppressedRequest_HasNoHeader_ShouldReturnFalse()
        {
            HeaderDictionary dict = new HeaderDictionary(
                new Dictionary<string, string[]>()
                {
                    {
                        "Suppress-Redirect-NotThatOne",  new[] { "XMLHttpRequest" }
                    },
                });

            var owinRequestMock = new Mock<IOwinRequest>();
            owinRequestMock.Setup(x => x.Headers).Returns(dict);
            IOwinRequest owinRequest = owinRequestMock.Object;

            Assert.IsTrue(!owinRequest.IsSuppressedRequest());
        }

        [TestMethod]
        public void OwinRequestUtilities_IsAjaxRequest_HasXMLHttpRequest_ShouldReturnTrue()
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

            Assert.IsTrue(owinRequest.IsAjaxRequest());
        }
        
        [TestMethod]
        public void OwinRequestUtilities_IsAjaxRequest_HasNoXMLHttpRequest_ShouldReturnFalse()
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

            Assert.IsTrue(!owinRequest.IsAjaxRequest());
        }

        [TestMethod]
        public void OwinRequestUtilities_IsWebSocketRequest_HasSignalR_ShouldReturnTrue()
        {
            var owinRequestMock = new Mock<IOwinRequest>(MockBehavior.Strict);
            owinRequestMock.Setup(x => x.Path).Returns(new PathString("/alainprovist/signalr/"));
            IOwinRequest owinRequest = owinRequestMock.Object;

            Assert.IsTrue(owinRequest.IsWebSocketRequest());
        }

        [TestMethod]
        public void OwinRequestUtilities_IsWebSocketRequest_HasNoSignalR_ShouldReturnFalse()
        {
            var owinRequestMock = new Mock<IOwinRequest>(MockBehavior.Strict);
            owinRequestMock.Setup(x => x.Path).Returns(new PathString("/alainprovist/"));
            IOwinRequest owinRequest = owinRequestMock.Object;

            Assert.IsTrue(!owinRequest.IsWebSocketRequest());
        }

        [TestMethod]
        public void OwinRequestUtilities_IsFileRequest_HasFileExtension_ShouldReturnTrue()
        {
            var owinRequestMock = new Mock<IOwinRequest>(MockBehavior.Strict);
            owinRequestMock.Setup(x => x.Path).Returns(new PathString("/alainprovist/signalr.jpeg"));
            IOwinRequest owinRequest = owinRequestMock.Object;

            Assert.IsTrue(owinRequest.IsFileRequest());
        }

        [TestMethod]
        public void OwinRequestUtilities_IsFileRequest_HasNoFileExtension_ShouldReturnFalse()
        {
            var owinRequestMock = new Mock<IOwinRequest>(MockBehavior.Strict);
            owinRequestMock.Setup(x => x.Path).Returns(new PathString("/alainprovist/signalr"));
            IOwinRequest owinRequest = owinRequestMock.Object;

            Assert.IsTrue(!owinRequest.IsFileRequest());
        }
    }
}