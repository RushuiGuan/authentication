using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Albatross.Authentication {
	public record ActiveDirectoryLogin : ILogin {
		public string Provider => "ActiveDirectory";
		public ActiveDirectoryLogin(IEnumerable<Claim> claims) {
			foreach (var claim in claims) {
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
				throw new InvalidOperationException("Active Directory jwt bearer token is missing primarysid");
			}
			if(string.IsNullOrEmpty(Name)){
				throw new InvalidOperationException("Active Directory jwt bearer token is missing name");
			}
		}
		public string Subject { get; } = string.Empty;
		public string Name { get; } = string.Empty;
	}
}