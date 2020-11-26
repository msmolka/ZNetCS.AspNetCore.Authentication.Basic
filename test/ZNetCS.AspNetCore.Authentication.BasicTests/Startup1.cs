// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Startup1.cs" company="Marcin Smółka zNET Computer Solutions">
//   Copyright (c) Marcin Smółka zNET Computer Solutions. All rights reserved.
// </copyright>
// <summary>
//   The startup.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#if !NETCOREAPP3_1 && !NET5_0
namespace ZNetCS.AspNetCore.Authentication.BasicTests
{
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
            app.UseAuthentication();
            app.UseMvc();
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
                .AddBasicAuthentication(WebHostBuilderHelper.ConfigureOptions());

            services.AddMvc();
        }

        #endregion
    }
}
#endif