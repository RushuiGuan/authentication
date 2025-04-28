using Microsoft.AspNetCore.Http;

namespace Albatross.Authentication.AspNetCore {
	public class GetCurrentLoginFromHttpContext : IGetCurrentLogin {
		Dictionary<string, ILoginFactory> factories = new Dictionary<string, ILoginFactory>();
		IHttpContextAccessor httpContextAccessor;
		public GetCurrentLoginFromHttpContext(IHttpContextAccessor httpContextAccessor, IEnumerable<ILoginFactory> factories) {
			this.httpContextAccessor = httpContextAccessor;
			foreach(var factory in factories) {
				this.factories[factory.Issuer] = factory;
			}
		}

		public Login? Get() {
			var claims = httpContextAccessor.HttpContext?.User.Claims;
			if (claims != null ) {
				var first = claims.FirstOrDefault();
				if (first != null) {
					if (factories.TryGetValue(first.Issuer, out var factory)) {
						return factory.Create(claims);
					} else {
						throw new InvalidOperationException($"Login factory for issuer {first.Issuer} is not registered");
					}
				} else {
					return null;
				}
			}
			return null;
		}
	}
}