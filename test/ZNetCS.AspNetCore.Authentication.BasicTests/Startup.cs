// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Startup.cs" company="Marcin Smółka">
//   Copyright (c) Marcin Smółka. All rights reserved.
// </copyright>
// <summary>
//   The startup.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ZNetCS.AspNetCore.Authentication.BasicTests;

#region Usings

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ZNetCS.AspNetCore.Authentication.Basic;

#endregion

/// <summary>
/// The startup.
/// </summary>
public class Startup
{
    #region Public Methods

    /// <summary>
    /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    /// </summary>
    /// <param name="app">
    /// The app builder.
    /// </param>
    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(o => o.MapControllers());
    }

    /// <summary>
    /// This method gets called by the runtime. Use this method to add services to the container.
    /// </summary>
    /// <param name="services">
    /// The services.
    /// </param>
    public void ConfigureServices(IServiceCollection services)
    {
        // Add framework services.
        services
            .AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme)
            .AddBasicAuthentication(WebApplicationFactoryHelper.ConfigureOptions());

        services.AddMvc();
    }

    #endregion
}