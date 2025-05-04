using System.Security.Claims;

namespace Albatross.Authentication.AspNetCore {
	public class GoogleLoginFactory : ILoginFactory {
		public string Issuer => "https://accounts.google.com";
		public ILogin Create(IEnumerable<Claim> claims) => new GoogleLogin(claims);
	}
}