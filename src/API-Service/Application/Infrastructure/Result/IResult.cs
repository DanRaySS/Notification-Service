using Microsoft.AspNetCore.Components.Web;

namespace API_Service.Application.Infrastructure.Result
{
    public interface IResult
    {
        bool IsSuccessfull { get; }
        IReadOnlyList<IError> GetErrors();
    }
}
