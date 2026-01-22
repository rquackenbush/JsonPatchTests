using JsonPatchTests.Common;
using SystemTextJsonPatch;

namespace JsonPatchTests.Havunen;

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
