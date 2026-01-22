using JsonPatchTests.Common;
using Xunit.Abstractions;

namespace JsonPatchTests.SystemTextJson;

public class SystemTextJsonTests(ITestOutputHelper output) : TestBase(output)
{
    protected override IJsonPatchDocumentFascade CreateJsonPatchDocument()
    {
        return new JsonPatchDocumentFascade();
    }
}
