using Microsoft.AspNetCore.Mvc;
using PushApiService.Dto;
using PushApiService.Interfaces;

namespace PushApiService.Controllers
{
	[ApiController]
	public class PushController : ControllerBase
	{
		private readonly IPushRepository _repo;

		public PushController(IPushRepository repo)
		{
			_repo = repo;
		}

		[HttpPost]
		[Route("api/push/registerDevice")]
		public ActionResult RegisterDevice([FromBody]DeviceInformation request)
		{
			_repo.RegisterDevice(request);

			return new OkResult();
		}

		[HttpPost]
		[Route("api/push/updateDevice")]
		public ActionResult UpdateDevice([FromBody]DeviceInformation request)
		{
			_repo.UpdateDevice(request);

			return new OkResult();
		}

		[HttpPost]
		[Route("api/push/sendPush")]
		public ActionResult SendPush([FromBody]PushSend request)
		{
			_repo.SendPush(request);

			return new OkResult();
		}
	}
}