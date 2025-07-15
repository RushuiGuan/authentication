using System;
using System.Security.Authentication;

namespace Albatross.Authentication {
	public static class Extensions {
		public static ILogin GetRequired(this IGetCurrentLogin getCurrentLogin) {
			var login = getCurrentLogin.Get();
			if (login == null) {
				throw new AuthenticationException("No login found");
			}
			return login;
		}
		public static Guid SubjectGuid(this ILogin login) {
			try {
				return Guid.Parse(login.Subject);
			} catch (FormatException) {
				throw new AuthenticationException("Subject is not a valid unique identifier");
			}
		}

		public static Guid SubjectGuid(this IGetCurrentLogin login)
			=> login.GetRequired().SubjectGuid();
	}
}