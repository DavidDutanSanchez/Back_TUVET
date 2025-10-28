namespace tu_vet_back.tuvet.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsNumericType(this Type type)
        {
            var underlyingType = Nullable.GetUnderlyingType(type) ?? type;
            return underlyingType == typeof(byte) || underlyingType == typeof(sbyte) ||
                underlyingType == typeof(short) || underlyingType == typeof(ushort) ||
                underlyingType == typeof(int) || underlyingType == typeof(uint) ||
                underlyingType == typeof(long) || underlyingType == typeof(ulong) ||
                underlyingType == typeof(float) || underlyingType == typeof(double) ||
                underlyingType == typeof(decimal);
        }
    }
}
