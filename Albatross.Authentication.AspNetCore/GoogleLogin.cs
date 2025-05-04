using System.Security.Claims;

namespace Albatross.Authentication.AspNetCore {
	public record GoogleLogin : ILogin {
		public string Provider => "Google";
		public GoogleLogin(IEnumerable<Claim> claims) {
			foreach(var claim in claims) {
				switch (claim.Type) {
					case ClaimType_Name:
						Name = claim.Value;
						break;
					case ClaimType_Email:
						Email = claim.Value;
						break;
					case ClaimType_EmailVerified:
						EmailVerified = bool.Parse(claim.Value);
						break;
					case ClaimType_Picture:
						Picture = claim.Value;
						break;
					case ClaimType_GivenName:
						GivenName = claim.Value;
						break;
					case ClaimType_Surname:
						Surname = claim.Value;
						break;
					case ClaimType_Sub:
						Subject = claim.Value;
						break;
				}
			}
			if (string.IsNullOrEmpty(Subject)) {
				throw new InvalidOperationException("Google jwt bearer token is missing sub claim.  Make sure the openid scope is included in the authentication request.");
			}
			if (string.IsNullOrEmpty(Name)) {
				throw new InvalidOperationException("Google jwt bearer token is missing name claim.  Make sure the profile scope is included in the authentication request.");
			}
			if(string.IsNullOrEmpty(Email)) {
				throw new InvalidOperationException("Google jwt bearer token is missing email claim.  Make sure the email scope is included in the authentication request.");
			}
		}
		public string Subject { get; }
		public string Name { get; }
		public string Email { get; init; }
		public bool EmailVerified { get; init; }
		public string? GivenName { get; init; }
		public string? Surname { get; init; }
		public string? Picture { get; init; }
		
		
		public const string ClaimType_Name = "name";
		public const string ClaimType_Email = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
		public const string ClaimType_EmailVerified = "email_verified";
		public const string ClaimType_Picture = "picture";
		public const string ClaimType_GivenName = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname";
		public const string ClaimType_Surname = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname";
		public const string ClaimType_Sub = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
		public const string ClaimType_Audience = "aud";
		public const string ClaimType_Issuer = "iss";
	}
}