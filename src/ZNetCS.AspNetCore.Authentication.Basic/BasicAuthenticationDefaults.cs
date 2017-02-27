// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BasicAuthenticationDefaults.cs" company="Marcin Smółka zNET Computer Solutions">
//   Copyright (c) Marcin Smółka zNET Computer Solutions. All rights reserved.
// </copyright>
// <summary>
//   The basic authentication defaults.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ZNetCS.AspNetCore.Authentication.Basic
{
    /// <summary>
    /// The basic authentication defaults.
    /// </summary>
    public static class BasicAuthenticationDefaults
    {
        #region Constants

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
}