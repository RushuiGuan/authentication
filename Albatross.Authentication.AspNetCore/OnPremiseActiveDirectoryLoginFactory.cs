using System.Security.Claims;

namespace Albatross.Authentication.AspNetCore {
	public class OnPremiseActiveDirectoryLoginFactory : ILoginFactory {
		public string Issuer => "AD AUTHORITY";

		public ILogin Create(IEnumerable<Claim> claims)
			=> new ActiveDirectoryLogin(claims);
	}
}