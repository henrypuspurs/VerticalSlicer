using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using VerticalSlicer.WebApi.Contracts;

namespace VerticalSlicer.WebApi.Features;

public partial class WeatherForecastController
{
    /// <summary>
    /// The default weather endpoint
    /// </summary>
    /// <returns>Some weather</returns>
    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<ActionResult<Response<IEnumerable<WeatherForecast.Model>>>> Get(
        [FromQuery] WeatherForecast.Request request,
        [FromServices] WeatherForecast.Handler handler,
        CancellationToken cancellationToken)
    {
        var response = await handler.Handle(request, cancellationToken);
        return Ok(response);
    }
}

public abstract class WeatherForecast
{
    public record Request
    {
        /// <summary>
        /// This is the bool that will be shown
        /// </summary>
        [FromQuery]
        public bool? ShowBool { get; init; }
    }
    
    public class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator()
        {
            RuleFor(x => x.ShowBool)
                .NotNull()
                .WithMessage("ShowBool is required to be true or false");
        }
    }

    public sealed class Handler : IRequestHandler<Request, IEnumerable<Model>>
    {
        private readonly IDateTimeService _dateTimeService;
        private readonly ILogger<Handler> _logger;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public Handler(
            IDateTimeService dateTimeService,
            ILogger<Handler> logger)
        {
            _dateTimeService = dateTimeService;
            _logger = logger;
        }

        public async Task<Response<IEnumerable<Model>>> Handle(
            Request request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling request: {Request}", request);

            var result = Enumerable.Range(1, 5).Select(index => new Model
            {
                Date = DateOnly.FromDateTime(_dateTimeService.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                ShowBool = request.ShowBool!.Value
                    ? "true"
                    : "false"
            });

            return new(result);
        }
    }

    /// <summary>
    /// The model returned from the weather endpoint
    /// </summary>
    public record Model
    {
        public DateOnly Date { get; init; }
        public int TemperatureC { get; init; }
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
        public string? Summary { get; init; }
        public string? ShowBool { get; init; }
    }
}