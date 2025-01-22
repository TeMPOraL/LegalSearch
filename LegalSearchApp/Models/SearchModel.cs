//TODO:
// - Introduce a different type for Id, or use UUIDs

// TODO: For API, get SearchStrategy to be passable as string.
// For future support of more strategies.
using System.Runtime.Serialization;

public enum SearchStrategy
{
    /// <summary>
    /// Direct string search on contents.
    /// </summary>
    // XXX this EnumMember doesn't lead to the endpoint definition accepting strings for this enum.
    // NOTE: this might be not an endpoint issue, but a Swagger generator issue.
    [EnumMember(Value = "FullText")]
    FullText
}

record SearchQuery(string Query, SearchStrategy Strategy);

record SearchResult(string Id, string Title, string Chunk, float Confidence);

record SearchResponse(SearchStrategy Strategy, SearchResult[] Results);
