using System.Security.Claims;
using System.Security.Principal;

namespace Albatross.Authentication.Windows {
	public record WindowsLogin : ILogin {
		public string Provider => "Windows";
		public WindowsLogin(WindowsIdentity identity) {
			foreach (var claim in identity.Claims) {
				switch (claim.Type) {
					case ClaimTypes.PrimarySid:
						Subject = claim.Value;
						break;
					case ClaimTypes.Name:
						Name = claim.Value;
						break;
				}
			}
			if (string.IsNullOrEmpty(Subject)) {
				throw new InvalidOperationException("Windows identity is missing primarysid claim");
			}
			if(string.IsNullOrEmpty(Name)){
				throw new InvalidOperationException("Window identify is missing name claim");
			}
		}
		public string Subject { get; } = string.Empty;
		public string Name { get; } = string.Empty;
	}
}