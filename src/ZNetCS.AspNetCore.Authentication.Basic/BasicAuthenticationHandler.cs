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
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Features.Authentication;
    using Microsoft.Extensions.Logging;
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
        /// <summary>
        /// The scheme name is "Basic".
        /// </summary>
        private const string Scheme = "Basic";

        #region Methods

        /// <inheritdoc />
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // RFC 7230 section 3.2.2
            var authorizationHeaderValues = this.Request.Headers.GetCommaSeparatedValues(HeaderNames.Authorization);

            // there is no authorization header so there is nothing to do by this middleware
            if ((authorizationHeaderValues == null) || (authorizationHeaderValues.Length == 0))
            {
                this.Logger.LogDebug("'Authorization' header is not present in the request.");
                return AuthenticateResult.Skip();
            }

            var basicAuthorizationHeader = authorizationHeaderValues.FirstOrDefault(s => s.StartsWith(Scheme + ' ', StringComparison.OrdinalIgnoreCase));

            // Authorization header is not 'Basic' so there is nothing to do by this middleware
            if (string.IsNullOrEmpty(basicAuthorizationHeader))
            {
                this.Logger.LogDebug("'Authorization' header is not in 'Basic' scheme in the request.");
                return AuthenticateResult.Skip();
            }

            var credentials = basicAuthorizationHeader.Replace($"{Scheme} ", string.Empty).Trim();

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

            var userName = decodedCredentials.Substring(0, delimiterIndex);
            var password = decodedCredentials.Substring(delimiterIndex + 1);

            var context = new ValidatePrincipalContext(this.Context, this.Options, userName, password);
            return await this.Options.Events.ValidatePrincipal(context);
        }

        /// <inheritdoc />
        protected override Task HandleSignInAsync(SignInContext context)
        {
            // Basic authentication have to be resolved on every request.
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        protected override Task HandleSignOutAsync(SignOutContext context)
        {
            // Basic authentication have to be resolved on every request.
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        protected override Task<bool> HandleUnauthorizedAsync(ChallengeContext context)
        {
            var realmHeader = new NameValueHeaderValue("realm", $"\"{this.Options.Realm}\"");
            this.Response.StatusCode = StatusCodes.Status401Unauthorized;
            this.Response.Headers.Append(HeaderNames.WWWAuthenticate, $"{Scheme} {realmHeader}");

            return Task.FromResult(true);
        }

        #endregion
    }
}