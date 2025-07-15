# About
An assembly that returns the identity of the current user.

# Features
* [ILogin](./ILogin.cs) interface that represents a user login with the following properties:
  * Issuer: The issuer of the login (e.g., "Windows", "htts://accounts.google.com").
  * Subject: The id of the login
  * Name: The name of the login
* [IGetCurrentLogin](./IGetCurrentLogin.cs) interface that returns the login of the current user.

# Implementations
* [Albatross.Authentication.AspNetCore](../Albatross.Authentication.AspNetCore/)
* [Albatross.Authentication.Windows](../Albatross.Authentication.Windows/)