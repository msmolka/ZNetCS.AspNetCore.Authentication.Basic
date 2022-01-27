// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidatePrincipalContext.cs" company="Marcin Smółka">
//   Copyright (c) Marcin Smółka. All rights reserved.
// </copyright>
// <summary>
//   The validate principal context.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ZNetCS.AspNetCore.Authentication.Basic.Events;

    #region Usings

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

#endregion

/// <summary>
/// The validate principal context.
/// </summary>
public class ValidatePrincipalContext : PrincipalContext<BasicAuthenticationOptions>
{
    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidatePrincipalContext"/> class.
    /// </summary>
    /// <param name="context">
    /// The context.
    /// </param>
    /// <param name="scheme">
    /// The scheme.
    /// </param>
    /// <param name="options">
    /// The options.
    /// </param>
    /// <param name="userName">
    /// The user name.
    /// </param>
    /// <param name="password">
    /// The password.
    /// </param>
    public ValidatePrincipalContext(HttpContext context, AuthenticationScheme scheme, BasicAuthenticationOptions options, string userName, string password)
        : base(context, scheme, options, null)
    {
        this.UserName = userName;
        this.Password = password;
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets or sets the authentication fail message.
    /// </summary>
    public string AuthenticationFailMessage { get; set; } = "Authentication failed.";

    /// <summary>
    /// Gets the password.
    /// </summary>
    public string Password { get; }

    /// <summary>
    /// Gets the user name.
    /// </summary>
    public string UserName { get; }

    #endregion
}