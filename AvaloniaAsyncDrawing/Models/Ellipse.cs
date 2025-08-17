// AvaloniaAsyncDrawing/Models/Ellipse.cs
using SkiaSharp;
using System;
using System.Text.Json;
using AvaloniaAsyncDrawing.Models;

namespace AvaloniaAsyncDrawing.Models
{
    /// <summary>
    /// 类型安全的椭圆数据结构，实现 ICanvasElement、IRenderable、ICloneable。
    /// </summary>
    public class Ellipse : ICanvasElement, IRenderable, ICloneable, IHitTestable
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
        /// 椭圆中心点。
        /// </summary>
        public SKPoint Center { get; set; }

        /// <summary>
        /// X 轴半径。
        /// </summary>
        public float RadiusX { get; set; }

        /// <summary>
        /// Y 轴半径。
        /// </summary>
        public float RadiusY { get; set; }

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

            var rect = new SKRect(
                Center.X - RadiusX,
                Center.Y - RadiusY,
                Center.X + RadiusX,
                Center.Y + RadiusY
            );

            if (FillColor.Alpha > 0)
            {
                using var fillPaint = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    Color = FillColor,
                    IsAntialias = true
                };
                canvas.DrawOval(rect, fillPaint);
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
                canvas.DrawOval(rect, strokePaint);
            }

            canvas.Restore();
        }

        /// <summary>
        /// 命中测试。
        /// </summary>
        public bool HitTest(SKPoint point)
        {
            if (!Visible) return false;
            SKMatrix invert;
            if (!Transform.TryInvert(out invert))
                return false;
            var local = invert.MapPoint(point);

            // 标准椭圆命中测试公式
            float dx = local.X - Center.X;
            float dy = local.Y - Center.Y;
            if (RadiusX == 0 || RadiusY == 0) return false;
            float value = (dx * dx) / (RadiusX * RadiusX) + (dy * dy) / (RadiusY * RadiusY);
            return value <= 1.0f;
        }

        /// <summary>
        /// 深拷贝。
        /// </summary>
        public object Clone()
        {
            return new Ellipse
            {
                Id = Guid.NewGuid().ToString(),
                Visible = this.Visible,
                Center = this.Center,
                RadiusX = this.RadiusX,
                RadiusY = this.RadiusY,
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
            System.Diagnostics.Debug.WriteLine($"[Ellipse.Serialize] Id:{Id} Json:{json}");
            return json;
        }

        /// <summary>
        /// 从 JSON 反序列化。
        /// </summary>
        public static Ellipse? Deserialize(string json)
        {
            var ellipse = JsonSerializer.Deserialize<Ellipse>(json);
            System.Diagnostics.Debug.WriteLine($"[Ellipse.Deserialize] Json:{json} ResultId:{ellipse?.Id}");
            return ellipse;
        }
    }
}