namespace VerticalSlicer.WebApi.Contracts;

public record Response
{
    public string? Message { get; init; }

    public Response(string? message)
    {
        Message = message;
    }
}

public record Response<T> : Response
{
    public T Data { get; init; }

    public Response(T data, string? message = null) : base(message)
    {
        Data = data;
    }
}