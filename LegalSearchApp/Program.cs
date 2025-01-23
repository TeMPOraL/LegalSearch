using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    //c.UseInlineDefinitionsForEnums();

});
builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.RequestProperties;
});

var app = builder.Build();

var documents = new List<LegalDocument> {
    new("1", "MIT License", "MIT License text goes here", "License"),
    new("2", "Apache License 2.0", "Apache License 2.0 text goes here", "License"),
    new("3", "GPL 3.0", "GPL 3.0 text goes here", "License"),
    new ("4", "BSD 3-Clause", "BSD 3-Clause text goes here", "License"),
    new ("5", "LGPL 3.0", "LGPL 3.0 text goes here", "License"),
    new ("6", "AGPL 3.0", "AGPL 3.0 text goes here", "License"),
    new ("7", "Mozilla Public License 2.0", "Mozilla Public License 2.0 text goes here", "License"),
    new ("8", "Eclipse Public License 2.0", "Eclipse Public License 2.0 text goes here", "License"),
    new ("9", "Unlicense", "Unlicense text goes here", "License"),
    new ("10", "Creative Commons Zero v1.0 Universal", "Creative Commons Zero v1.0 Universal text goes here", "License")
};

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpLogging();

#region API Routes

app.MapGet("/search", Results<BadRequest<string>, Ok<SearchResponse>> ([AsParameters] SearchQuery query) =>
{
    if (query.Strategy != SearchStrategy.FullText)
    {
        return TypedResults.BadRequest("Only FullText search is supported at the moment.");
    }

    // TODO: make it distinguish between results found in title vs. results found in contents, and add a field to SearchResult
    // that indicates the source of the chunk, and possibly make the chunk optional or fetched from either title or contents.
    return TypedResults.Ok(new SearchResponse(
        Strategy: SearchStrategy.FullText,
        Results:
            documents.Where((d) =>
            {
                return d.Content.Contains(query.Query, StringComparison.OrdinalIgnoreCase)
                || d.Title.Contains(query.Query, StringComparison.OrdinalIgnoreCase);
            }).Select((d) => new SearchResult(
                Id: d.Id,
                Title: d.Title,
                Chunk: d.Content.Substring(0, Math.Min(100, d.Content.Length)),
                Confidence: 1.0f
            )).ToArray()
    ));
}).WithSummary("Search for legal documents by a query string.").WithDescription("Currently only supports FullText search strategy.");

app.MapGet("/document/{id}", Results<NotFound<string>, FileContentHttpResult> (string id) =>
{
    var document = documents.FirstOrDefault((d) => d.Id == id);
    if (document == null)
    {
        return TypedResults.NotFound("No document with such ID found.");
    }
    return TypedResults.File(Encoding.UTF8.GetBytes(document.Content), "text/plain", $"document-{id}.txt");
}).WithSummary("Download the full legal document by its ID.");

app.MapPost("/upload", () =>
{
    return TypedResults.InternalServerError("Not implemented yet.");
}).WithSummary("Upload a new legal document.").WithDescription("Document will be subject to processing to enable search. This endpoint is not implemented yet.");

app.MapGet("/githublicenses", async () =>
{
    var licenses = await LegalDataLoader.LoadGithubLicenses();
    return TypedResults.Ok(licenses);
}).WithSummary("DEBUG -- Fetch popular licenses from GitHub API.");
#endregion

app.Run();

