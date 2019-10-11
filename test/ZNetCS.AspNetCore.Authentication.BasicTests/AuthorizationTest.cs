// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthorizationTest.cs" company="Marcin Smółka zNET Computer Solutions">
//   Copyright (c) Marcin Smółka zNET Computer Solutions. All rights reserved.
// </copyright>
// <summary>
//   The authorization test.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ZNetCS.AspNetCore.Authentication.BasicTests
{
    #region Usings

    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Net.Http.Headers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using NameValueHeaderValue = Microsoft.Net.Http.Headers.NameValueHeaderValue;

    #endregion

    /// <summary>
    /// The authorization test.
    /// </summary>
    [TestClass]
    public class AuthorizationTest
    {
        #region Public Methods

        /// <summary>
        /// The authorized valid credentials.
        /// </summary>
        [TestMethod]
        public async Task AuthorizedCredentialsTest()
        {
            using var server = new TestServer(WebHostBuilderHelper.CreateStartupBuilder());
            using HttpClient client = server.CreateClient();

            // Arrange
            client.DefaultRequestHeaders.Add(HeaderNames.Authorization, AuthorizationHeaderHelper.GetBasic());

            // Act
            HttpResponseMessage response = await client.GetAsync("api/test");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "StatusCode != OK");
        }

        /// <summary>
        /// The authorized valid credentials.
        /// </summary>
        [TestMethod]
        public async Task AuthorizedCredentialsTestWithDi()
        {
            using var server = new TestServer(WebHostBuilderHelper.CreateBuilder());
            using HttpClient client = server.CreateClient();

            // Arrange
            client.DefaultRequestHeaders.Add(HeaderNames.Authorization, AuthorizationHeaderHelper.GetBasic());

            // Act
            HttpResponseMessage response = await client.GetAsync("api/test");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "StatusCode != OK");
        }

        /// <summary>
        /// The unauthorized basic realm.
        /// </summary>
        [TestMethod]
        public async Task UnauthorizedBasicRealmTestWithDi()
        {
            using var server = new TestServer(WebHostBuilderHelper.CreateBuilder());
            using HttpClient client = server.CreateClient();

            // Act
            HttpResponseMessage response = await client.GetAsync("api/test");

            // Assert
            AuthenticationHeaderValue wwwAuth = response.Headers.WwwAuthenticate.Single();
            NameValueHeaderValue nvh = NameValueHeaderValue.Parse(wwwAuth.Parameter);

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode, "StatusCode != Unauthorized");
            Assert.AreEqual("Basic", wwwAuth.Scheme, "Scheme != Basic");
            Assert.AreEqual("realm", nvh.Name, "!realm");
            Assert.AreEqual("\"Basic Realm\"", nvh.Value, "!basic realm");
        }

        /// <summary>
        /// The unauthorized basic realm.
        /// </summary>
        [TestMethod]
        public async Task UnauthorizedBasicRealmTestWithOptions()
        {
            using var server = new TestServer(WebHostBuilderHelper.CreateBuilder(o => { }));
            using HttpClient client = server.CreateClient();

            // Act
            HttpResponseMessage response = await client.GetAsync("api/test");

            // Assert
            AuthenticationHeaderValue wwwAuth = response.Headers.WwwAuthenticate.Single();
            NameValueHeaderValue nvh = NameValueHeaderValue.Parse(wwwAuth.Parameter);

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode, "StatusCode != Unauthorized");
            Assert.AreEqual("Basic", wwwAuth.Scheme, "Scheme != Basic");
            Assert.AreEqual("realm", nvh.Name, "!realm");
            Assert.AreEqual("\"Basic Realm\"", nvh.Value, "!basic realm");
        }

        /// <summary>
        /// The unauthorized.
        /// </summary>
        [TestMethod]
        public async Task UnauthorizedBasicTestWithDi()
        {
            using var server = new TestServer(WebHostBuilderHelper.CreateBuilder());
            using HttpClient client = server.CreateClient();

            // Act
            HttpResponseMessage response = await client.GetAsync("api/test");

            // Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode, "StatusCode != Unauthorized");
        }

        /// <summary>
        /// The unauthorized.
        /// </summary>
        [TestMethod]
        public async Task UnauthorizedBasicTestWithOptions()
        {
            using var server = new TestServer(WebHostBuilderHelper.CreateBuilder(o => { }));
            using HttpClient client = server.CreateClient();

            // Act
            HttpResponseMessage response = await client.GetAsync("api/test");

            // Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode, "StatusCode != Unauthorized");
        }

        /// <summary>
        /// The unauthorized invalid credentials realm.
        /// </summary>
        [TestMethod]
        public async Task UnauthorizedInvalidCredentialsTest()
        {
            using var server = new TestServer(WebHostBuilderHelper.CreateStartupBuilder());
            using HttpClient client = server.CreateClient();

            // Arrange
            client.DefaultRequestHeaders.Add(HeaderNames.Authorization, AuthorizationHeaderHelper.GetBasic("test", "test"));

            // Act
            HttpResponseMessage response = await client.GetAsync("api/test");

            // Assert
            AuthenticationHeaderValue wwwAuth = response.Headers.WwwAuthenticate.Single();
            NameValueHeaderValue nvh = NameValueHeaderValue.Parse(wwwAuth.Parameter);

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode, "StatusCode != Unauthorized");
            Assert.AreEqual("Basic", wwwAuth.Scheme, "Scheme != Basic");
            Assert.AreEqual("realm", nvh.Name, "!realm");
            Assert.AreEqual("\"My realm\"", nvh.Value, "!My realm");
        }

        /// <summary>
        /// The unauthorized basic realm.
        /// </summary>
        [TestMethod]
        public async Task UnauthorizedMyRealmTest()
        {
            using var server = new TestServer(WebHostBuilderHelper.CreateBuilder(o => { o.Realm = "My realm"; }));
            using HttpClient client = server.CreateClient();

            // Act
            HttpResponseMessage response = await client.GetAsync("api/test");

            // Assert
            AuthenticationHeaderValue wwwAuth = response.Headers.WwwAuthenticate.Single();
            NameValueHeaderValue nvh = NameValueHeaderValue.Parse(wwwAuth.Parameter);

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode, "StatusCode != Unauthorized");
            Assert.AreEqual("Basic", wwwAuth.Scheme, "Scheme != Basic");
            Assert.AreEqual("realm", nvh.Name, "!realm");
            Assert.AreEqual("\"My realm\"", nvh.Value, "!My realm");
        }

        /// <summary>
        /// The unauthorized vapid credentials but not setup.
        /// </summary>
        [TestMethod]
        public async Task UnauthorizedValidCredentialsTestWithOptions()
        {
            using var server = new TestServer(WebHostBuilderHelper.CreateBuilder(o => { }));
            using HttpClient client = server.CreateClient();

            // Arrange
            client.DefaultRequestHeaders.Add(HeaderNames.Authorization, AuthorizationHeaderHelper.GetBasic());

            // Act
            HttpResponseMessage response = await client.GetAsync("api/test");

            // Assert
            AuthenticationHeaderValue wwwAuth = response.Headers.WwwAuthenticate.Single();
            NameValueHeaderValue nvh = NameValueHeaderValue.Parse(wwwAuth.Parameter);

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode, "StatusCode != Unauthorized");
            Assert.AreEqual("Basic", wwwAuth.Scheme, "Scheme != Basic");
            Assert.AreEqual("realm", nvh.Name, "!realm");
            Assert.AreEqual("\"Basic Realm\"", nvh.Value, "!basic realm");
        }

        /// <summary>
        /// The unauthorized invalid credentials realm.
        /// </summary>
        [TestMethod]
        public async Task UnauthorizedWrongHeaderTest()
        {
            using var server = new TestServer(WebHostBuilderHelper.CreateStartupBuilder());
            using HttpClient client = server.CreateClient();

            // Arrange
            client.DefaultRequestHeaders.Add(HeaderNames.Authorization, "Basic");

            // Act
            HttpResponseMessage response = await client.GetAsync("api/test");

            // Assert
            AuthenticationHeaderValue wwwAuth = response.Headers.WwwAuthenticate.Single();
            NameValueHeaderValue nvh = NameValueHeaderValue.Parse(wwwAuth.Parameter);

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode, "StatusCode != Unauthorized");
            Assert.AreEqual("Basic", wwwAuth.Scheme, "Scheme != Basic");
            Assert.AreEqual("realm", nvh.Name, "!realm");
            Assert.AreEqual("\"My realm\"", nvh.Value, "!My realm");
        }

        /// <summary>
        /// The unauthorized basic realm via ajax.
        /// </summary>
        [TestMethod]
        public async Task UnauthorizedMyRealmTestAjaxRequestSuppressed()
        {
            using var server = new TestServer(WebHostBuilderHelper.CreateBuilder(o =>
            {
                o.Realm = "My realm";
                o.AjaxRequestOptions.SuppressWwwAuthenticateHeader = true;
            }));
            using HttpClient client = server.CreateClient();
            client.DefaultRequestHeaders.Add(Basic.BasicAuthenticationDefaults.AjaxRequestHeaderName, Basic.BasicAuthenticationDefaults.AjaxRequestHeaderValue);

            // Act
            HttpResponseMessage response = await client.GetAsync("api/test");

            // Assert
            AuthenticationHeaderValue wwwAuth = response.Headers.WwwAuthenticate.SingleOrDefault();

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode, "StatusCode != Unauthorized");
            Assert.IsNull(wwwAuth, "No header should be sent back on ajax request");
        }

        #endregion
    }
}