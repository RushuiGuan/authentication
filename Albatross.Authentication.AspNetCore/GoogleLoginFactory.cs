using System.Security.Claims;

namespace Albatross.Authentication.AspNetCore {
	public class GoogleLoginFactory : ILoginFactory {
		public const string ClaimType_Name = "name";
		public const string ClaimType_Email = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
		public const string ClaimType_EmailVerified = "email_verified";
		public const string ClaimType_Picture = "picture";
		public const string ClaimType_GivenName = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname";
		public const string ClaimType_Surname = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname";
		public const string ClaimType_Sub = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
		public const string ClaimType_Audience = "aud";
		public const string ClaimType_Issuer = "iss";
		
		public string Issuer => "https://accounts.google.com";
		
		public Login Create(IEnumerable<Claim> claims) {
			var login = new Login("Google");
			foreach(var claim in claims) {
				switch (claim.Type) {
					case ClaimType_Name:
						login.Name = claim.Value;
						break;
					case ClaimType_Email:
						login.Email = claim.Value;
						break;
					case ClaimType_EmailVerified:
						login.EmailVerified = bool.Parse(claim.Value);
						break;
					case ClaimType_Picture:
						login.Picture = claim.Value;
						break;
					case ClaimType_GivenName:
						login.GivenName = claim.Value;
						break;
					case ClaimType_Surname:
						login.Surname = claim.Value;
						break;
					case ClaimType_Sub:
						login.Subject = claim.Value;
						break;
				}
			}
			if (string.IsNullOrEmpty(login.Subject)) {
				throw new InvalidOperationException("Google jwt bearer token is missing sub claim.  Make sure the openid scope is included in the authentication request.");
			}
			return login;
		}
	}
}
