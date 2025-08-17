// AvaloniaAsyncDrawing/Models/SKColorJsonConverter.cs
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using SkiaSharp;

namespace AvaloniaAsyncDrawing.Models
{
    /// <summary>
    /// SKColor <-> #AARRGGBB 字符串互转的 JsonConverter。
    /// </summary>
    public class SKColorJsonConverter : JsonConverter<SKColor>
    {
        public override SKColor Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var hex = reader.GetString();
            if (string.IsNullOrWhiteSpace(hex))
                return SKColors.Transparent;
            if (hex.StartsWith("#"))
                hex = hex.Substring(1);
            if (hex.Length == 8)
            {
                var a = Convert.ToByte(hex.Substring(0, 2), 16);
                var r = Convert.ToByte(hex.Substring(2, 2), 16);
                var g = Convert.ToByte(hex.Substring(4, 2), 16);
                var b = Convert.ToByte(hex.Substring(6, 2), 16);
                return new SKColor(r, g, b, a);
            }
            return SKColors.Transparent;
        }

        public override void Write(Utf8JsonWriter writer, SKColor value, JsonSerializerOptions options)
        {
            var hex = $"#{value.Alpha:X2}{value.Red:X2}{value.Green:X2}{value.Blue:X2}";
            writer.WriteStringValue(hex);
        }
    }
}