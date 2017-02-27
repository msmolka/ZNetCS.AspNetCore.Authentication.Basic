// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationBuilderExtensions.cs" company="Marcin Smółka zNET Computer Solutions">
//   Copyright (c) Marcin Smółka zNET Computer Solutions. All rights reserved.
// </copyright>
// <summary>
//   Extension methods to add cookie authentication capabilities to an HTTP application pipeline.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ZNetCS.AspNetCore.Authentication.Basic.DependecyInjection
{
    #region Usings

    using System;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Options;

    #endregion

    /// <summary>
    /// Extension methods to add cookie authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        #region Public Methods

        /// <summary>
        /// Adds the <see cref="BasicAuthenticationMiddleware"/> to manually configure basic authentication.
        /// </summary>
        /// <param name="app">
        /// The <see cref="IApplicationBuilder"/> to use basic authentication on.
        /// </param>
        /// <param name="options">
        /// The <see cref="BasicAuthenticationOptions"/> to configure the middleware with.
        /// </param>
        /// <returns>
        /// The <see cref="IApplicationBuilder"/> so that additional calls can be chained.
        /// </returns>
        public static IApplicationBuilder UseBasicAuthentication(
            this IApplicationBuilder app,
            BasicAuthenticationOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return app.UseMiddleware<BasicAuthenticationMiddleware>(Options.Create(options));
        }

        #endregion
    }
}