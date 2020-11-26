// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AjaxRequestOptions.cs" company="Marcin Smółka zNET Computer Solutions">
//   Copyright (c) Marcin Smółka zNET Computer Solutions. All rights reserved.
// </copyright>
// <summary>
//   The ajax request options.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ZNetCS.AspNetCore.Authentication.Basic
{
    /// <summary>
    /// The ajax request options.
    /// </summary>
    public class AjaxRequestOptions
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the ajax request header name.
        /// </summary>
        public string HeaderName { get; set; } = BasicAuthenticationDefaults.AjaxRequestHeaderName;

        /// <summary>
        /// Gets or sets the ajax request header value.
        /// </summary>
        public string HeaderValue { get; set; } = BasicAuthenticationDefaults.AjaxRequestHeaderValue;

        /// <summary>
        /// Gets or sets a value indicating whether suppress sending the WWWAuthenticate response header when a request has the
        /// header (X-Requested-With,XMLHttpRequest).
        /// </summary>
        public bool SuppressWwwAuthenticateHeader { get; set; }

        #endregion
    }
}