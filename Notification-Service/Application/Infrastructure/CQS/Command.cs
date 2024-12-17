using MediatR;

namespace Notification_Service.Application.Infrastructure.CQS
{
    public abstract class Command : IRequest<Result.Result>, ICommand
    {

    }
}
