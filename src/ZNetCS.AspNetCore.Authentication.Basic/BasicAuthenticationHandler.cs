// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BasicAuthenticationHandler.cs" company="Marcin Smółka zNET Computer Solutions">
//   Copyright (c) Marcin Smółka zNET Computer Solutions. All rights reserved.
// </copyright>
// <summary>
//   The basic authentication handler.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ZNetCS.AspNetCore.Authentication.Basic
{
    #region Usings

    using System;
    using System.Linq;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.Net.Http.Headers;

    using ZNetCS.AspNetCore.Authentication.Basic.Events;

    #endregion

    /// <summary>
    /// The basic authentication handler.
    /// </summary>
    /// <remarks>
    /// See RFC 7617 https://tools.ietf.org/html/rfc7617#section-2.
    /// The Basic authentication scheme is based on the model that the client
    /// needs to authenticate itself with a user-id and a password for each
    /// protection space("realm").  The realm value is a free-form string
    /// that can only be compared for equality with other realms on that
    /// server.The server will service the request only if it can validate
    /// the user-id and password for the protection space applying to the
    /// requested resource.
    /// </remarks>
    public class BasicAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOptions>
    {
        #region Constants

        /// <summary>
        /// The scheme name is "Basic".
        /// </summary>
        private const string Basic = "Basic";

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicAuthenticationHandler"/> class.
        /// </summary>
        /// <param name="options">
        /// The options.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        /// <param name="encoder">
        /// The encoder.
        /// </param>
        /// <param name="clock">
        /// The clock.
        /// </param>
        public BasicAuthenticationHandler(IOptionsMonitor<BasicAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(
            options,
            logger,
            encoder,
            clock)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the events. The handler calls methods on the events which give the application control
        /// at certain points where processing is occurring. If it is not provided a default instance
        /// is supplied which does nothing when the methods are called.
        /// </summary>
        protected new BasicAuthenticationEvents Events
        {
            get => (BasicAuthenticationEvents)base.Events;
            set => base.Events = value;
        }

        #endregion

        #region Methods

        /// <inheritdoc/>
        protected override Task<object> CreateEventsAsync() => Task.FromResult<object>(new BasicAuthenticationEvents());

        /// <inheritdoc/>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // RFC 7230 section 3.2.2
            var authorizationHeaderValues = this.Request.Headers.GetCommaSeparatedValues(HeaderNames.Authorization);

            // there is no authorization header so there is nothing to do by this middleware
            if ((authorizationHeaderValues == null) || (authorizationHeaderValues.Length == 0))
            {
                this.Logger.LogDebug("'Authorization' header is not present in the request.");
                return AuthenticateResult.NoResult();
            }

            string basicAuthorizationHeader = authorizationHeaderValues.FirstOrDefault(s => s.StartsWith(Basic + ' ', StringComparison.OrdinalIgnoreCase));

            // Authorization header is not 'Basic' so there is nothing to do by this middleware
            if (string.IsNullOrEmpty(basicAuthorizationHeader))
            {
                this.Logger.LogDebug("'Authorization' header is not in 'Basic' scheme in the request.");
                return AuthenticateResult.NoResult();
            }

            string credentials = basicAuthorizationHeader.Replace($"{Basic} ", string.Empty).Trim();

            if (string.IsNullOrEmpty(credentials))
            {
                return AuthenticateResult.Fail("The credentials are not present for 'Basic' scheme.");
            }

            string decodedCredentials;

            try
            {
                decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(credentials));
            }
            catch (Exception)
            {
                return AuthenticateResult.Fail("The credentials can not be decoded in 'Basic' scheme.");
            }

            int delimiterIndex = decodedCredentials.IndexOf(":", StringComparison.OrdinalIgnoreCase);

            if (delimiterIndex < 0)
            {
                return AuthenticateResult.Fail("The credentials delimiter is not present in 'Basic' scheme.");
            }

            string userName = decodedCredentials.Substring(0, delimiterIndex);
            string password = decodedCredentials.Substring(delimiterIndex + 1);

            var context = new ValidatePrincipalContext(this.Context, this.Scheme, this.Options, userName, password);
            await this.Events.ValidatePrincipalAsync(context);

            if (context.Principal == null)
            {
                return AuthenticateResult.Fail(context.AuthenticationFailMessage);
            }

            return AuthenticateResult.Success(new AuthenticationTicket(context.Principal, context.Properties, this.Scheme.Name));
        }

        /// <inheritdoc/>
        protected override Task HandleChallengeAsync(AuthenticationProperties context)
        {
            var realmHeader = new NameValueHeaderValue("realm", $"\"{this.Options.Realm}\"");
            this.Response.StatusCode = StatusCodes.Status401Unauthorized;

            if (this.Options.SupressResponseHeaderWWWAuthenticateForAjaxRequests)
            {
                if (this.Request.Headers.TryGetValue(this.Options.AjaxRequestHeaderName, out var value))
                {
                    if (value == this.Options.AjaxRequestHeaderValue)
                    {
                        return Task.CompletedTask;
                    }
                }
            }

            this.Response.Headers.Append(HeaderNames.WWWAuthenticate, $"{Basic} {realmHeader}");
            return Task.CompletedTask;
        }

        #endregion
    }
}