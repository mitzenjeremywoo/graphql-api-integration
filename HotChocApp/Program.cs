
var builder = WebApplication.CreateBuilder(args);

var weatherForecastClient = "WeatherForecastClient";

// Register your http client factory to the server
builder.Services.AddHttpClient(weatherForecastClient, ctx => { ctx.BaseAddress = new Uri("http://localhost:5199"); });

// do manual instantiation or your can use AddHttpClient to do this. 
// using the code generated from NSWAG, i decided to keep this contained in case i re-generate the code in the future.
builder.Services.AddTransient(ctx =>
{
    var clientFactory = ctx.GetRequiredService<IHttpClientFactory>();
    var httpClient = clientFactory.CreateClient(weatherForecastClient);

    return new TodoReader.TodoService("http://localhost:5199", httpClient);
});

builder.Services.AddGraphQLServer()
    .AddQueryType<Query>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapGraphQL();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
