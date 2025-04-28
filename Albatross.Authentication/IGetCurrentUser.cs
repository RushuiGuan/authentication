using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Authentication {
	[Obsolete("Use IGetCurrentLogin instead.")]
	public interface IGetCurrentUser {
		string Get();
		string Provider { get; }
	}
}