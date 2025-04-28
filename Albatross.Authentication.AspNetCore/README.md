# About
Return the identity of the current aspnetcore user through HttpContext.

## Features
Currently support Google social login and on premise active directory login.  But new login can be supported easily by creating implemenatations for [ILoginFactory](../Albatross.Authentication/ILoginFactory.cs).  Here is an example:
```c#
// new code to support twitter social login
public class TwitterLoginFactory : ILoginFactory {
	public string Issuer => "https://twitter.com/i/oauth2/authorize";
	public Login Create(IEnumerable<Claim> claims) {
		var login = new Login("Twitter");
		foreach(var claim in claims) {
			switch (claim.Type) {
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
		if (string.IsNullOrEmpty(login.Subject)) {
			throw new InvalidOperationException("Twitter jwt bearer token is missing sub claim.  Make sure the openid scope is included in the authentication request.");
		}
		return login;
	}
}
// not register this factory in your di registration container
services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoginFactory, TwitterLoginFactory>());
```

