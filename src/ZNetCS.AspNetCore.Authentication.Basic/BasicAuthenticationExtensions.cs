// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BasicAuthenticationExtensions.cs" company="Marcin Smółka zNET Computer Solutions">
//   Copyright (c) Marcin Smółka zNET Computer Solutions. All rights reserved.
// </copyright>
// <summary>
//   The basic authentication extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ZNetCS.AspNetCore.Authentication.Basic
{
    #region Usings

    using System;

    using Microsoft.AspNetCore.Authentication;

    #endregion

    /// <summary>
    /// The basic authentication extensions.
    /// </summary>
    public static class BasicAuthenticationExtensions
    {
        #region Public Methods

        /// <summary>
        /// Adds basic authentication.
        /// </summary>
        /// <param name="builder">
        /// The authentication builder.
        /// </param>
        public static AuthenticationBuilder AddBasicAuthentication(this AuthenticationBuilder builder)
            => builder.AddBasicAuthentication(BasicAuthenticationDefaults.AuthenticationScheme);

        /// <summary>
        /// Adds basic authentication.
        /// </summary>
        /// <param name="builder">
        /// The authentication builder.
        /// </param>
        /// <param name="authenticationScheme">
        /// The authentication scheme.
        /// </param>
        public static AuthenticationBuilder AddBasicAuthentication(this AuthenticationBuilder builder, string authenticationScheme)
            => builder.AddBasicAuthentication(authenticationScheme, null);

        /// <summary>
        /// Adds basic authentication.
        /// </summary>
        /// <param name="builder">
        /// The authentication builder.
        /// </param>
        /// <param name="configureOptions">
        /// The configure options.
        /// </param>
        public static AuthenticationBuilder AddBasicAuthentication(this AuthenticationBuilder builder, Action<BasicAuthenticationOptions> configureOptions)
            => builder.AddBasicAuthentication(BasicAuthenticationDefaults.AuthenticationScheme, configureOptions);

        /// <summary>
        /// Adds basic authentication.
        /// </summary>
        /// <param name="builder">
        /// The builder.
        /// </param>
        /// <param name="authenticationScheme">
        /// The authentication scheme.
        /// </param>
        /// <param name="configureOptions">
        /// The configure options.
        /// </param>
        public static AuthenticationBuilder AddBasicAuthentication(
            this AuthenticationBuilder builder,
            string authenticationScheme,
            Action<BasicAuthenticationOptions> configureOptions)
            => builder.AddScheme<BasicAuthenticationOptions, BasicAuthenticationHandler>(authenticationScheme, configureOptions);

        #endregion
    }
}