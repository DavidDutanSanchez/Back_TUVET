namespace tu_vet_back.tuvet.Extensions
{
    public static class ListExtensions
    {
        /// <summary>
        /// Dynamically casts a list of objects to a specified type.
        /// </summary>
        /// <param name="list">The list of objects to cast.</param>
        /// <param name="targetType">The target type to cast to.</param>
        /// <returns>A list of the specified type.</returns>
        public static dynamic Cast(this IEnumerable<object> list, Type targetType)
        {
            // Use reflection to create a generic method for casting
            var castMethod = typeof(Enumerable)
                .GetMethod("Cast")
                .MakeGenericMethod(targetType);

            // Invoke the cast method and return the result as a list
            return castMethod.Invoke(null, new object[] { list });
        }
    }
}
