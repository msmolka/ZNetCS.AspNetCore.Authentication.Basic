// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BasicAuthenticationMiddleware.cs" company="Marcin Smółka zNET Computer Solutions">
//   Copyright (c) Marcin Smółka zNET Computer Solutions. All rights reserved.
// </copyright>
// <summary>
//   The basic authentication middleware.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ZNetCS.AspNetCore.Authentication.Basic
{
    #region Usings

    using System.Text.Encodings.Web;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    #endregion

    /// <summary>
    /// The basic authentication middleware.
    /// </summary>
    public class BasicAuthenticationMiddleware : AuthenticationMiddleware<BasicAuthenticationOptions>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicAuthenticationMiddleware"/> class.
        /// </summary>
        /// <param name="next">
        /// The next request delegate.
        /// </param>
        /// <param name="loggerFactory">
        /// The logger factory.
        /// </param>
        /// <param name="encoder">
        /// The encoder.
        /// </param>
        /// <param name="options">
        /// The options.
        /// </param>
        public BasicAuthenticationMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, UrlEncoder encoder, IOptions<BasicAuthenticationOptions> options)
            : base(next, options, loggerFactory, encoder)
        {
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override AuthenticationHandler<BasicAuthenticationOptions> CreateHandler()
        {
            return new BasicAuthenticationHandler();
        }

        #endregion
    }
}