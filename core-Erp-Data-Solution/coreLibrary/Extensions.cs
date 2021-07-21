using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace coreLogic
{
    public static class Extensions
    {
        public static void CopyTo(this object source, object target, string excludedProperties, BindingFlags memberAccess)
        {
            string[] excluded = null;
            if (!string.IsNullOrEmpty(excludedProperties))
                excluded = excludedProperties.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            MemberInfo[] miT = target.GetType().GetMembers(memberAccess);
            foreach (MemberInfo Field in miT)
            {
                string name = Field.Name;

                // Skip over any property exceptions
                if (!string.IsNullOrEmpty(excludedProperties) &&
                    excluded.Contains(name))
                    continue;

                if (Field.MemberType == MemberTypes.Field)
                {
                    FieldInfo SourceField = source.GetType().GetField(name);
                    if (SourceField == null)
                        continue;

                    object SourceValue = SourceField.GetValue(source);
                    ((FieldInfo)Field).SetValue(target, SourceValue);
                }
                else if (Field.MemberType == MemberTypes.Property)
                {
                    PropertyInfo piTarget = Field as PropertyInfo;
                    PropertyInfo SourceField = source.GetType().GetProperty(name, memberAccess);
                    if (SourceField == null)
                        continue;

                    if (piTarget.CanWrite && SourceField.CanRead)
                    {
                        object SourceValue = SourceField.GetValue(source, null);

                        if (GenericClassifier.IsICollection(SourceValue.GetType()))
                            continue;

                        if (GenericClassifier.IsIEnumerable(SourceValue.GetType()))
                            continue;

                        if (!(SourceValue is System.String
                            || SourceValue is System.Double
                            || SourceValue is System.Boolean
                            || SourceValue is System.Int16
                            || SourceValue is System.Int32
                            || SourceValue is System.Int64
                            || SourceValue is System.Decimal
                            || SourceValue is System.Byte
                            || SourceValue is System.DateTime
                            || SourceValue is System.Array))
                            continue;

                        piTarget.SetValue(target, SourceValue, null);
                    }
                }
            }

        }

        public static class GenericClassifier
        {
            public static bool IsICollection(Type type)
            {
                return Array.Exists(type.GetInterfaces(), IsGenericCollectionType);
            }
            public static bool IsIEnumerable(Type type)
            {
                return Array.Exists(type.GetInterfaces(), IsGenericEnumerableType);
            }
            static bool IsGenericCollectionType(Type type)
            {
                return type.IsGenericType && (typeof(ICollection<>) == type.GetGenericTypeDefinition());
            }
            static bool IsGenericEnumerableType(Type type)
            {
                return type.IsGenericType && (typeof(IEnumerable<>) == type.GetGenericTypeDefinition());
            }
        }
    }
}
