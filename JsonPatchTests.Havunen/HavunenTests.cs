using Xunit.Abstractions;
using JsonPatchTests.Common;

namespace JsonPatchTests.Havunen;

public class HavunenTests(ITestOutputHelper output) : TestBase(output)
{
    protected override IJsonPatchDocumentFascade CreateJsonPatchDocument()
    {
        return new JsonPatchDocumentFascade();
    }
}