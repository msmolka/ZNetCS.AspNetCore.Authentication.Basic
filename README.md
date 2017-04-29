# ZNetCS.AspNetCore.Authentication.Basic

[![NuGet](https://img.shields.io/nuget/v/ZNetCS.AspNetCore.Authentication.Basic.svg)](https://www.nuget.org/packages/ZNetCS.AspNetCore.Authentication.Basic)

A simple basic authentication middleware.

## Installing 

Install using the [ZNetCS.AspNetCore.Authentication.Basic NuGet package](https://www.nuget.org/packages/ZNetCS.AspNetCore.Authentication.Basic)

```
PM> Install-Package ZNetCS.AspNetCore.Authentication.Basic
```

## Usage 

When you install the package, it should be added to your `.csproj`. Alternatively, you can add it directly by adding:


```xml
<ItemGroup>
    <PackageReference Include="ZNetCS.AspNetCore.Authentication.Basic" Version="1.0.0" />    
</ItemGroup>
```

In order to use the basic authentication middleware, you must configure the services in the `Configure` call of `Startup`. Because basic 
authentication is manual process handled on each request, there is need to validate credentials manually (see below).

```csharp
using ZNetCS.AspNetCore.Authentication.Basic.DependencyInjection;
```

```
...
```

```csharp

public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
{   
    var options = new BasicAuthenticationOptions
    {
        Realm = "My Application",
        Events = new BasicAuthenticationEvents
        {            
            OnValidatePrincipal = context =>
            {
                // here validation comes
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
                    
                    // return success result with ticket
                    return Task.FromResult(AuthenticateResult.Success(ticket));
                }

                // return failed result
                return Task.FromResult(AuthenticateResult.Fail("Authentication failed."));
            }
        }
    };
        
    app.UseBasicAuthentication(options);

    // other middleware e.g. MVC etc
}
```


