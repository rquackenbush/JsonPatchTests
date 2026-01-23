using System.Dynamic;
using System.Reflection;

namespace JsonPatchTests.Common;

// Wraps a CLR object with DynamicObject. Has this been done before? Probably. Just seeing what our
// options are and how the various JsonPatchDocument implemntations interact with DynamicObjects.
public class ReflectionDynamicObject(object source) : DynamicObject
{
    public object Source => source;

    public override IEnumerable<string> GetDynamicMemberNames()
    {
        var propertyInfos = source.GetType().GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.SetProperty);

        return propertyInfos
            .Select(x => x.Name);
    }

    private static bool ShouldWrap(Type type)
    {
        if (type == typeof(string))
            return false;

        if (type.IsClass)
            return true;

        return false;
    }

    public override bool TryGetMember(GetMemberBinder binder, out object? result)
    {
        var propertyInfo = source.GetType().GetProperty(binder.Name);

        if (propertyInfo != null)
        {
            var value = propertyInfo.GetValue(source);

            if (value == null)
            {
                result = null;
            }
            else if (ShouldWrap(propertyInfo.PropertyType))
            {
                result = new ReflectionDynamicObject(value);
            }
            else
            {
                result = value;
            }

            return true;
        }

        return base.TryGetMember(binder, out result);
    }

    public override bool TrySetMember(SetMemberBinder binder, object? value)
    {
        var propertyInfo = source.GetType().GetProperty(binder.Name);

        if (propertyInfo != null)
        {
            propertyInfo.SetValue(source, value);
            return true;
        }

        return base.TrySetMember(binder, value);
    }
}