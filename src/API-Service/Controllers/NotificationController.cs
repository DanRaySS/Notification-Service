
using Microsoft.AspNetCore.Mvc;
using API_Service.Application.ErrorTypes;
using API_Service.Application.Services;
using API_Service.DTOs;
using Prometheus;

namespace API_Service.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    public class NotificationController : ControllerBase
    {
        // Счетчики для всех типов запросов
        private static readonly Counter GetAllCounter = Metrics.CreateCounter("api_get_all_requests_total", "Total number of GET all requests.");
        private static readonly Counter GetByIdCounter = Metrics.CreateCounter("api_get_by_id_requests_total", "Total number of GET by ID requests.");
        private static readonly Counter PostCounter = Metrics.CreateCounter("api_post_requests_total", "Total number of POST requests.");
        private static readonly Counter PatchCounter = Metrics.CreateCounter("api_patch_requests_total", "Total number of PATCH requests.");

        // Гистограммы для времени выполнения операций
        private static readonly Histogram GetAllDuration = Metrics.CreateHistogram("api_get_all_duration_seconds", "Duration of GET all requests in seconds.");
        private static readonly Histogram GetByIdDuration = Metrics.CreateHistogram("api_get_by_id_duration_seconds", "Duration of GET by ID requests in seconds.");
        private static readonly Histogram PostDuration = Metrics.CreateHistogram("api_post_duration_seconds", "Duration of POST requests in seconds.");
        private static readonly Histogram PatchDuration = Metrics.CreateHistogram("api_patch_duration_seconds", "Duration of PATCH requests in seconds.");
        public ILogger<NotificationController> _logger { get; }
        private readonly NotificationService _service;

        public NotificationController(ILogger<NotificationController> logger, 
            NotificationService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmailNotification([FromQuery] string channelType, [FromBody]CreateNotificationDto command, CancellationToken cancellationToken)
        {
            PostCounter.Inc(); // Увеличиваем счетчик POST запросов
            using (PostDuration.NewTimer()) // Измеряем длительность выполнения
            {
                var result = await _service.SendNotification(command, channelType, cancellationToken);
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
        }

        [HttpPatch("updateById")]
        public async Task<IActionResult> UpdateNotificationById([FromBody]UpdateNotificationDto command, [FromQuery] Guid id, CancellationToken cancellationToken)
        {
            PatchCounter.Inc(); // Увеличиваем счетчик PATCH запросов
            using (PatchDuration.NewTimer()) // Измеряем длительность выполнения
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
        }

        [HttpGet("getById")]
        public async Task<IActionResult> GetNotificationById([FromQuery] Guid id, CancellationToken cancellationToken)
        {
            GetByIdCounter.Inc(); // Увеличиваем счетчик GET запросов по ID

            using (GetByIdDuration.NewTimer()) // Измеряем длительность выполнения
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
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllNotifications(CancellationToken cancellationToken)
        {
            GetAllCounter.Inc(); // Увеличиваем счетчик GET запросов

            using (GetAllDuration.NewTimer()) // Измеряем время выполнения
            {
                // Логика получения всех уведомлений
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
}
