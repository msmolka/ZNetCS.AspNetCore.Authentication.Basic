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

    #endregion

    /// <summary>
    /// The basic authentication events.
    /// </summary>
    public class BasicAuthenticationEvents
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets a delegate assigned to this property will be invoked when the related method is called.
        /// </summary>
        public Func<ValidatePrincipalContext, Task> OnValidatePrincipal { get; set; } = context => Task.CompletedTask;

        #endregion

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
        public virtual Task ValidatePrincipalAsync(ValidatePrincipalContext context) => this.OnValidatePrincipal(context);

        #endregion
    }
}