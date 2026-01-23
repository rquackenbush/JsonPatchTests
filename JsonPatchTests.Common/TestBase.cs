using Shouldly;
using System.Text.Json;
using System.Text.Json.Nodes;
using Xunit;
using Xunit.Abstractions;

namespace JsonPatchTests.Common;

public abstract class TestBase(ITestOutputHelper output)
{
    protected static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
    {
        WriteIndented = true
    };

    protected ITestOutputHelper Output => output;

    protected abstract IJsonPatchDocumentFascade CreateJsonPatchDocument();

    [Fact]
    public void JsonNode_AddPrimitiveProperty()
    {
        var node = JsonNode.Parse("{}");

        node.ShouldNotBeNull();

        var patchDocument = CreateJsonPatchDocument();

        patchDocument.Add("/Add", "added");

        patchDocument.Apply(node);

        Output.WriteLine(node.ToString());
    }

    [Fact]
    public void JsonNode_AddNewArrayProperty()
    {
        var node = JsonNode.Parse("{}");

        node.ShouldNotBeNull();

        var patchDocument = CreateJsonPatchDocument();

        patchDocument.Add("/NewArray", new string[] { "One", "Two" });

        patchDocument.Apply(node);

        Output.WriteLine(node.ToString());
    }

    //[Fact]
    //public void JsonNode_InsertArrayMember()
    //{
    //    var source = new
    //    {
    //        MyArray = new string[]
    //        {
    //            "One",
    //            "Two"
    //        }
    //    };

    //    var node = JsonNode.Parse(JsonSerializer.Serialize(source));

    //    node.ShouldNotBeNull();

    //    var patchDocument = CreateJsonPatchDocument();

    //    patchDocument.Add("/MyArray/", new string[] { });
    //    patchDocument.Add("/MyArray/-", "First");

    //    patchDocument.Apply(node);

    //    Output.WriteLine(node.ToString());
    //}

    [Fact]
    public void JsonNode_InsertArrayOnNewMember()
    {
        var source = new
        {
        };

        var node = JsonNode.Parse(JsonSerializer.Serialize(source));

        node.ShouldNotBeNull();

        var patchDocument = CreateJsonPatchDocument();

        patchDocument.Add("/MyArray/-", "One");

        patchDocument.Apply(node);

        Output.WriteLine(node.ToString());
    }

    [Fact]
    public void JsonNode_RemoveExistingProperty()
    {
        var source = new
        {
            Foo = "bar"
        };

        var node = JsonNode.Parse(JsonSerializer.Serialize(source));

        node.ShouldNotBeNull();

        var patchDocument = CreateJsonPatchDocument();

        patchDocument.Remove("Foo");

        patchDocument.Apply(node);

    }

    [Fact]
    public void JsonNode_AddExistingProperty()
    {
        var source = new
        {
            Foo = "bar"
        };

        var node = JsonNode.Parse(JsonSerializer.Serialize(source));

        node.ShouldNotBeNull();

        var patchDocument = CreateJsonPatchDocument();

        patchDocument.Add("Foo", "asdfasfsadf");

        patchDocument.Apply(node);

        Output.WriteLine(node.ToString());
    }

    [Fact]
    public void JsonNode_ReplaceExistingProperty()
    {
        var source = new
        {
            Foo = "bar"
        };

        var node = JsonNode.Parse(JsonSerializer.Serialize(source));

        node.ShouldNotBeNull();

        var patchDocument = CreateJsonPatchDocument();

        patchDocument.Replace("Foo", "asdfasfsadf");

        patchDocument.Apply(node);

        Output.WriteLine(node.ToString());
    }

    private void Test<TPatchable>(Func<Person, TPatchable> convertToPatchable, Func<TPatchable, Person> convertFromPatchable)
        where TPatchable : notnull
    {
        var before = new Person
        {
            FirstName = "Kate",
            Metadata = new Metadata
            {
                Id = 42
            }
        };

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
    public void Person_JsonDocumentTest()
    {
        Test<JsonDocument>(
            p => JsonDocument.Parse(JsonSerializer.Serialize(p)),
            p => JsonSerializer.Deserialize<Person>(p)!
        );
    }

    [Fact]
    public void Person_DynamicObjectTest()
    {
        Test<ReflectionDynamicObject>(
            p => new ReflectionDynamicObject(p),
            p => (Person)p.Source
        );
    }

    [Fact]
    public void Person_JsonElementTest()
    {
        Test<JsonElement>(
            p => JsonSerializer.Deserialize<JsonElement>(JsonSerializer.Serialize(p)),
            p => JsonSerializer.Deserialize<Person>(p)!
        );
    }

    [Fact]
    public void Person_JsonNodeTest()
    {
        Test<JsonNode>(
            p => JsonNode.Parse(JsonSerializer.Serialize(p))!,
            p => JsonSerializer.Deserialize<Person>(p)!
        );
    }

    [Fact]
    public void Person_PocoTest()
    {
        Test<Person>(
            p => p,
            p => p
        );
    }
}