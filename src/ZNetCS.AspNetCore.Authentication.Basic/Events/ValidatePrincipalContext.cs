// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidatePrincipalContext.cs" company="Marcin Smółka zNET Computer Solutions">
//   Copyright (c) Marcin Smółka zNET Computer Solutions. All rights reserved.
// </copyright>
// <summary>
//   The validate principal context.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ZNetCS.AspNetCore.Authentication.Basic.Events
{
    #region Usings

    using System;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Http;

    #endregion

    /// <summary>
    /// The validate principal context.
    /// </summary>
    public class ValidatePrincipalContext : BaseContext
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatePrincipalContext"/> class.
        /// </summary>
        /// <param name="context">
        /// The context.
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
        public ValidatePrincipalContext(HttpContext context, BasicAuthenticationOptions options, string userName, string password) : base(context)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            this.Options = options;
            this.UserName = userName;
            this.Password = password;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the options.
        /// </summary>
        public BasicAuthenticationOptions Options { get; }

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
}