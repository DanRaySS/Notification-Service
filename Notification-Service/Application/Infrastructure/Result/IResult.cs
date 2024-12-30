using Microsoft.AspNetCore.Components.Web;

namespace Notification_Service.Application.Infrastructure.Result
{
    public interface IResult
    {
        bool IsSuccessfull { get; }
        IReadOnlyList<IError> GetErrors();
    }
}
