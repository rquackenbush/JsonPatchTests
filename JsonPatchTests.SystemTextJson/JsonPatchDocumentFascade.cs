using JsonPatchTests.Common;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;

namespace JsonPatchTests.SystemTextJson;

public class JsonPatchDocumentFascade : IJsonPatchDocumentFascade
{
    private readonly JsonPatchDocument _patch = new JsonPatchDocument();

    public void Apply(object toPatch)
    {
        _patch.ApplyTo(toPatch);
    }

    public void Replace(string path, object? value)
    {
        _patch.Replace(path, value);
    }
}
