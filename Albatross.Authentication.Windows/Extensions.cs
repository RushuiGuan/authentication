using Microsoft.Extensions.DependencyInjection;

namespace Albatross.Authentication.Windows {
	public static class Extensions {
		public static IServiceCollection AddWindowsPrincipalProvider(this IServiceCollection svc) {
#pragma warning disable CS0618 // Type or member is obsolete
			svc.AddSingleton<IGetCurrentUser, GetCurrentWindowsUser>();
#pragma warning restore CS0618 // Type or member is obsolete
			svc.AddSingleton<IGetCurrentLogin, GetCurrentWindowsLogin>();
			return svc;
		}
	}
}