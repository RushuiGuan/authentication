using System;
using Xunit;

namespace Albatross.Authentication.UnitTest {
	/// <summary>
	/// A <see cref="FactAttribute"/> that is skipped unless the tests are running on Windows. The Windows
	/// authentication implementation relies on Windows-only APIs, so these tests build everywhere (enabled by
	/// EnableWindowsTargeting) but only execute on Windows.
	/// </summary>
	public sealed class WindowsFactAttribute : FactAttribute {
		public WindowsFactAttribute() {
			if (!OperatingSystem.IsWindows()) {
				Skip = "Windows-only test; skipped on non-Windows platforms.";
			}
		}
	}
}
