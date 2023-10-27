using UnityEngine;
using System.Reflection;

public static class ComponentCopy
{
    public static T CopyComponent<T>(T original, GameObject destination) where T : Component
    {
        System.Type type = original.GetType();
        T copy = destination.AddComponent(type) as T;

        // Copy fields
        FieldInfo[] fields = type.GetFields();
        foreach (FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }

        // Copy properties
        PropertyInfo[] properties = type.GetProperties();
        foreach (PropertyInfo property in properties)
        {
            if (property.CanWrite)
            {
                property.SetValue(copy, property.GetValue(original, null), null);
            }
        }
        return copy;
    }
}