using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Albatross.Authentication {
	public interface ILoginFactory {
		string Issuer { get; }
		Login Create(IEnumerable<Claim> claims);
	}
}
