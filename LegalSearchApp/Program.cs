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
        // Simplify this code so it maps the known minValue, maxValue range to indices in the summaries array. AI!
        string summary;
        if (temperatureC <= -10)
            summary = "Freezing";
        else if (temperatureC <= 0)
            summary = "Bracing";
        else if (temperatureC <= 10)
            summary = "Chilly";
        else if (temperatureC <= 15)
            summary = "Cool";
        else if (temperatureC <= 20)
            summary = "Mild";
        else if (temperatureC <= 25)
            summary = "Warm";
        else if (temperatureC <= 30)
            summary = "Balmy";
        else if (temperatureC <= 35)
            summary = "Hot";
        else if (temperatureC <= 40)
            summary = "Sweltering";
        else
            summary = "Scorching";

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
