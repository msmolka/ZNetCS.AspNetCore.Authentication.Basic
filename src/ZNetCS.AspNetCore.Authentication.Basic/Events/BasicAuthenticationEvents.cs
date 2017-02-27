// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BasicAuthenticationEvents.cs" company="Marcin Smółka zNET Computer Solutions">
//   Copyright (c) Marcin Smółka zNET Computer Solutions. All rights reserved.
// </copyright>
// <summary>
//   The basic authentication events.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ZNetCS.AspNetCore.Authentication.Basic.Events
{
    #region Usings

    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication;

    #endregion

    /// <summary>
    /// The basic authentication events.
    /// </summary>
    public class BasicAuthenticationEvents : IBasicAuthenticationEvents
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets a delegate assigned to this property will be invoked when the related method is called.
        /// </summary>
        public Func<ValidatePrincipalContext, Task<AuthenticateResult>> OnValidatePrincipal { get; set; } =
            context => Task.FromResult(AuthenticateResult.Fail("Incorrect credentials."));

        #endregion

        #region Implemented Interfaces

        #region IBasicAuthenticationEvents

        /// <inheritdoc />
        public virtual Task<AuthenticateResult> ValidatePrincipal(ValidatePrincipalContext context) => this.OnValidatePrincipal(context);

        #endregion

        #endregion
    }
}