# ZNetCS.AspNetCore.Authentication.Basic

[![NuGet](https://img.shields.io/nuget/v/ZNetCS.AspNetCore.Authentication.Basic.svg)](https://www.nuget.org/packages/ZNetCS.AspNetCore.Authentication.Basic)

A simple basic authentication middleware.

## Installing 

Install using the [ZNetCS.AspNetCore.Authentication.Basic NuGet package](https://www.nuget.org/packages/ZNetCS.AspNetCore.Authentication.Basic)

```
PM> Install-Package ZNetCS.AspNetCore.Authentication.Basic
```

# Important change in 3.0.0 
The `OnValidatePrincipal` will not return `AuthenticationResult` any more. To simplify process can simply return `Task.CompletedTask`.
Also to make success authentication `Principal` have to be assigned to `ValidatePrincipalContext` context.

## Usage 

When you install the package, it should be added to your `.csproj`. Alternatively, you can add it directly by adding:


```xml
<ItemGroup>
    <PackageReference Include="ZNetCS.AspNetCore.Authentication.Basic" Version="3.0.1" />
</ItemGroup>
```

In order to use the basic authentication middleware, you must configure the services in the `Configure` and `ConfigureServices` call of `Startup`. Because basic 
authentication is manual process handled on each request, there is need to validate credentials manually (see below).

```csharp
using ZNetCS.AspNetCore.Authentication.Basic;
using ZNetCS.AspNetCore.Authentication.Basic.Events;
```

```
...
```

```csharp
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

                            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, BasicAuthenticationDefaults.AuthenticationScheme));
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

            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, BasicAuthenticationDefaults.AuthenticationScheme));
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
                options.EventsType = typeof(AuthenticationEvents)
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