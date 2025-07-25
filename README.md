|Assembly| Version                                                               |
|-|-----------------------------------------------------------------------|
|[Albatross.Authentication](./Albatross.Authentication/) |[![NuGet Version](https://img.shields.io/nuget/v/Albatross.Authentication)](https://www.nuget.org/packages/Albatross.Authentication)|
|[Albatross.Authentication.Windows](./Albatross.Authentication.Windows/) |[![NuGet Version](https://img.shields.io/nuget/v/Albatross.Authentication.Windows)](https://www.nuget.org/packages/Albatross.Authentication.Windows)|
|[Albatross.Authentication.AspNetCore](./Albatross.Authentication.AspNetCore/) |[![NuGet Version](https://img.shields.io/nuget/v/Albatross.Authentication.AspNetCore)](https://www.nuget.org/packages/Albatross.Authentication.AspNetCore)|

## Overview

This repository contains **Albatross Authentication**, a set of .NET class libraries for retrieving a user’s identity in different environments. The base library (`Albatross.Authentication`) exposes abstractions that represent a user login and provides methods to fetch the current login and user name. Two concrete packages extend it:

1. **Albatross.Authentication.AspNetCore** — for use in ASP.NET Core applications
2. **Albatross.Authentication.Windows** — for use in Windows (e.g., service) environments

All packages are distributed as NuGet libraries.

---

## Base Library: `Albatross.Authentication`

### Key Interfaces

- `ILogin`

	- Properties: `Provider`, `Subject`, `Name`

- `IGetCurrentLogin`

	- Method: `ILogin Get()`

- `IGetCurrentUser`

	- Method: `string Get()`

- `ILoginFactory`

	- Method: `ILogin? Create(string provider, string subject, string name)`

### Static Helpers

- `Extensions.NormalizeIdentity(string identity)` (normalizes domain-prefixed usernames)

---

## ASP.NET Core Integration: `Albatross.Authentication.AspNetCore`

### Purpose

Provides integration with ASP.NET Core authentication mechanisms. Extracts login details from `HttpContext.User` using claims-based authentication.

### Features

- Service implementations:

	- `IGetCurrentLogin` from `HttpContext`
	- `IGetCurrentUser` as shortcut to `ILogin.Name`

- Factory implementations:

	- `GoogleLoginFactory` — uses standard claim types like `name`, `sub`, and `iss`
	- `OnPremiseActiveDirectoryLoginFactory` — uses `WindowsIdentity.Name`

- Extension Methods:

	- `AddAspNetCoreLogin()` to register ASP.NET Core services

---

## Windows Integration: `Albatross.Authentication.Windows`

### Purpose

Provides identity extraction in Windows-hosted apps (e.g., background services) using `WindowsIdentity.GetCurrent().Name`.

### Features

- `GetCurrentWindowsLogin` (returns `ILogin`)
- `GetCurrentWindowsUser` (returns user name string)
- Registers as `IGetCurrentLogin` and `IGetCurrentUser`
- Extension Method: `AddWindowsLogin()`

---

## Sample Usage

```csharp
services.AddWindowsLogin();
// or
services.AddAspNetCoreLogin();

var user = provider.GetRequiredService<IGetCurrentUser>().Get();
```

You can retrieve the normalized subject or name from the login service directly:

```csharp
var login = provider.GetRequiredService<IGetCurrentLogin>().Get();
Console.WriteLine(login.Subject);
```

---

## Unit Tests

Tests are included in `Albatross.Authentication.UnitTest` for both Windows and ASP.NET Core scenarios. Example tests:

- `NormalizeIdentity`
- `Anonymous Login`
- Fake `HttpContext` injection with claims

---

## License

MIT License. See `LICENSE` file for details.

---

## Authors & Maintainers

Developed and maintained by [Rushui Guan](https://github.com/RushuiGuan).

