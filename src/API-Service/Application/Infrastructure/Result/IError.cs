namespace API_Service.Application.Infrastructure.Result
{
    public interface IError
    {
        string Type { get; }
        Dictionary<string, object> Data { get; }
    }
}
