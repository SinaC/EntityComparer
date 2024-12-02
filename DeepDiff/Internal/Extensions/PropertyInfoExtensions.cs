using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DeepDiff.Internal.Extensions
{
    internal static class PropertyInfoExtensions
    {
        public static bool IsSameAs(this PropertyInfo propertyInfo, PropertyInfo otherPropertyInfo)
        {
            return propertyInfo == otherPropertyInfo ||
                   propertyInfo.Name == otherPropertyInfo.Name
                    && (propertyInfo.DeclaringType == otherPropertyInfo.DeclaringType
                        || propertyInfo.DeclaringType.IsSubclassOf(otherPropertyInfo.DeclaringType)
                        || otherPropertyInfo.DeclaringType.IsSubclassOf(propertyInfo.DeclaringType)
                        || propertyInfo.DeclaringType.GetInterfaces().Contains(otherPropertyInfo.DeclaringType)
                        || otherPropertyInfo.DeclaringType.GetInterfaces().Contains(propertyInfo.DeclaringType));
        }

        public static void CopyPropertyValues(this IEnumerable<PropertyInfo> properties, object existingEntity, object newEntity)
        {
            if (properties == null)
                return;
            foreach (var propertyInfo in properties)
            {
                var newValue = propertyInfo.GetValue(newEntity);
                propertyInfo.SetValue(existingEntity, newValue);
            }
        }

        public static bool IsEnumerable(this PropertyInfo propertyInfo)
        {
            if (propertyInfo.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(propertyInfo.PropertyType))
                return true;
            return false;
        }
    }
}