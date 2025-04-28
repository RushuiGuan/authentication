using System;
using Xunit;

namespace Albatross.Authentication.UnitTest {
	public class TestWindowsAuthentication {
		[Fact]
		public void Run() {
#pragma warning disable CS0618 // Type or member is obsolete
			var user = new Windows.GetCurrentWindowsUser().Get();
#pragma warning restore CS0618 // Type or member is obsolete
			Assert.Equal(Environment.UserName, user);
		}


		[Fact]
		public void TestLogin() {
			var login = new Windows.GetCurrentWindowsLogin().Get();
			Assert.NotNull(login);
			Assert.Equal("Windows", login.Provider);
			Assert.Equal(Environment.UserDomainName + "\\" + Environment.UserName, login.Name);
		}
	}
}