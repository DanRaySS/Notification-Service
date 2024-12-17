using Notification_Service.Application.Infrastructure.Result;

namespace Notification_Service.Application.Infrastructure.CQS
{
    public abstract class CommandHandler<TCommand> : HandleBase<TCommand, Result.Result>, ICommandHandler<TCommand>
        where TCommand : Command
    {
        protected Result.Result Success() => Result.Result.Success();
        protected Result.Result Error(IError error) => Result.Result.Error(error);
    }
}
