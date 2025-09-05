# Albatross.Authentication

A comprehensive .NET library that provides a unified interface for retrieving the identity of the current user across different platforms and authentication mechanisms.

## Features

- **Cross-platform user authentication** - Unified interface that works across different environments
- **Multiple authentication providers** - Support for Windows, ASP.NET Core, Google OAuth, and Active Directory
- **Extensible architecture** - Easy to add new authentication providers through the `ILoginFactory` interface
- **Standardized user information** - Consistent `ILogin` interface with Provider, Subject, and Name properties
- **Legacy support** - Backward compatibility with `IGetCurrentUser` interface for existing applications
- **Dependency injection ready** - Built-in support for Microsoft.Extensions.DependencyInjection

### Core Interfaces

- **[ILogin](./ILogin.cs)** - Represents a user login with the following properties:
  - `Provider`: The authentication provider (e.g., "Windows", "https://accounts.google.com")
  - `Subject`: The unique identifier of the user
  - `Name`: The display name of the user
- **[IGetCurrentLogin](./IGetCurrentLogin.cs)** - Service interface that returns the login information of the current user
- **[IGetCurrentUser](./IGetCurrentUser.cs)** - Legacy interface that returns the string identity of the current user (primarily used in Windows domain environments)
- **[ILoginFactory](./ILoginFactory.cs)** - Factory interface for creating login instances from claims

## Prerequisites

- **.NET Standard 2.0** or higher
- **dotnet SDK 8.0** or higher for building and development
- **Microsoft.Extensions.DependencyInjection** (for dependency injection scenarios)

### Platform-specific Requirements

- **Windows Authentication**: Windows environment with `System.Security.Principal.Windows` package
- **ASP.NET Core Authentication**: ASP.NET Core application with `Microsoft.AspNetCore.App` framework reference

## Installation

### Using .NET CLI

```bash
dotnet add package Albatross.Authentication
```

### Using Package Manager Console

```powershell
Install-Package Albatross.Authentication
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

# Run tests (requires .NET 9.0 SDK)
dotnet test
```

## Usage Examples

### Basic Usage with Dependency Injection

```csharp
using Microsoft.Extensions.DependencyInjection;
using Albatross.Authentication;

// Setup dependency injection
var services = new ServiceCollection();

// For Windows authentication
services.AddWindowsPrincipalProvider();

// For ASP.NET Core authentication
services.AddAspNetCorePrincipalProvider();

var serviceProvider = services.BuildServiceProvider();

// Get current user login information
var loginService = serviceProvider.GetRequiredService<IGetCurrentLogin>();
var login = loginService.Get();

if (login != null)
{
    Console.WriteLine($"Provider: {login.Provider}");
    Console.WriteLine($"Subject: {login.Subject}");
    Console.WriteLine($"Name: {login.Name}");
}

// Legacy interface for simple user name
var userService = serviceProvider.GetRequiredService<IGetCurrentUser>();
var userName = userService.Get();
Console.WriteLine($"Current User: {userName}");
```

### Creating Custom Authentication Providers

```csharp
using Albatross.Authentication;
using System.Security.Claims;

// Example: Custom Twitter authentication provider
public class TwitterLoginFactory : ILoginFactory
{
    public string Issuer => "https://twitter.com/i/oauth2/authorize";
    
    public Login Create(IEnumerable<Claim> claims)
    {
        var login = new Login("Twitter");
        foreach (var claim in claims)
        {
            switch (claim.Type)
            {
                case "name":
                    login.Name = claim.Value;
                    break;
                case "email":
                    login.Email = claim.Value;
                    break;
                case "sub":
                    login.Subject = claim.Value;
                    break;
            }
        }
        
        if (string.IsNullOrEmpty(login.Subject))
        {
            throw new InvalidOperationException(
                "Twitter JWT bearer token is missing sub claim. " +
                "Make sure the openid scope is included in the authentication request.");
        }
        
        return login;
    }
}

// Register the custom factory
services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoginFactory, TwitterLoginFactory>());
```

