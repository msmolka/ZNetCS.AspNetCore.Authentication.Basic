// -----------------------------------------------------------------------
// <copyright file="EmptyStartup.cs" company="Marcin Smółka">
// Copyright (c) Marcin Smółka. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ZNetCS.AspNetCore.Authentication.BasicTests;

    #region Usings

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

#endregion

/// <summary>
/// The startup.
/// </summary>
public class EmptyStartup
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
    }

    /// <summary>
    /// This method gets called by the runtime. Use this method to add services to the container.
    /// </summary>
    /// <param name="services">
    /// The services.
    /// </param>
    public void ConfigureServices(IServiceCollection services)
    {
    }

    #endregion
}