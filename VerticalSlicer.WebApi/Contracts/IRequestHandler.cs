namespace VerticalSlicer.WebApi.Contracts;

public interface IRequestHandler<TRequest, TResponse>
{
    Task<Response<TResponse>> Handle(TRequest request, CancellationToken cancellationToken);
}