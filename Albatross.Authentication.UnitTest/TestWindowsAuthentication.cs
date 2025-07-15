using System;
using Xunit;

namespace Albatross.Authentication.UnitTest {
	public class TestWindowsAuthentication {
		[Fact]
		public void Run() {
			var user = new Windows.GetCurrentWindowsUser().Get();
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