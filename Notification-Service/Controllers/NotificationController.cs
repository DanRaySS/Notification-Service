using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Notification_Service.Application.Features.Notifications;
using Notification_Service.DTOs;

namespace Notification_Service.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    public class NotificationController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ILogger<NotificationController> _logger { get; }
        private readonly IMapper _mapper;

        public NotificationController(ILogger<NotificationController> logger, IMediator mediator, IMapper mapper)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateNotification([FromBody]CreateNotificationDto command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            if (!result.IsSuccessfull)
            {
                return BadRequest(result.GetErrors().FirstOrDefault());
            }
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

        // [HttpGet("all")]
        // public async Task<ActionResult<List<NotificationDto>>> GetAllNotifications()
        // {
            
        // }
    }
}
