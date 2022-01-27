// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BasicAuthenticationExtensions.cs" company="Marcin Smółka">
//   Copyright (c) Marcin Smółka. All rights reserved.
// </copyright>
// <summary>
//   The basic authentication extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

#region Usings

using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Authentication;

using ZNetCS.AspNetCore.Authentication.Basic;

#endregion

/// <summary>
/// The basic authentication extensions.
/// </summary>
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Public API")]
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
        Action<BasicAuthenticationOptions>? configureOptions)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        return builder.AddScheme<BasicAuthenticationOptions, BasicAuthenticationHandler>(authenticationScheme, configureOptions);
    }

    #endregion
}