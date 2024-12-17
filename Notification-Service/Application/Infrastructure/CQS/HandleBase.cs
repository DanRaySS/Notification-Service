using MediatR;

namespace Notification_Service.Application.Infrastructure.CQS
{
    public abstract class HandleBase<T, TResult> : IRequestHandler<T, TResult> where T : IRequest<TResult>
        where TResult : class, Result.IResult
    {
        async Task<TResult> IRequestHandler<T, TResult>.Handle(T request, CancellationToken cancellationToken)
        {
            var res = await CoreHandle(request, cancellationToken);
            return (TResult) res;
        }

        public abstract Task<TResult> Handle(T request, CancellationToken cancellationToken);

        protected virtual async Task<Result.IResult> CoreHandle(T request, CancellationToken cancellationToken)
        {
            var canHandle = await CanHandle(request, cancellationToken);
            if (!canHandle.IsSuccessfull)
            {
                return canHandle;
            }

            return await Handle(request, cancellationToken);
        }

        protected virtual Task<Result.Result> CanHandle(T request, CancellationToken cancellationToken)
        {
            return Task.FromResult(Result.Result.Success());
        }
    }
}
