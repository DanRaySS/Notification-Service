using MediatR;
using Microsoft.AspNetCore.Mvc;
using Notification_Service.Application.Features.Notifications;

namespace Notification_Service.Controllers
{
    [Route("api/notifications")]
    public class NotificationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ILogger<NotificationController> _logger { get; }

        public NotificationController(ILogger<NotificationController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost("")]
        public IActionResult CreateNotification([FromBody]CreateNotificationCommand command)
        {
            return Ok();
        }

        [HttpGet("getById")]
        public async Task<IActionResult> GetNotification(GetNotificationQuery query, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            if (!result.IsSuccessfull)
            {
                return BadRequest(result.GetErrors().FirstOrDefault());
            }
            return Ok();
        }

        [HttpGet("search")]
        public IActionResult SearchNotifications()
        {
            return Ok();
        }
    }
}
