using Sample.Api;

namespace SampleApi {
	internal class Program {
		public static Task Main(string[] args) {
			Albatross.Logging.Extensions.RemoveLegacySlackSinkOptions();
			return new Albatross.Hosting.Setup(args, true)
				.ConfigureWebHost<MyStartup>()
				.RunAsync();
		}
	}
}
