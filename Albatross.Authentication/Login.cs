namespace Albatross.Authentication {
	public record class Login {
		public Login(string provider){
			Provider = provider;
		}
		public string Provider { get; set; }
		public string Subject { get; set; } = string.Empty;
		
		public string? Name { get; set; }
		public string? GivenName { get; set; }
		public string? Surname { get; set; }

		public string? Email { get; set; }
		public bool EmailVerified { get; set; }
		public string? Picture { get; set; }
	}
}