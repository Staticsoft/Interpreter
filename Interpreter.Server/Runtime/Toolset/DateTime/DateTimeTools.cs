using Staticsoft.Flow;

namespace Staticsoft.Interpreter.Server;

public class DateTimeTools(
    Operation<CurrentDateTimeOperation.Input, CurrentDateTimeOperation.Output> getCurrentDateTime
)
{
    readonly Operation<CurrentDateTimeOperation.Input, CurrentDateTimeOperation.Output> GetCurrentDateTime = getCurrentDateTime;

    public async Task<DateTimeAccessor> Now()
    {
        var dateTime = await GetCurrentDateTime.Execute(new());

        return new(dateTime.Now);
    }

    public class DateTimeAccessor(
        DateTime dateTime
    )
    {
        readonly DateTime DateTime = dateTime;

        public DateTime Now
            => DateTime;

        public DateTime UtcNow
            => DateTime;
    }
}