### ASP.NET Core Integration

```csharp
using Albatross.Authentication.AspNetCore;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Add ASP.NET Core authentication services
        services.AddAspNetCorePrincipalProvider();
        
        // Other service configuration...
    }
}

// In a controller or service
[ApiController]
public class UserController : ControllerBase
{
    private readonly IGetCurrentLogin _loginService;
    
    public UserController(IGetCurrentLogin loginService)
    {
        _loginService = loginService;
    }
    
    [HttpGet("current")]
    public IActionResult GetCurrentUser()
    {
        var login = _loginService.Get();
        return Ok(new { 
            Provider = login?.Provider,
            Subject = login?.Subject,
            Name = login?.Name 
        });
    }
}
```

## Project Structure

```
authentication/
├── Albatross.Authentication/              # Core library (.NET Standard 2.0)
│   ├── ILogin.cs                         # Core login interface
│   ├── IGetCurrentLogin.cs               # Current login service interface
│   ├── IGetCurrentUser.cs                # Legacy user service interface
│   ├── ILoginFactory.cs                  # Login factory interface
│   └── README.md                         # This file
├── Albatross.Authentication.AspNetCore/   # ASP.NET Core implementation
│   ├── GetCurrentLoginFromHttpContext.cs # HttpContext-based login service
│   ├── GoogleLoginFactory.cs             # Google OAuth login factory
│   ├── OnPremiseActiveDirectoryLoginFactory.cs # AD login factory
│   └── Extensions.cs                     # Dependency injection extensions
├── Albatross.Authentication.Windows/      # Windows implementation
│   ├── GetCurrentWindowsLogin.cs         # Windows-based login service
│   ├── GetCurrentWindowsUser.cs          # Windows-based user service
│   └── Extensions.cs                     # Dependency injection extensions
├── Albatross.Authentication.UnitTest/     # Unit tests
├── Sample.Api/                           # Sample API application
└── README.md                             # Repository overview
```

## Running Unit Tests

The project includes comprehensive unit tests to verify functionality across different authentication scenarios.

```bash
# Run all tests (requires .NET 9.0 SDK)
dotnet test

# Run specific test project
dotnet test Albatross.Authentication.UnitTest/Albatross.Authentication.UnitTest.csproj

# Run tests with detailed output
dotnet test --verbosity normal
```

### Test Coverage

- Windows authentication validation
- Anonymous login scenarios
- Claims-based authentication
- Login factory implementations
- HttpContext integration

## Contributing

We welcome contributions to improve Albatross.Authentication! Here's how you can contribute:

### Submitting Issues

1. Check existing issues to avoid duplicates
2. Use the issue template when creating new issues
3. Provide clear reproduction steps and environment details
4. Include relevant code samples when reporting bugs

### Making Changes

1. **Fork** the repository on GitHub
2. **Clone** your fork locally:
   ```bash
   git clone https://github.com/YOUR-USERNAME/authentication.git
   ```
3. **Create a branch** for your changes:
   ```bash
   git checkout -b feature/your-feature-name
   ```
4. **Make your changes** following the existing code style
5. **Add or update tests** for your changes
6. **Build and test** locally:
   ```bash
   dotnet build
   dotnet test
   ```
7. **Commit your changes** with a clear message:
   ```bash
   git commit -m "Add support for new authentication provider"
   ```
8. **Push** to your fork and **submit a pull request**

### Code Style Guidelines

- Follow existing code formatting and naming conventions
- Add XML documentation comments for public APIs
- Include unit tests for new functionality
- Keep changes focused and atomic
- Update documentation when adding new features

## License

This project is licensed under the **MIT License** - see the [LICENSE](../LICENSE) file for details.

### MIT License Summary

- ✅ Commercial use
- ✅ Modification
- ✅ Distribution
- ✅ Private use
- ❌ Liability
- ❌ Warranty

---

**Albatross.Authentication** - Developed and maintained by [Rushui Guan](https://github.com/RushuiGuan)