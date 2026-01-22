using JsonPatchTests.Common;
using Xunit.Abstractions;

namespace JsonPatchTests.Newtonsoft;

public class NewtonsoftTests(ITestOutputHelper output) : TestBase(output)
{
    protected override IJsonPatchDocumentFascade CreateJsonPatchDocument()
    {
        return new JsonPatchDocumentFascade();
    }
}
