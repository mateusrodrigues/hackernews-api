# HackerNews API

## How to Run

This project uses the following frameworks:

- API
    - .NET Core 3.1
    - ASP.NET Core 3.1
    - Newtonsoft.Json 12.0.3
    - Swashbuckle.AspNetCore 5.0.0-rc5
- Unit Tests
    - .NET Core 3.1
    - xUnit 2.4.0
    - Moq 4.13.1

To run the project, inside of the solutions's root folder, run:

```
dotnet run --project HackerNews.Api/HackerNews.Api.csproj
```

And point the browser to

```
https://localhost:5001/swagger
```

The endpoint can be hit by clicking on the `GET /api/Stories` Swagger menu item, then `Try it out` and `Execute`.

To run the unit tests, go to the solution's root folder and run:

```
dotnet test HackerNews.UnitTests/HackerNews.UnitTests.csproj
```

## Assumptions

- Supposing we're always dealing with stories (not job, comment, or pollopt) as `type`, I'm using `descendents` as the comment count instead of counting the comment IDs in `kids` for performance reasons.
- The `TimestampToDateTimeOffsetConverter` only implements the `ReadJson` method because it is assumed in this assignment we won't be serializing anything to JSON, rather just deserializing.
- The memory cache implemented keeps stories cached for **6 hours**. I'm using absolute expiration rather than sliding to allow for possible edits to the story appear in case that happens.

## Future Enhancements

- Replace the ASP.NET Core `IMemoryCache` with a more robust solution, such as Redis, to allow for multiple instances of the API to share the same cache memory.
- For the `GetStoryDetail_StoryIsInCache_ReadFromCacheAndReturnStory` and `GetStoryDetail_StoryIsNotInCache_SaveToCacheAndReturnStoryFromApi` tests, engineer a better way for object equality instead of asserting the equality of every property.
- The `CacheService` should be tested with an integration test to allow for better testing with the memory cache external dependency.
- In `StoriesController`, find a better way to call `GetStoryDetails()` without having to do `.Result`.
- Remove the Newtonsoft.Json annotations and dependency from the HackerNews.Domain project for better separation of concerns.