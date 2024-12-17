using MediatR;

namespace Notification_Service.Application.Infrastructure.CQS
{
    public abstract class Query<TResult>: IQuery<Result.Result<TResult>>, IRequest<Result.Result<TResult>>
    {

    }
}
