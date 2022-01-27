// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebApplicationFactoryHelper.cs" company="Marcin Smółka">
//   Copyright (c) Marcin Smółka. All rights reserved.
// </copyright>
// <summary>
//   Web host builder helper.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ZNetCS.AspNetCore.Authentication.BasicTests;

#region Usings

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

using ZNetCS.AspNetCore.Authentication.Basic;
using ZNetCS.AspNetCore.Authentication.Basic.Events;

#endregion

/// <summary>
/// Web Application Factory helper.
/// </summary>
public static class WebApplicationFactoryHelper
{
    #region Public Methods

    /// <summary>
    /// Creates code builder.
    /// </summary>
    /// <param name="configureOptions">
    /// The configure options action.
    /// </param>
    public static WebApplicationFactory<EmptyStartup> CreateFactory(Action<BasicAuthenticationOptions> configureOptions)
    {
        var factory = new EmptyWebApplicationFactory()
            .WithWebHostBuilder(
                builder =>
                {
                    builder.ConfigureServices(
                        s =>
                        {
                            s.AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme).AddBasicAuthentication(configureOptions);
                        });
                });

        return factory;
    }

    /// <summary>
    /// Creates code builder.
    /// </summary>
    public static WebApplicationFactory<EmptyStartup> CreateFactory()
    {
        var factory = new EmptyWebApplicationFactory()
            .WithWebHostBuilder(
                builder =>
                {
                    builder.ConfigureServices(
                        s =>
                        {
                            s.AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme)
                                .AddBasicAuthentication(options => options.EventsType = typeof(BasicAuthenticationEventsTest));
                            s.AddSingleton<BasicAuthenticationEventsTest>();
                        });
                });

        return factory;
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
                    }

                    return Task.CompletedTask;
                }
            };
        };
    }

    /// <summary>
    /// Creates builder.
    /// </summary>
    public static WebApplicationFactory<Startup> CreateStartupFactory() => new StartupWebApplicationFactory();

    #endregion
}