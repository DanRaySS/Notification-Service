using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Notification_Service.Application.ErrorTypes;
using Notification_Service.Application.Services;
using Notification_Service.DTOs;
using Notification_Service.Entities;

namespace Notification_Service.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    public class NotificationController : ControllerBase
    {
        public ILogger<NotificationController> _logger { get; }
        private readonly NotificationService _service;

        public NotificationController(ILogger<NotificationController> logger, NotificationService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpPost("send/email")]
        public async Task<IActionResult> SendEmailNotification([FromBody]CreateNotificationDto command, CancellationToken cancellationToken)
        {
            var result = await _service.SendNotification(command, ChannelType.Email, cancellationToken);
            if (!result.IsSuccessfull)
            {
                var err = result.GetErrors().FirstOrDefault();

                if (err is ValidationError)
                    return BadRequest(err);
                else if (err is NotificationNotFoundError)
                    return NotFound(err);
            }
            return Ok();
        }

        [HttpPost("send/all")]
        public async Task<IActionResult> SendAllChannelsNotification([FromBody]CreateNotificationDto command, CancellationToken cancellationToken)
        {
            var result = await _service.SendNotification(command, ChannelType.All, cancellationToken);
            if (!result.IsSuccessfull)
            {
                var err = result.GetErrors().FirstOrDefault();

                if (err is ValidationError)
                    return BadRequest(err);
                else if (err is NotificationNotFoundError)
                    return NotFound(err);
            }
            return Ok();
        }

        [HttpPut("updateById")]
        public async Task<IActionResult> UpdateNotificationById([FromQuery]UpdateNotificationDto command, [FromQuery] Guid id, CancellationToken cancellationToken)
        {
            var result = await _service.UpdateNotificationById(command, id, cancellationToken);
            if (!result.IsSuccessfull)
            {
                var err = result.GetErrors().FirstOrDefault();

                if (err is ValidationError)
                    return BadRequest(err);
                else if (err is NotificationNotFoundError)
                    return NotFound(err);
            }
            return Ok();
        }

        [HttpGet("getById")]
        public async Task<IActionResult> GetNotificationById([FromQuery] Guid id, CancellationToken cancellationToken)
        {
            var result = await _service.GetNotificationById(id, cancellationToken);
            if (!result.IsSuccessfull)
            {
                var err = result.GetErrors().FirstOrDefault();

                if (err is ValidationError)
                    return BadRequest(err);
                else if (err is NotificationNotFoundError)
                    return NotFound(err);
            }
            return Ok(result.Value);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllNotifications(CancellationToken cancellationToken)
        {
            var result = await _service.GetAllNotifications(cancellationToken);
            if (!result.IsSuccessfull)
            {
                var err = result.GetErrors().FirstOrDefault();

                if (err is ValidationError)
                    return BadRequest(err);
                else if (err is NotificationNotFoundError)
                    return NotFound(err);
            }
            return Ok(result.Value);
        }
    }
}
