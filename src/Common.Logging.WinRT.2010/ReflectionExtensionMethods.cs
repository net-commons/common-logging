using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace Common.Logging
{
    /// <summary>
    /// Extension Methods wrapping the WinRT Reflection API in methods matching the full BCL System.Reflection API
    ///     to minimize changes to calling code that relies upon reflection
    /// </summary>
    public static class ReflectionExtensionMethods
    {

        /// <summary>
        /// Gets the constructor.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="constructorParams">The constructor parameters.</param>
        /// <returns>ConstructorInfo.</returns>
        public static ConstructorInfo GetConstructor(this Type type, Type[] constructorParams)
        {
            var constructors = type.GetTypeInfo().DeclaredConstructors;
            return FindMatchBasedOnParameterSignature<ConstructorInfo>(constructors, constructorParams);
        }

        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>PropertyInfo.</returns>
        public static PropertyInfo GetProperty(this Type type, String propertyName)
        {
            return type.GetTypeInfo().GetDeclaredProperty(propertyName);
        }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>IEnumerable&lt;PropertyInfo&gt;.</returns>
        public static IEnumerable<PropertyInfo> GetProperties(this Type type)
        {
            return type.GetTypeInfo().DeclaredProperties;
        }

        /// <summary>
        /// Gets the method.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <returns>MethodInfo.</returns>
        public static MethodInfo GetMethod(this Type type, String methodName)
        {
            return type.GetTypeInfo().GetDeclaredMethod(methodName);
        }

        /// <summary>
        /// Gets the method.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="methodParams">The method parameters.</param>
        /// <returns>MethodInfo.</returns>
        public static MethodInfo GetMethod(this Type type, String methodName, Type[] methodParams)
        {
            var methods = type.GetTypeInfo().DeclaredMethods;
            return FindMatchBasedOnParameterSignature<MethodInfo>(methods, methodParams);
        }

        private static TReturn FindMatchBasedOnParameterSignature<TReturn>(IEnumerable<MethodBase> methods, Type[] methodParams) where TReturn : MethodBase
        {
            foreach (var ctor in methods)
            {
                var parameters = ctor.GetParameters();

                //if no match in param count, can't be the one we want!
                if (methodParams.Length != parameters.Length)
                    break;

                //if all the types don't match, not the one we're seeking
                for (int i = 0; i < methodParams.Length; i++)
                {
                    if (methodParams[i] != parameters[i].ParameterType)
                        break;
                }

                return ctor as TReturn;
            }

            //if we get this far, no match found so return NULL
            // (calling code resp. for null-check)
            return null;
        }

        /// <summary>
        /// Determines whether [is subclass of] [the specified parent type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="parentType">Type of the parent.</param>
        /// <returns><c>true</c> if [is subclass of] [the specified parent type]; otherwise, <c>false</c>.</returns>
        public static bool IsSubclassOf(this Type type, Type parentType)
        {
            return type.GetTypeInfo().IsSubclassOf(parentType);
        }

        /// <summary>
        /// Determines whether [is assignable from] [the specified parent type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="parentType">Type of the parent.</param>
        /// <returns><c>true</c> if [is assignable from] [the specified parent type]; otherwise, <c>false</c>.</returns>
        public static bool IsAssignableFrom(this Type type, Type parentType)
        {
            return type.GetTypeInfo().IsAssignableFrom(parentType.GetTypeInfo());
        }

        /// <summary>
        /// Determines whether the specified type is enum.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if the specified type is enum; otherwise, <c>false</c>.</returns>
        public static bool IsEnum(this Type type)
        {
            return type.GetTypeInfo().IsEnum;
        }

        /// <summary>
        /// Determines whether the specified type is primitive.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if the specified type is primitive; otherwise, <c>false</c>.</returns>
        public static bool IsPrimitive(this Type type)
        {
            return type.GetTypeInfo().IsPrimitive;
        }

        /// <summary>
        /// Gets the type of the base.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Type.</returns>
        public static Type GetBaseType(this Type type)
        {
            return type.GetTypeInfo().BaseType;
        }

        /// <summary>
        /// Determines whether [is generic type] [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if [is generic type] [the specified type]; otherwise, <c>false</c>.</returns>
        public static bool IsGenericType(this Type type)
        {
            return type.GetTypeInfo().IsGenericType;
        }

        /// <summary>
        /// Gets the generic arguments.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Type[].</returns>
        public static Type[] GetGenericArguments(this Type type)
        {
            return type.GetTypeInfo().GenericTypeArguments;
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="propertyValue">The property value.</param>
        /// <returns>System.Object.</returns>
        public static object GetPropertyValue(this Object instance, string propertyValue)
        {
            try
            {
                PropertyInfo pi = instance.GetType().GetTypeInfo().GetDeclaredProperty(propertyValue);
                return pi?.GetValue(instance);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the type information.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>System.Reflection.TypeInfo.</returns>
        public static TypeInfo GetTypeInfo(this Type type)
        {
            IReflectableType reflectableType = (IReflectableType)type;
            return reflectableType.GetTypeInfo();
        }
    }
}
