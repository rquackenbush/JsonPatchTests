# JsonPatchTests

Experimenting with the different JsonPatchDocument implementations.

The three I'm looking at:

|Package|Test Project|
|---|---|
|`SystemTextJsonPatch`|`JsonPatchTests.Havunen`|
|`Microsoft.AspNetCore.JsonPatch`| `JsonPatchTests.Newtonsoft`|
|`Microsoft.AspNetCore.JsonPatch.SystemTextJson`| `JsonPatchTests.SystemTextJson`|

Are my project names perfect? Absolutely not. This is a throwaway project after all.

## Implementation

All of the heavy lifting is done in the [`JsonPatchTests.Common.TestBase`](./JsonPatchTests.Common/TestBase.cs) class.

### Notes

- This abstract class has a number of `[Fact]` tests built in. This means we can test multiple `JsonPatchDocument` implementations with very little duplication of code.
- [`IJsonPatchDocumentFascade`](./JsonPatchTests.Common/IJsonPatchDocumentFascade.cs) wraps a given `JsonPatchDocument` implementation.
- I separated the tests into separate projects because I didn't want any confusion over which implementation was being used. Using namespaces to differentiate between types absolutely *works*, but feels fragile.
