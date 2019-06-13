using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pspl.Api.Requests;
using Pspl.Api.Responses;
using Pspl.Shared.Extensions;
using Pspl.Shared.Notifications;
using Pspl.Shared.Repositories;
using Pspl.Shared.Utils;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Pspl.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdController : BaseController
    {
        private readonly ILogger<AdController> _logger;
        private readonly IAdRepository _adRepository;

        public AdController(
            ILogger<AdController> logger,
            IAdRepository adRepository,
            IPushOverNotification pushOverNotification)
            : base(pushOverNotification)
        {
            _logger = logger;
            _adRepository = adRepository;
        }

        [AllowAnonymous]
        [HttpPost("info")]
        public async Task<IActionResult> GetInfo(InfoRequest infoRequest)
        {
            var logMsg = $"Request for: {infoRequest.Url}";

            try
            {
                _logger.LogInformation(logMsg);

                var result = (await _adRepository.FindByAsync(a => a.Url == infoRequest.Url))
                    .FirstOrDefault();

                try
                {
                    var msg = (result is null) ? " not found" : " found";

                    logMsg = logMsg + msg;

                    PushOver.Send(Statics.ApiBaner, logMsg);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.GetFullMessage());
                }

                if (!(result is null))
                {
                    return Ok(new AdResponse
                    {
                        IsSuspicious = true,
                        Url = result.Url,
                        Description = result.Description,
                        Name = result.Name
                    });
                }
                else
                    return Ok(new EmptyResponse { IsSuspicious = false, Message = "No result" });
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.GetFullMessage());

                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}