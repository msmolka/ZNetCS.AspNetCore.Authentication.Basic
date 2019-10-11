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

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using ZNetCS.AspNetCore.Authentication.Basic;
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
        /// <param name="configureOptions">
        /// The configure options action.
        /// </param>
        public static IWebHostBuilder CreateBuilder(Action<BasicAuthenticationOptions> configureOptions)
        {
            IWebHostBuilder builder = new WebHostBuilder()
                .ConfigureServices(
                    s =>
                    {
                        s.AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme).AddBasicAuthentication(configureOptions);
                        s.AddMvc();
                    })
                .Configure(
                    app =>
                    {
#if NETCOREAPP3_0
                        app.UseRouting();
                        app.UseAuthentication();
                        app.UseAuthorization();
                        app.UseEndpoints(o => o.MapControllers());
#else

                        app.UseAuthentication();
                        app.UseMvc();
#endif
                    })
                .ConfigureLogging(
                    (context, logging) =>
                    {
                        logging
                            .AddFilter("Default", LogLevel.Debug)
                            .AddDebug();
                    });

            return builder;
        }

        /// <summary>
        /// Creates code builder.
        /// </summary>
        public static IWebHostBuilder CreateBuilder()
        {
            IWebHostBuilder builder = new WebHostBuilder()
                .ConfigureServices(
                    s =>
                    {
                        s.AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme).AddBasicAuthentication(options => options.EventsType = typeof(BasicAuthenticationEventsTest));
                        s.AddSingleton<BasicAuthenticationEventsTest>();
                        s.AddMvc();
                    })
                .Configure(
                    app =>
                    {
#if NETCOREAPP3_0
                        app.UseRouting();
                        app.UseAuthentication();
                        app.UseAuthorization();
                        app.UseEndpoints(o => o.MapControllers());
#else

                        app.UseAuthentication();
                        app.UseMvc();
#endif
                    })
                .ConfigureLogging(
                    (context, logging) =>
                    {
                        logging
                            .AddFilter("Default", LogLevel.Debug)
                            .AddDebug();
                    });

            return builder;
        }

        /// <summary>
        /// Creates options.
        /// </summary>
        public static Action<BasicAuthenticationOptions> ConfigureOptions()
        {
            return options =>
            {
                options.Realm = "My realm";
                options.Events = new BasicAuthenticationEvents
                {
                    OnValidatePrincipal = context =>
                    {
                        if ((context.UserName == "userName") && (context.Password == "password"))
                        {
                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, context.UserName, context.Options.ClaimsIssuer)
                            };

                            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, BasicAuthenticationDefaults.AuthenticationScheme));
                            context.Principal = principal;
                            context.AuthenticationFailMessage = null;
                        }

                        return Task.CompletedTask;
                    }
                };
            };
        }

        /// <summary>
        /// Creates builder.
        /// </summary>
        public static IWebHostBuilder CreateStartupBuilder()
        {
            return new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .ConfigureLogging(
                    (context, logging) =>
                    {
                        logging
                            .AddFilter("Default", LogLevel.Debug)
                            .AddDebug();
                    });
        }

        #endregion
    }
}
