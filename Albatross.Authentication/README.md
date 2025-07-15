# About
An assembly that returns the identity of the current user.

# Features
* [ILogin](./ILogin.cs) interface that represents a user login with the following properties:
  * Issuer: The issuer of the login (e.g., "Windows", "htts://accounts.google.com").
  * Subject: The id of the login
  * Name: The name of the login
* [IGetCurrentLogin](./IGetCurrentLogin.cs) interface that returns the login of the current user.
* [IGetCurrentUser](./IGetCurrentUser.cs) interface that return the string identity of the current user.  The interface is considered as legacy and are used in a windows domain environment.

# Implementations
* [Albatross.Authentication.AspNetCore](../Albatross.Authentication.AspNetCore/)
* [Albatross.Authentication.Windows](../Albatross.Authentication.Windows/)