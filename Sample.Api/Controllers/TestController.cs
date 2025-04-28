using Albatross.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sample.Api.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class TestController : ControllerBase {
		private readonly IGetCurrentLogin getCurrent;

		public TestController(IGetCurrentLogin getCurrent) {
			this.getCurrent = getCurrent;
		}

		[HttpGet]
		[Authorize]
		public Login? Get() => getCurrent.Get();
	}
}
