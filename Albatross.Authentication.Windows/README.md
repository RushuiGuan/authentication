# Albatross.Authentication.Windows

A .NET library that provides Windows-specific authentication functionality for retrieving the identity of the current Windows user. This library integrates with the Albatross.Authentication framework to provide seamless Windows authentication support in Windows-hosted applications.

## Features

- **Windows Identity Extraction**: Retrieves the current Windows user identity using `System.Security.Principal.WindowsIdentity.GetCurrent()`
- **Domain User Support**: Automatically normalizes domain usernames by removing the domain prefix (e.g., `DOMAIN\username` becomes `username`)
- **ILogin Implementation**: Provides `WindowsLogin` record that implements the `ILogin` interface with Provider, Subject, and Name properties
- **Service Implementations**:
  - `IGetCurrentLogin` implementation for getting structured login information
  - `IGetCurrentUser` implementation for getting the username as a string (legacy support)
- **Dependency Injection**: Built-in extension methods for easy service registration
- **Claims-based Authentication**: Extracts user information from Windows identity claims (PrimarySid and Name)

## Prerequisites

- **.NET SDK**: .NET 8.0 or .NET 9.0
- **Target Framework**: `net8.0-windows` or `net9.0-windows`
- **Operating System**: Windows (required for Windows Identity functionality)
- **Dependencies**:
  - `Microsoft.Extensions.DependencyInjection.Abstractions` (v9.0.7)
  - `System.Security.Principal.Windows` (v5.0.0)
  - `Albatross.Authentication` (base library)

## Installation

### 1. Install the NuGet Package

```bash
dotnet add package Albatross.Authentication.Windows
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Build the Project

```bash
dotnet build
```

### 4. Run the Application

```bash
dotnet run
```

## Example Usage

### Basic Setup with Dependency Injection

```csharp
using Albatross.Authentication.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// Register Windows authentication services
var builder = Host.CreateDefaultBuilder();
builder.ConfigureServices(services =>
{
    services.AddWindowsPrincipalProvider();
});

var host = builder.Build();
```

### Getting Current User Information

```csharp
using Albatross.Authentication;

// Get the current user as a string (legacy method)
var userService = serviceProvider.GetRequiredService<IGetCurrentUser>();
string currentUser = userService.Get();
Console.WriteLine($"Current user: {currentUser}"); // Output: "john.doe" (without domain)

// Get structured login information
var loginService = serviceProvider.GetRequiredService<IGetCurrentLogin>();
ILogin login = loginService.Get();
Console.WriteLine($"Provider: {login.Provider}"); // Output: "Windows"
Console.WriteLine($"Subject: {login.Subject}");   // Output: User's SID
Console.WriteLine($"Name: {login.Name}");         // Output: "DOMAIN\john.doe"
```

### Creating WindowsLogin Directly

```csharp
using System.Security.Principal;
using Albatross.Authentication.Windows;

// Get current Windows identity
var identity = WindowsIdentity.GetCurrent();

// Create WindowsLogin instance
var windowsLogin = new WindowsLogin(identity);

Console.WriteLine($"Provider: {windowsLogin.Provider}"); // "Windows"
Console.WriteLine($"Subject: {windowsLogin.Subject}");   // User's SID
Console.WriteLine($"Name: {windowsLogin.Name}");         // Full domain\username
```

### Integration in ASP.NET Core or Windows Services

```csharp
// In a Windows Service or desktop application
public class MyWorkerService : BackgroundService
{
    private readonly IGetCurrentLogin _getCurrentLogin;

    public MyWorkerService(IGetCurrentLogin getCurrentLogin)
    {
        _getCurrentLogin = getCurrentLogin;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var currentLogin = _getCurrentLogin.Get();
        // Use the login information for auditing, logging, etc.
    }
}
```

## Project Structure

```
Albatross.Authentication.Windows/
├── Albatross.Authentication.Windows.csproj    # Project file with dependencies and targeting
├── README.md                                  # This documentation file
├── WindowsLogin.cs                           # ILogin implementation for Windows
├── GetCurrentWindowsLogin.cs                 # Service to get ILogin from Windows identity
├── GetCurrentWindowsUser.cs                  # Service to get username string (legacy)
└── Extensions.cs                             # Dependency injection extension methods
```

### Key Components

- **WindowsLogin**: A record that implements `ILogin` interface, extracting user information from Windows identity claims
- **GetCurrentWindowsLogin**: Service class that creates `WindowsLogin` instances from the current Windows identity
- **GetCurrentWindowsUser**: Legacy service that returns the current username as a string with domain prefix removed
- **Extensions**: Contains `AddWindowsPrincipalProvider()` method for easy dependency injection setup

## How to Run Unit Tests

### Prerequisites for Testing
- Ensure you're running on a Windows machine with a valid Windows user context
- The tests use `Environment.UserName` and `Environment.UserDomainName` for validation

### Running Tests

```bash
# Navigate to the test project directory
cd Albatross.Authentication.UnitTest

# Run all tests
dotnet test

# Run only Windows authentication tests
dotnet test --filter "TestWindowsAuthentication"

# Run with verbose output
dotnet test --verbosity normal
```

### Example Test Cases

The unit tests validate:
- User identity extraction matches `Environment.UserName`
- Login provider is correctly set to "Windows"
- Domain and username formatting is correct
- Claims extraction from Windows identity works properly

## Contributing Guidelines

We welcome contributions to improve the Albatross.Authentication.Windows library!

### Steps to Contribute

1. **Fork the Repository**: Create a fork of the [authentication repository](https://github.com/RushuiGuan/authentication)

2. **Create a Feature Branch**:
   ```bash
   git checkout -b feature/your-feature-name
   ```

3. **Make Your Changes**:
   - Follow existing code style and conventions
   - Add unit tests for new functionality
   - Update documentation as needed

4. **Test Your Changes**:
   ```bash
   dotnet test
   ```

5. **Submit a Pull Request**:
   - Provide a clear description of your changes
   - Reference any related issues
   - Ensure all tests pass

### Code Standards

- Follow C# coding conventions
- Use meaningful variable and method names
- Add XML documentation comments for public APIs
- Maintain compatibility with existing interfaces
- Target the appropriate .NET frameworks (net8.0-windows, net9.0-windows)

### Reporting Issues

- Use the [GitHub Issues](https://github.com/RushuiGuan/authentication/issues) page
- Provide detailed reproduction steps
- Include environment information (OS, .NET version, etc.)

## License

This project is licensed under the **MIT License**. See the [LICENSE](../LICENSE) file for the full license text.

### MIT License Summary

- ✅ Commercial use allowed
- ✅ Modification allowed
- ✅ Distribution allowed
- ✅ Private use allowed
- ❌ No warranty provided
- ❌ No liability accepted

---

**Developed and maintained by [Rushui Guan](https://github.com/RushuiGuan)**

For more information about the broader authentication framework, see the [main repository README](../README.md).