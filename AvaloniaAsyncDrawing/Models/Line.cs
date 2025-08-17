// AvaloniaAsyncDrawing/Models/Line.cs
using SkiaSharp;
using System;
using AvaloniaAsyncDrawing.Models;
using System.Globalization;

namespace AvaloniaAsyncDrawing.Models
{
    /// <summary>
    /// 类型安全的线段数据结构，实现 ICanvasElement, IRenderable。
    /// </summary>
    public class Line : ICanvasElement, IRenderable, IHitTestable
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
        /// 起点。
        /// </summary>
        public SKPoint Start { get; set; }

        /// <summary>
        /// 终点。
        /// </summary>
        public SKPoint End { get; set; }

        /// <summary>
        /// 线段颜色。
        /// </summary>
        public SKColor Color { get; set; } = SKColors.Black;

        /// <summary>
        /// 线宽。
        /// </summary>
        public float StrokeWidth { get; set; } = 1.0f;

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
            using var paint = new SKPaint
            {
                Color = Color,
                StrokeWidth = StrokeWidth,
                IsStroke = true,
                IsAntialias = true
            };
            canvas.Save();
            canvas.SetMatrix(Transform);
            canvas.DrawLine(Start, End, paint);
            canvas.Restore();
        }

        /// <summary>
        /// 命中测试（点到线段距离小于等于2像素）。
        /// </summary>
        public bool HitTest(SKPoint point)
        {
            if (!Visible) return false;
            var matrix = Transform;
            var s = matrix.MapPoint(Start);
            var e = matrix.MapPoint(End);
            float dist = DistancePointToSegment(point, s, e);
            return dist <= 2f;
        }

        /// <summary>
        /// 点到线段距离算法。
        /// </summary>
        private static float DistancePointToSegment(SKPoint p, SKPoint a, SKPoint b)
        {
            float dx = b.X - a.X;
            float dy = b.Y - a.Y;
            if (dx == 0 && dy == 0) return SKPoint.Distance(p, a);
            float t = ((p.X - a.X) * dx + (p.Y - a.Y) * dy) / (dx * dx + dy * dy);
            t = Math.Clamp(t, 0, 1);
            float projX = a.X + t * dx;
            float projY = a.Y + t * dy;
            return SKPoint.Distance(p, new SKPoint(projX, projY));
        }

        /// <summary>
        /// 简单序列化为字符串（格式：x1,y1,x2,y2,#AARRGGBB,strokeWidth）。
        /// </summary>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture,
                "{0},{1},{2},{3},{4},{5}",
                Start.X, Start.Y, End.X, End.Y, Color.ToString(), StrokeWidth);
        }

        /// <summary>
        /// 从字符串反序列化。
        /// </summary>
        public static Line Parse(string s)
        {
            var parts = s.Split(',');
            if (parts.Length != 6) throw new FormatException("Line string format error.");
            return new Line
            {
                Start = new SKPoint(float.Parse(parts[0], CultureInfo.InvariantCulture), float.Parse(parts[1], CultureInfo.InvariantCulture)),
                End = new SKPoint(float.Parse(parts[2], CultureInfo.InvariantCulture), float.Parse(parts[3], CultureInfo.InvariantCulture)),
                Color = SKColor.Parse(parts[4]),
                StrokeWidth = float.Parse(parts[5], CultureInfo.InvariantCulture)
            };
        }

        /// <summary>
        /// 克隆。
        /// </summary>
        public Line Clone()
        {
            return new Line
            {
                Id = this.Id,
                Visible = this.Visible,
                Start = this.Start,
                End = this.End,
                Color = this.Color,
                StrokeWidth = this.StrokeWidth,
                Transform = this.Transform
            };
        }
    }
}