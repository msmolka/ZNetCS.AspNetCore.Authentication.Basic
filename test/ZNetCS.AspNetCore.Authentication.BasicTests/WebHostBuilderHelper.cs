// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebHostBuilderHelper.cs" company="Marcin Smółka zNET Computer Solutions">
//   Copyright (c) Marcin Smółka zNET Computer Solutions. All rights reserved.
// </copyright>
// <summary>
//   Web host builder helper.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ZNetCS.AspNetCore.Authentication.BasicTests
{
    #region Usings

    using System.Collections.Generic;
    using System.IO;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http.Authentication;
    using Microsoft.Extensions.DependencyInjection;

    using ZNetCS.AspNetCore.Authentication.Basic;
    using ZNetCS.AspNetCore.Authentication.Basic.DependecyInjection;
    using ZNetCS.AspNetCore.Authentication.Basic.Events;

    #endregion

    /// <summary>
    /// Web host builder helper.
    /// </summary>
    public class WebHostBuilderHelper
    {
        #region Public Methods

        /// <summary>
        /// Creates code builder.
        /// </summary>
        /// <param name="options">
        /// The options.
        /// </param>
        public static IWebHostBuilder CreateBuilder(BasicAuthenticationOptions options = null)
        {
            if (options == null)
            {
                options = CreateOptions();
            }

            IWebHostBuilder builder = new WebHostBuilder()
                .ConfigureServices(
                    s => { s.AddMvc(); })
                .Configure(
                    app =>
                    {
                        app.UseBasicAuthentication(options);
                        app.UseMvc();
                    });

            return builder;
        }

        /// <summary>
        /// Creates options.
        /// </summary>
        public static BasicAuthenticationOptions CreateOptions()
        {
            return new BasicAuthenticationOptions
            {
                Realm = "My realm",
                Events = new BasicAuthenticationEvents
                {
                    OnValidatePrincipal = context =>
                    {
                        if ((context.UserName == "userName") && (context.Password == "password"))
                        {
                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, context.UserName, context.Options.ClaimsIssuer)
                            };

                            var ticket = new AuthenticationTicket(
                                new ClaimsPrincipal(
                                    new ClaimsIdentity(claims, context.Options.AuthenticationScheme)),
                                new AuthenticationProperties(),
                                context.Options.AuthenticationScheme);

                            return Task.FromResult(AuthenticateResult.Success(ticket));
                        }

                        return Task.FromResult(AuthenticateResult.Fail("Authentication failed."));
                    }
                }
            };
        }

        /// <summary>
        /// Creates builder.
        /// </summary>
        public static IWebHostBuilder CreateStartupBuilder()
        {
            return new WebHostBuilder()
                .UseStartup<Startup>()
                .UseContentRoot(Directory.GetCurrentDirectory());
        }

        #endregion
    }
}