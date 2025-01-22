# Basic idea of what this app does:
- Ingests some legal documents from various sources (e.g. PDFs on hard drive, GitHub licenses via API)
- Runs some basic chunking and embedding to enable semantic search
- Provides an API that takes a query, runs a search and returns matches, ideally with best chunks

Additional goals here include:
- Using vector database for storage and retrieval
- Putting it on Azure and making use of all the magic Azure services for building RAG pipeline

# Implementation plan
## Phase 0 -- DONE
- Drop all the nonsense from default template
- Add API endpoint stubs and associated data structures

## Phase 0.5 -- DONE
- Put it up on GitHub
- Set up basic GitHub actions to build/test it if it makes sense

## Phase 1
- Eat up GitHub licenses and keep them in memory
- Implement API for regular text search -- DONE

## Phase 2
- Get it hosted on Azure in simplest possible form

## Phase 3
- Build the embedding pipeline
- Use some vector database on e.g. Azure

# API design
We'll need a simple search endpoint, say:
- In:
    - query string
    - search strategy to use (in case we support more than one)
- Out: list of results, where each result contains:
    - ID
    - Document title
    - Matching chunk - text, maybe location
    - Whatever confidence value we can use to sort on

Another endpoint to retrieve full text of the thing:
- In: ID
- Out: Document text for download

We also might want to have an endpoint for uploading files, which will be processed (chunked, embedded) and stored for further retrieval.
- In: file upload
- Out: OK if it got added and processed, error otherwise