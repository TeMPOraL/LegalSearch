# Legal Search

**THIS IS A TOY PROJECT** that I'm doing only to remind myself how to write C# and put stuff up on Azure.

A search API for legal documents with support for full-text and (planned) semantic search capabilities.

## Current Status

Phase 1 - Basic implementation with in-memory storage and full-text search.

## API Endpoints

### GET /search
Search through stored documents.
- Query Parameters:
  - `query`: Text to search for
  - `strategy`: Search strategy to use (currently supports: "fulltext")
- Returns: Array of matches with document chunks and confidence scores

### GET /document/{id}
Retrieve full document text.
- Parameters:
  - `id`: Document identifier
- Returns: Document text as downloadable content

### POST /upload
Upload new documents for processing.
- Body: File upload
- Returns: Success/failure status

## Local Development

1. Clone the repository
2. Open in Visual Studio or VS Code
3. Run with `dotnet run` from the LegalSearchApp directory

## Planned Features

- Azure cloud hosting
- Vector database integration
- Semantic search capabilities
- RAG (Retrieval Augmented Generation) pipeline

See [design.md](design.md) for detailed implementation plan.