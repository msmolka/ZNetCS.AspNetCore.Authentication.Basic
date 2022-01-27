// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BasicAuthenticationDefaults.cs" company="Marcin Smółka">
//   Copyright (c) Marcin Smółka. All rights reserved.
// </copyright>
// <summary>
//   The basic authentication defaults.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ZNetCS.AspNetCore.Authentication.Basic;

/// <summary>
/// The basic authentication defaults.
/// </summary>
public static class BasicAuthenticationDefaults
{
    #region Constants

    /// <summary>
    /// The ajax request header name.
    /// </summary>
    public const string AjaxRequestHeaderName = "X-Requested-With";

    /// <summary>
    /// The ajax request header value.
    /// </summary>
    public const string AjaxRequestHeaderValue = "XMLHttpRequest";

    /// <summary>
    /// The default value used for BasicAuthenticationOptions.AuthenticationScheme.
    /// </summary>
    public const string AuthenticationScheme = "Basic";

    /// <summary>
    /// The default value used for BasicAuthenticationOptions.Realm.
    /// </summary>
    public const string Realm = "Basic Realm";

    #endregion
}