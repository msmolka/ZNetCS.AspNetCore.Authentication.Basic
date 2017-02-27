// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BasicAuthenticationOptions.cs" company="Marcin Smółka zNET Computer Solutions">
//   Copyright (c) Marcin Smółka zNET Computer Solutions. All rights reserved.
// </copyright>
// <summary>
//   Configuration options for <see cref="BasicAuthenticationMiddleware" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ZNetCS.AspNetCore.Authentication.Basic
{
    #region Usings

    using System.Diagnostics.CodeAnalysis;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Options;

    using ZNetCS.AspNetCore.Authentication.Basic.Events;

    #endregion

    /// <summary>
    /// Configuration options for <see cref="BasicAuthenticationMiddleware"/>.
    /// </summary>
    public class BasicAuthenticationOptions : AuthenticationOptions, IOptions<BasicAuthenticationOptions>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicAuthenticationOptions"/> class.
        /// </summary>
        public BasicAuthenticationOptions()
        {
            this.AuthenticationScheme = BasicAuthenticationDefaults.AuthenticationScheme;
            this.Realm = BasicAuthenticationDefaults.Realm;
            this.AutomaticAuthenticate = true;
            this.AutomaticChallenge = true;
            this.Events = new BasicAuthenticationEvents();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the events that may be assigned to an instance of an object created by the application at startup time.
        /// The middleware calls methods on the provider which give the application control at final authentication level.
        /// </summary>
        public IBasicAuthenticationEvents Events { get; set; }

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
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "OK")]
        public string Realm { get; set; }

        #endregion

        #region Explicit Interface Properties

        #region IOptions<BasicAuthenticationOptions>

        /// <summary>
        /// Gets the basic authentication options.
        /// </summary>
        BasicAuthenticationOptions IOptions<BasicAuthenticationOptions>.Value => this;

        #endregion

        #endregion
    }
}