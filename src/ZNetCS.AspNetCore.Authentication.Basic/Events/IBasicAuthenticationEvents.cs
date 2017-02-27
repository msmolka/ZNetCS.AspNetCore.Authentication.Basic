// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBasicAuthenticationEvents.cs" company="Marcin Smółka zNET Computer Solutions">
//   Copyright (c) Marcin Smółka zNET Computer Solutions. All rights reserved.
// </copyright>
// <summary>
//   Specifies callback methods which the <see cref="BasicAuthenticationMiddleware"></see> invokes to enable developer
//   control over the authentication process.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ZNetCS.AspNetCore.Authentication.Basic.Events
{
    #region Usings

    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication;

    #endregion

    /// <summary>
    /// Specifies callback methods which the <see cref="BasicAuthenticationMiddleware"></see> invokes to enable application
    /// control over the authentication process.
    /// </summary>
    public interface IBasicAuthenticationEvents
    {
        #region Public Methods

        /// <summary>
        /// Called each time a request principal has been validated by the middleware. By implementing this method the
        /// application have alter or reject the principal which has arrived with the request.
        /// </summary>
        /// <param name="context">
        /// Contains information about the login session as well as the user name and provide password.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the completed operation.
        /// </returns>
        Task<AuthenticateResult> ValidatePrincipal(ValidatePrincipalContext context);

        #endregion
    }
}