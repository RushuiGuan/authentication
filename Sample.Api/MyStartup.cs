using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Sample.Api {
	public class MyStartup : Albatross.Hosting.Startup {
		public MyStartup(IConfiguration configuration) : base(configuration) { }
		public override void ConfigureServices(IServiceCollection services) {
			base.ConfigureServices(services);
		}
	}
}
