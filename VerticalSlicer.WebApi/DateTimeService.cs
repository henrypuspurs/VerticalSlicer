namespace VerticalSlicer.WebApi;

public class DateTimeService : IDateTimeService
{
    public DateTime Now => DateTime.UtcNow;
}

public interface IDateTimeService
{
    DateTime Now { get; }
}