using Microsoft.AspNetCore.Mvc;
using Pspl.Shared.Notifications;

namespace Pspl.Api.Controllers
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected readonly IPushOverNotification PushOver;

        public BaseController(
            IPushOverNotification pushOverNotification)
        {
            PushOver = pushOverNotification;
        }
    }
}