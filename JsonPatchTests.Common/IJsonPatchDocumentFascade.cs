namespace JsonPatchTests.Common;

public interface IJsonPatchDocumentFascade
{
    void Replace(string path, object? value);

    void Add(string path, object? value);

    void Move(string path, string pathNew);

    void Remove(string path);

    void Copy(string from, string path);

    void Apply(object toPatch);
}
