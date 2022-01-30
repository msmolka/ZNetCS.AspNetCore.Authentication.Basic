// -----------------------------------------------------------------------
// <copyright file="EmptyWebApplicationFactory.cs" company="Marcin Smółka">
// Copyright (c) Marcin Smółka. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ZNetCS.AspNetCore.Authentication.BasicTests;

#region Usings

using System.IO;
using System.Reflection;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

#endregion

/// <inheritdoc/>
internal class EmptyWebApplicationFactory : WebApplicationFactory<EmptyStartup>
{
    /// <inheritdoc/>
    protected override IHostBuilder CreateHostBuilder() => Host.CreateDefaultBuilder().ConfigureWebHostDefaults(_ => { });

    /// <inheritdoc />
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseContentRoot(GetPath() ?? string.Empty);
        builder.ConfigureServices(s => { s.AddMvc(); });
        builder.Configure(
            app =>
            {
                app.UseRouting();
                app.UseAuthentication();
                app.UseAuthorization();
                app.UseEndpoints(o => o.MapControllers());
            });
        builder.ConfigureLogging(
            (_, logging) =>
            {
                logging
                    .AddFilter("Default", LogLevel.Debug)
                    .AddDebug();
            });
    }

    /// <summary>
    /// Get root path for test web server.
    /// </summary>
    private static string? GetPath()
    {
        string path = Path.GetDirectoryName(typeof(EmptyStartup).GetTypeInfo().Assembly.Location)!;

        // ReSharper disable PossibleNullReferenceException
        DirectoryInfo? di = new DirectoryInfo(path).Parent?.Parent?.Parent;

        return di?.FullName;
    }
}