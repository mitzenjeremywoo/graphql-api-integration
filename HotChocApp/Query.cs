using HotChocolate;
using TodoReader;

public class Query
{
    public async Task<ICollection<TodoReader.WeatherForecast>> GetTodosAsync(
        [Service]TodoService service,
        CancellationToken cancellationToken)
    {
        return await service.GetWeatherForecastAsync(cancellationToken);
    }   
}