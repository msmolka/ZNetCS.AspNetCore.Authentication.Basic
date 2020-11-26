// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BasicAuthenticationOptions.cs" company="Marcin Smółka zNET Computer Solutions">
//   Copyright (c) Marcin Smółka zNET Computer Solutions. All rights reserved.
// </copyright>
// <summary>
//   Configuration options for basic authentication.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ZNetCS.AspNetCore.Authentication.Basic
{
    #region Usings

    using Microsoft.AspNetCore.Authentication;

    using ZNetCS.AspNetCore.Authentication.Basic.Events;

    #endregion

    /// <summary>
    /// Configuration options for basic authentication.
    /// </summary>
    public class BasicAuthenticationOptions : AuthenticationSchemeOptions
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicAuthenticationOptions"/> class.
        /// </summary>
        public BasicAuthenticationOptions()
        {
            this.Realm = BasicAuthenticationDefaults.Realm;
            this.Events = new BasicAuthenticationEvents();
            this.AjaxRequestOptions = new AjaxRequestOptions();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the ajax request options. Special authentication header handling for Ajax requests.
        /// </summary>
        public AjaxRequestOptions AjaxRequestOptions { get; set; }

        /// <summary>
        /// Gets or sets basic authentication events. The Provider may be assigned to an instance of an object created
        /// by the application at startup time. The handler calls methods on the provider which give the application
        /// control at certain points where processing is occurring. If it is not provided a default instance is
        /// supplied which does nothing when the methods are called.
        /// </summary>
        public new BasicAuthenticationEvents Events
        {
            get => (BasicAuthenticationEvents)base.Events;
            set => base.Events = value;
        }

        /// <summary>
        /// Gets or sets the realm.
        /// </summary>
        /// <remarks>
        /// See [RFC7235], Section 2.2 https://tools.ietf.org/html/rfc7235#section-2.2.
        /// The "realm" authentication parameter is reserved for use by
        /// authentication schemes that wish to indicate a scope of protection.
        /// A protection space is defined by the canonical root URI (the scheme
        /// and authority components of the effective request URI; see Section
        /// 5.5 of [RFC7230]) of the server being accessed, in combination with
        /// the realm value if present.These realms allow the protected
        /// resources on a server to be partitioned into a set of protection
        /// spaces, each with its own authentication scheme and/or authorization
        /// database.The realm value is a string, generally assigned by the
        /// origin server, that can have additional semantics specific to the
        /// authentication scheme.Note that a response can have multiple
        /// challenges with the same auth-scheme but with different realms.
        /// </remarks>
        public string Realm { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether suppress sending the WWWAuthenticate response header.
        /// In some rest scenarios it is not needed, so can be suppressed.
        /// </summary>
        public bool SuppressWwwAuthenticateHeader { get; set; }

        #endregion
    }
}