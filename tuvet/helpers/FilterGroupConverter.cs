using System.Text.Json;
using System.Text.Json.Serialization;

namespace tu_vet_back.tuvet.Helpers
{
    public class ValueConverter : JsonConverter<object>
    {
        public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Number:
                    return reader.TryGetInt64(out long l) ? l : reader.GetDouble();

                case JsonTokenType.String:
                    return reader.GetString();

                case JsonTokenType.True:
                    return true;

                case JsonTokenType.False:
                    return false;

                case JsonTokenType.StartObject:
                    return JsonSerializer.Deserialize<object>(ref reader, options);

                case JsonTokenType.StartArray:
                    var list = new List<object?>();
                    while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                    {
                        list.Add(Read(ref reader, typeToConvert, options)); // Recursive call for array elements
                    }
                    return list;

                case JsonTokenType.Null:
                    return null;

                default:
                    throw new JsonException($"Unsupported token type: {reader.TokenType}");
            }
        }
        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
