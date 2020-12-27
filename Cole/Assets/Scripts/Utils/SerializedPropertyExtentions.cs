#if UNITY_EDITOR
using UnityEditor;
namespace ProjectFukalite.Utils
{
    public static class SerializedPropertyExtentions
    {
        public static T GetValue<T>(this SerializedProperty property)
        {
            return ReflectionUtil.GetNestedObject<T>(property.serializedObject.targetObject, property.propertyPath);
        }
    }
}
#endif