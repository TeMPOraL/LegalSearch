var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
    {
        var date = DateOnly.FromDateTime(DateTime.Now.AddDays(index));
        var temperatureC = Random.Shared.Next(-20, 55);
        string summary = temperatureC switch
        {
            <= -10 => "Freezing",
            <= 0 => "Bracing",
            <= 10 => "Chilly",
            <= 15 => "Cool",
            <= 20 => "Mild",
            <= 25 => "Warm",
            <= 30 => "Balmy",
            <= 35 => "Hot",
            <= 40 => "Sweltering",
            _ => "Scorching"
        };

        return new WeatherForecast(date, temperatureC, summary);
    })
    .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
