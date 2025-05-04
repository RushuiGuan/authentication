using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Albatross.Authentication {
	public interface ILoginFactory {
		string Issuer { get; }
		ILogin Create(IEnumerable<Claim> claims);
	}
}
