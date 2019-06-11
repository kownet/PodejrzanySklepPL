using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pspl.Api.Requests;
using Pspl.Api.Responses;
using Pspl.Shared.Extensions;
using Pspl.Shared.Repositories;
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
            IAdRepository adRepository)
        {
            _logger = logger;
            _adRepository = adRepository;
        }

        [AllowAnonymous]
        [HttpGet("info")]
        public async Task<IActionResult> GetInfo(InfoRequest infoRequest)
        {
            try
            {
                _logger.LogInformation($"Request for: {infoRequest.Url}");

                var result = (await _adRepository.FindByAsync(a => a.Url == infoRequest.Url))
                    .FirstOrDefault();

                if (!(result is null))
                {
                    return Ok(new AdResponse
                    {
                        Url = result.Url,
                        Description = result.Description,
                        Name = result.Name
                    });
                }
                else
                    return Ok(new EmptyResponse { Message = "No result" });
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.GetFullMessage());

                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}