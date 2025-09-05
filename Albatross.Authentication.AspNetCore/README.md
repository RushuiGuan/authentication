# Albatross.Authentication.AspNetCore

ASP.NET Core integration for the Albatross Authentication library. This package provides implementations for retrieving user identity information from HttpContext in ASP.NET Core applications using claims-based authentication.

## Features

- **Claims-based Authentication Support**: Extract user identity from `HttpContext.User` claims
- **Multiple Login Providers**: Built-in support for Google social login and on-premise Active Directory
- **Extensible Login Factories**: Easy to add support for new authentication providers
- **User Identity Normalization**: Handles domain-prefixed usernames (e.g., `domain\user` → `user`)
- **Dependency Injection Integration**: Seamless integration with ASP.NET Core DI container
- **Anonymous User Handling**: Graceful handling of unauthenticated users

### Supported Authentication Providers

- **Google OAuth**: Support for Google social login with standard OpenID Connect claims
- **On-Premise Active Directory**: Windows-based authentication using `WindowsIdentity`
- **Custom Providers**: Extensible through `ILoginFactory` implementations

## Prerequisites

- **.NET SDK**: 8.0 or higher
- **ASP.NET Core**: This package requires the ASP.NET Core framework
- **Dependencies**: 
  - `Albatross.Authentication` (base library)
  - `Microsoft.AspNetCore.App` (framework reference)

## Installation

### Using .NET CLI

```bash
dotnet add package Albatross.Authentication.AspNetCore
```

### Using Package Manager Console

```powershell
Install-Package Albatross.Authentication.AspNetCore
```

### Building from Source

```bash
# Clone the repository
git clone https://github.com/RushuiGuan/authentication.git
cd authentication

# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run tests
dotnet test
```

## Usage

### Basic Setup

Register the authentication services in your ASP.NET Core application:

```csharp
using Albatross.Authentication.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Register Albatross Authentication services
builder.Services.AddAspNetCorePrincipalProvider();

var app = builder.Build();
```

### Getting Current User Information

#### Retrieve User Name

```csharp
using Microsoft.AspNetCore.Mvc;
using Albatross.Authentication;

[ApiController]
[Route("[controller]")]
public class HomeController : ControllerBase
{
    private readonly IGetCurrentUser _getCurrentUser;

    public HomeController(IGetCurrentUser getCurrentUser)
    {
        _getCurrentUser = getCurrentUser;
    }

    [HttpGet]
    public IActionResult GetUser()
    {
        var userName = _getCurrentUser.Get();
        return Ok(new { UserName = userName });
    }
}
```

#### Retrieve Full Login Information

```csharp
using Microsoft.AspNetCore.Mvc;
using Albatross.Authentication;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IGetCurrentLogin _getCurrentLogin;

    public AuthController(IGetCurrentLogin getCurrentLogin)
    {
        _getCurrentLogin = getCurrentLogin;
    }

    [HttpGet("profile")]
    public IActionResult GetProfile()
    {
        var login = _getCurrentLogin.Get();
        if (login != null)
        {
            return Ok(new 
            {
                Provider = login.Provider,
                Subject = login.Subject,
                Name = login.Name
            });
        }
        return Unauthorized();
    }
}
```

### Adding Custom Authentication Providers

You can easily extend support for new authentication providers by implementing `ILoginFactory`:

```csharp
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Albatross.Authentication;

// Example: Twitter social login support
public record TwitterLogin : ILogin 
{
    public string Provider => "Twitter";
    public string Subject { get; } = string.Empty;
    public string Name { get; } = string.Empty;
    public string Email { get; init; } = string.Empty;

    public TwitterLogin(IEnumerable<Claim> claims) 
    {
        foreach(var claim in claims) 
        {
            switch (claim.Type) 
            {
                case "name":
                    Name = claim.Value;
                    break;
                case "email":
                    Email = claim.Value;
                    break;
                case "sub":
                    Subject = claim.Value;
                    break;
            }
        }
        if (string.IsNullOrEmpty(Subject)) 
        {
            throw new InvalidOperationException("Twitter jwt bearer token is missing sub claim. Make sure the openid scope is included in the authentication request.");
        }
        if (string.IsNullOrEmpty(Name)) 
        {
            throw new InvalidOperationException("Twitter jwt bearer token is missing name claim.");
        }
    }
}

public class TwitterLoginFactory : ILoginFactory 
{
    public string Issuer => "https://twitter.com/i/oauth2/authorize";
    public ILogin Create(IEnumerable<Claim> claims) => new TwitterLogin(claims);
}

// Register the custom factory
services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoginFactory, TwitterLoginFactory>());
```

### Extension Methods

The library provides helpful extension methods for working with user identity:

```csharp
using Albatross.Authentication.AspNetCore;

// Get identity from ClaimsPrincipal
var identity = User.GetIdentity();

// Get identity from HttpContext
var identity = HttpContext.GetIdentity();

// Normalize domain-prefixed usernames
var normalizedName = "domain\\user".NormalizeIdentity(); // Returns "user"
```

## Project Structure

```
Albatross.Authentication.AspNetCore/
├── Extensions.cs                           # Extension methods for DI and identity handling
├── GetCurrentUserFromHttpContext.cs       # IGetCurrentUser implementation
├── GetCurrentLoginFromHttpContext.cs      # IGetCurrentLogin implementation
├── GoogleLogin.cs                          # Google login model
├── GoogleLoginFactory.cs                  # Google OAuth login factory
├── OnPremiseActiveDirectoryLoginFactory.cs # AD login factory
├── ActiveDirectoryLogin.cs                # AD login model
├── Albatross.Authentication.AspNetCore.csproj
└── README.md
```

## Running Tests

The project includes comprehensive unit tests. To run them:

```bash
# Run all tests
dotnet test

# Run with detailed output
dotnet test --verbosity normal

# Run specific test project
dotnet test Albatross.Authentication.UnitTest/
```

### Test Coverage

- Identity normalization (domain\\user → user)
- Anonymous user handling
- Claims extraction from HttpContext
- Login factory registration and resolution

## Contributing

We welcome contributions! Here's how to get started:

1. **Fork the repository** on GitHub
2. **Clone your fork** locally:
   ```bash
   git clone https://github.com/yourusername/authentication.git
   ```
3. **Create a feature branch**:
   ```bash
   git checkout -b feature/your-feature-name
   ```
4. **Make your changes** and add tests
5. **Run tests** to ensure everything works:
   ```bash
   dotnet test
   ```
6. **Commit your changes**:
   ```bash
   git commit -m "Add your meaningful commit message"
   ```
7. **Push to your fork**:
   ```bash
   git push origin feature/your-feature-name
   ```
8. **Create a Pull Request** on GitHub

### Development Guidelines

- Follow existing code style and conventions
- Add unit tests for new functionality
- Update documentation for public APIs
- Ensure all tests pass before submitting PR

## License

This project is licensed under the MIT License. See the [LICENSE](../LICENSE) file for details.

```
MIT License

Copyright (c) 2019 Rushui Guan

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```

---

**Developed and maintained by [Rushui Guan](https://github.com/RushuiGuan)**