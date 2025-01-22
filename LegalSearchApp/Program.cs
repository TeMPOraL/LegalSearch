var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    //c.UseInlineDefinitionsForEnums();

});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region API Routes
app.MapGet("/search", ([AsParameters] SearchQuery query) =>
{
    //TODO
    return new SearchResponse(
        Strategy: SearchStrategy.FullText,
        Results: [
            new SearchResult("1", "Legal stuff", "Legal stuff goes here", 1.0f),
                new SearchResult("2", "Legal stuff 2", "Legal stuff goes here", 0.5f)
        ]
    );
});

app.MapGet("/document/{id}", (string id) =>
{
    //TODO
    return "I'll figure out how to make this send data as a download later.";
});

app.MapPost("/upload", () =>
{
    //TODO
});
#endregion

app.Run();

