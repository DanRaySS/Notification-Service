namespace Notification_Service.Application.Infrastructure.CQS
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        Task<Result.Result> Handle(TCommand command, CancellationToken cancellationToken);
    }
}
