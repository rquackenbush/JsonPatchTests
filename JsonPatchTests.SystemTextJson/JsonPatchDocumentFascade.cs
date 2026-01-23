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

    public void Add(string path, object? value)
    {
        _patch.Add(path, value);
    }

    public void Replace(string path, object? value)
    {
        _patch.Replace(path, value);
    }

    public void Move(string path, string pathNew)
    {
        _patch.Move(path, pathNew);
    }

    public void Remove(string path)
    {
        _patch.Remove(path);
    }

    public void Copy(string from, string path)
    {
        _patch.Copy(from, path);
    }
}
