# ZNetCS.AspNetCore.Authentication.Basic

[![NuGet](https://img.shields.io/nuget/v/ZNetCS.AspNetCore.Authentication.Basic.svg)](https://www.nuget.org/packages/ZNetCS.AspNetCore.Authentication.Basic)
[![Build](https://github.com/msmolka/ZNetCS.AspNetCore.Authentication.Basic/workflows/build/badge.svg)](https://github.com/msmolka/ZNetCS.AspNetCore.Authentication.Basic/actions)

A simple basic authentication middleware. Allows setup authentication using configuration or dependency injection 
and suppress `WWW-Authenticate` header globally or for AJAX request.

## Installing 

Install using the [ZNetCS.AspNetCore.Authentication.Basic NuGet package](https://www.nuget.org/packages/ZNetCS.AspNetCore.Authentication.Basic)

```
PM> Install-Package ZNetCS.AspNetCore.Authentication.Basic
```
## Version history

### 6.0.0

Cleanup events initialization and nullable checkup. Events are now only initialized in handler not in options. 
Unless configured during initialization (no change in code is required, it is just code cleanup). Logger improvements.

### 5.0.0

Added direct references to latest framework and removed no longer supported frameworks.
Added possibility to suppress WWWAuthenticate header globally not only on Ajax request.

### 4.0.0

From now assembly is signed.

### 3.0.0

The `OnValidatePrincipal` will not return `AuthenticationResult` any more. To simplify process can simply return `Task.CompletedTask`.
Also to make success authentication `Principal` have to be assigned to `ValidatePrincipalContext` context.

## Usage 

When you install the package, it should be added to your `.csproj`. Alternatively, you can add it directly by adding:

```xml
<ItemGroup>
    <PackageReference Include="ZNetCS.AspNetCore.Authentication.Basic" Version="6.0.0" />
</ItemGroup>
```

```c#
using ZNetCS.AspNetCore.Authentication.Basic;
using ZNetCS.AspNetCore.Authentication.Basic.Events;
```

```
...
```

### .NET 6

In order to use the basic authentication middleware, you must configure the services in the `Program.cs` file.

```c#
// Add services to the container.
builder.Services
    .AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme)
    .AddBasicAuthentication(
        options =>
        {
            options.Realm = "My Application";
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

                        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, context.Scheme.Name));
                        context.Principal = principal;
                    }
                    else 
                    {
                        // optional with following default.
                        // context.AuthenticationFailMessage = "Authentication failed."; 
                    }

                    return Task.CompletedTask;
                }
            };
        });
```
or using dependency injection
```c#
public class AuthenticationEvents : BasicAuthenticationEvents
{
    #region Public Methods

    /// <inheritdoc/>
    public override Task ValidatePrincipalAsync(ValidatePrincipalContext context)
    {
        if ((context.UserName == "userName") && (context.Password == "password"))
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, context.UserName, context.Options.ClaimsIssuer)
            };

            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, context.Scheme.Name));
            context.Principal = principal;
        }

        return Task.CompletedTask;
    }

    #endregion
}

```

and then registration

```c#

builder.Services.AddScoped<AuthenticationEvents>();

builder.Services
    .AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme)
    .AddBasicAuthentication(
        options =>
        {
            options.Realm = "My Application";
            options.EventsType = typeof(AuthenticationEvents);
        });

```
then
```c#

// configure default authentication initialization
app.UseAuthentication();

// other middleware e.g. MVC etc
```


### .NET 5 and Below

In order to use the basic authentication middleware, you must configure the services in the `Configure` and `ConfigureServices` call of `Startup`. Because basic 
authentication is manual process handled on each request, there is need to validate credentials manually (see below).

```c#
public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
{   

    // default authentication initialization
    app.UseAuthentication();

    // other middleware e.g. MVC etc
}

public void ConfigureServices(IServiceCollection services)
{
    services
        .AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme)
        .AddBasicAuthentication(
            options =>
            {
                options.Realm = "My Application";
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

                            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, context.Scheme.Name));
                            context.Principal = principal;
                        }
                        else 
                        {
                            // optional with following default.
                            // context.AuthenticationFailMessage = "Authentication failed."; 
                        }

                        return Task.CompletedTask;
                    }
                };
            });
}
```
or using dependency injection:

```c#
public class AuthenticationEvents : BasicAuthenticationEvents
{
    #region Public Methods

    /// <inheritdoc/>
    public override Task ValidatePrincipalAsync(ValidatePrincipalContext context)
    {
        if ((context.UserName == "userName") && (context.Password == "password"))
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, context.UserName, context.Options.ClaimsIssuer)
            };

            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, context.Scheme.Name));
            context.Principal = principal;
        }

        return Task.CompletedTask;
    }

    #endregion
}

```

and then registration

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddScoped<AuthenticationEvents>();

    services
        .AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme)
        .AddBasicAuthentication(
            options =>
            {
                options.Realm = "My Application";
                options.EventsType = typeof(AuthenticationEvents);
            });
}
```

As from version 3.0.1 You can suppress the response WWW-Authenticate header (avoiding the browser to show a popup) for ajax requests by using a switch.

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddScoped<AuthenticationEvents>();

    services
        .AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme)
        .AddBasicAuthentication(
            options =>
            {
                options.Realm = "My Application";
                options.AjaxRequestOptions.SuppressWwwAuthenticateHeader = true;
            });
}
```
