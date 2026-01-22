using Shouldly;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace JsonPatchTests.Common;

public abstract class TestBase(ITestOutputHelper output)
{
    protected static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
    {
        WriteIndented = true
    };

    private Person CreatePerson()
    {
        return new Person
        {
            FirstName = "Kate",
            Metadata = new Metadata
            {
                Id = 42
            }
        };
    }

    protected abstract IJsonPatchDocumentFascade CreateJsonPatchDocument();

    private void Test<TPatchable>(Func<Person, TPatchable> convertToPatchable, Func<TPatchable, Person> convertFromPatchable)
        where TPatchable : notnull
    {
        var before = CreatePerson();

        var patchable = convertToPatchable(before);

        var patchDocument = CreateJsonPatchDocument();

        patchDocument.Replace("/FirstName", "Juliet");
        patchDocument.Replace("/Metadata/Id", 7);

        patchDocument.Apply(patchable);

        var after = convertFromPatchable(patchable);

        output.WriteLine("After patch:");
        output.WriteLine(JsonSerializer.Serialize(after, SerializerOptions));

        after.FirstName.ShouldBe("Juliet");
        after.Metadata.ShouldNotBeNull();
        after.Metadata.Id.ShouldBe(7);
    }

    [Fact]
    public void JsonDocumentTest()
    {
        Test<JsonDocument>(
            p =>JsonDocument.Parse(JsonSerializer.Serialize(p)),
            p => JsonSerializer.Deserialize<Person>(p)!
        );
    }

    [Fact]
    public void DynamicObjectTest()
    {
        Test<ReflectionDynamicObject>(
            p => new ReflectionDynamicObject(p),
            p => (Person)p.Source
        );
    }

    [Fact]
    public void JsonElementTest()
    {
        Test<JsonElement>(
            p => JsonSerializer.Deserialize<JsonElement>(JsonSerializer.Serialize(p)),
            p => JsonSerializer.Deserialize<Person>(p)!
        );
    }

    [Fact]
    public void PocoTest()
    {
        Test<Person>(
            p => p,
            p => p
        );
    }
}