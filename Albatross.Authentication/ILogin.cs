namespace Albatross.Authentication {
	public interface ILogin {
		public string Provider { get; }
		public string Subject { get; } 
		public string Name { get;  }
	}
}