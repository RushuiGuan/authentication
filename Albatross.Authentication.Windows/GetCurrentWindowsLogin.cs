namespace Albatross.Authentication.Windows {
	public class GetCurrentWindowsLogin : IGetCurrentLogin {
		public Login? Get() {
			var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
			Login? login = null;
			if (identity != null) {
				login = new Login("Windows");
				login.Name = identity.Name;
				foreach (var claim in identity.Claims) {
					switch (claim.Type) {
						case System.Security.Claims.ClaimTypes.Name:
							login.Name = claim.Value;
							break;
						case System.Security.Claims.ClaimTypes.PrimarySid:
							login.Subject = claim.Value;
							break;
					}
				}
			}
			return login;
		}
	}
}