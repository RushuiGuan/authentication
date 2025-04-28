using System.Security.Claims;

namespace Albatross.Authentication.AspNetCore {
	public class OnPremiseActiveDirectoryLoginFactory : ILoginFactory {
		public const string ClaimType_Name = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
		public const string ClaimType_PrimarySID = "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid";

		public string Issuer => "AD AUTHORITY";

		public Login Create(IEnumerable<Claim> claims) {
			var login = new Login("ActiveDirectory");
			foreach (var claim in claims) {
				switch (claim.Type) {
					case ClaimType_PrimarySID:
						login.Subject = claim.Value;
						break;
					case ClaimType_Name:
						login.Name = claim.Value.NormalizeIdentity();
						break;
				}
			}
			if (string.IsNullOrEmpty(login.Subject)) {
				throw new InvalidOperationException("Active Directory jwt bearer token is missing primarysid");
			}
			return login;
		}
	}
}
