using Notification_Service.Application.Infrastructure.Result;

namespace Notification_Service.Application.Infrastructure.CQS
{
    public abstract class QueryHandler<TQuery, TResult> : HandleBase<TQuery, Result.Result<TResult>>, 
        IQueryHandler<TQuery, Result.Result<TResult>> where TQuery : Query<TResult>
    {
        protected Result.Result Success() => Result.Result.Success();
        protected Result<TResult> Success(TResult result) => Result.Result<TResult>.Success(result);
        protected Result<TResult> Error(IError error) => Result.Result<TResult>.Error(error);
    }
}
