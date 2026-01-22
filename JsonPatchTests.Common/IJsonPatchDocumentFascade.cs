namespace JsonPatchTests.Common;

public interface IJsonPatchDocumentFascade
{
    void Replace(string path, object? value);

    void Apply(object toPatch);
}
