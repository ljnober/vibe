// AvaloniaAsyncDrawing/Models/Rect.cs
using SkiaSharp;
using System;
using AvaloniaAsyncDrawing.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AvaloniaAsyncDrawing.Models
{
    /// <summary>
    /// 类型安全的矩形数据结构，实现 ICanvasElement、IRenderable 接口。
    /// </summary>
    public class Rect : ICanvasElement, IRenderable, ICloneable, IHitTestable
    {
        /// <summary>
        /// 元素唯一标识。
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// 是否可见。
        /// </summary>
        public bool Visible { get; set; } = true;

        /// <summary>
        /// 左上角坐标。
        /// </summary>
        public SKPoint TopLeft { get; set; }

        /// <summary>
        /// 宽度。
        /// </summary>
        public float Width { get; set; }

        /// <summary>
        /// 高度。
        /// </summary>
        public float Height { get; set; }

        /// <summary>
        /// 填充颜色。
        /// </summary>
        [System.Text.Json.Serialization.JsonConverter(typeof(SKColorJsonConverter))]
        public SKColor FillColor { get; set; } = SKColors.Transparent;

        /// <summary>
        /// 描边颜色。
        /// </summary>
        [System.Text.Json.Serialization.JsonConverter(typeof(SKColorJsonConverter))]
        public SKColor StrokeColor { get; set; } = SKColors.Black;

        /// <summary>
        /// 描边粗细。
        /// </summary>
        public float StrokeWidth { get; set; } = 1f;

        /// <summary>
        /// 变换矩阵。
        /// </summary>
        public SKMatrix Transform { get; set; } = SKMatrix.CreateIdentity();

        /// <summary>
        /// 渲染方法。
        /// </summary>
        public void Render(SKCanvas canvas)
        {
            if (!Visible) return;
            canvas.Save();
            canvas.SetMatrix(Transform);

            var rect = new SKRect(TopLeft.X, TopLeft.Y, TopLeft.X + Width, TopLeft.Y + Height);

            if (FillColor.Alpha > 0)
            {
                using var fillPaint = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    Color = FillColor,
                    IsAntialias = true
                };
                canvas.DrawRect(rect, fillPaint);
            }

            if (StrokeWidth > 0 && StrokeColor.Alpha > 0)
            {
                using var strokePaint = new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    Color = StrokeColor,
                    StrokeWidth = StrokeWidth,
                    IsAntialias = true
                };
                canvas.DrawRect(rect, strokePaint);
            }

            canvas.Restore();
        }

        /// <summary>
        /// 命中测试。
        /// </summary>
        public bool HitTest(SKPoint point)
        {
            if (!Visible) return false;
            // 逆变换点
            SKMatrix invert;
            if (!Transform.TryInvert(out invert))
                return false;
            var local = invert.MapPoint(point);
            var rect = new SKRect(TopLeft.X, TopLeft.Y, TopLeft.X + Width, TopLeft.Y + Height);
            return rect.Contains(local.X, local.Y);
        }

        /// <summary>
        /// 深拷贝。
        /// </summary>
        public object Clone()
        {
            return new Rect
            {
                Id = Guid.NewGuid().ToString(),
                Visible = this.Visible,
                TopLeft = this.TopLeft,
                Width = this.Width,
                Height = this.Height,
                FillColor = this.FillColor,
                StrokeColor = this.StrokeColor,
                StrokeWidth = this.StrokeWidth,
                Transform = this.Transform
            };
        }

        /// <summary>
        /// 序列化为 JSON 字符串。
        /// </summary>
        public string Serialize()
        {
            var json = JsonSerializer.Serialize(this);
            System.Diagnostics.Debug.WriteLine($"[Rect.Serialize] Id:{Id} Json:{json}");
            return json;
        }

        /// <summary>
        /// 从 JSON 反序列化。
        /// </summary>
        public static Rect? Deserialize(string json)
        {
            var rect = JsonSerializer.Deserialize<Rect>(json);
            System.Diagnostics.Debug.WriteLine($"[Rect.Deserialize] Json:{json} ResultId:{rect?.Id}");
            return rect;
        }
    }
}