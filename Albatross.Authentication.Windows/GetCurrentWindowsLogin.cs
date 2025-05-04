namespace Albatross.Authentication.Windows {
	public class GetCurrentWindowsLogin : IGetCurrentLogin {
		public ILogin? Get() {
			var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
			return new WindowsLogin(identity);
		}
	}
}