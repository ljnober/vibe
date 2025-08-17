// AvaloniaAsyncDrawing/Models/GeometryData.cs
using SkiaSharp;

namespace AvaloniaAsyncDrawing.Models
{
    /// <summary>
    /// 几何对象数据类型声明，实现 ICanvasElement 与 IRenderable。
    /// </summary>
    public class GeometryData : ICanvasElement, IRenderable, IHitTestable
    {
        /// <summary>
        /// 对象唯一标识。
        /// </summary>
        public string Id { get; set; } = string.Empty;
    
        /// <summary>
        /// 是否可见。
        /// </summary>
        public bool Visible { get; set; }
    
        /// <summary>
        /// 类型描述（如 Rectangle、Ellipse 等）。
        /// </summary>
        public string Type { get; set; } = string.Empty;
    
        /// <summary>
        /// 顶点坐标数组。
        /// </summary>
        public SKPoint[] Points { get; set; } = Array.Empty<SKPoint>();
    
        /// <summary>
        /// 路径对象。
        /// </summary>
        public SKPath? Path { get; set; }
    
        /// <summary>
        /// 填充画笔。
        /// </summary>
        public SKPaint? FillPaint { get; set; }
    
        /// <summary>
        /// 描边画笔。
        /// </summary>
        public SKPaint? StrokePaint { get; set; }
    
        /// <summary>
        /// 变换矩阵。
        /// </summary>
        public SKMatrix Transform { get; set; }
    
        /// <summary>
        /// 渲染方法。
        /// </summary>
        public void Render(SKCanvas canvas)
        {
            if (!Visible) return;
            canvas.Save();
            canvas.SetMatrix(Transform);
            if (StrokePaint != null)
            {
                if (Path != null)
                    canvas.DrawPath(Path, StrokePaint);
                else if (Points != null && Points.Length > 1)
                    canvas.DrawPoints(SKPointMode.Polygon, Points, StrokePaint);
            }
            if (FillPaint != null && Path != null)
                canvas.DrawPath(Path, FillPaint);
            canvas.Restore();
        }
    
        /// <summary>
        /// 命中测试。
        /// </summary>
        public bool HitTest(SKPoint point)
        {
            if (!Visible) return false;
            // 优先 Path 区域命中
            if (Path != null)
            {
                using (var path = new SKPath())
                {
                    path.AddPath(Path, Transform);
                    if (path.Contains(point.X, point.Y))
                        return true;
                }
            }
            // Rectangle 区域命中
            if (Type == "Rectangle" && Points != null && Points.Length == 4)
            {
                var x0 = Points[0].X;
                var y0 = Points[0].Y;
                var x2 = Points[2].X;
                var y2 = Points[2].Y;
                var minX = Math.Min(x0, x2);
                var maxX = Math.Max(x0, x2);
                var minY = Math.Min(y0, y2);
                var maxY = Math.Max(y0, y2);
                if (point.X >= minX && point.X <= maxX && point.Y >= minY && point.Y <= maxY)
                    return true;
            }
            // Ellipse 区域命中
            if (Type == "Ellipse" && Points != null && Points.Length == 4)
            {
                // 椭圆中心与半轴
                var cx = (Points[0].X + Points[1].X) / 2;
                var cy = (Points[2].Y + Points[3].Y) / 2;
                var rx = Math.Abs(Points[1].X - Points[0].X) / 2;
                var ry = Math.Abs(Points[2].Y - Points[0].Y) / 2;
                if (rx > 0 && ry > 0)
                {
                    var norm = Math.Pow((point.X - cx) / rx, 2) + Math.Pow((point.Y - cy) / ry, 2);
                    if (norm <= 1.0)
                        return true;
                }
            }
            // 点近似命中
            if (Points != null && Points.Length > 0)
            {
                foreach (var p in Points)
                {
                    var tp = Transform.MapPoint(p);
                    if (SKPoint.Distance(tp, point) < 3f) // 3像素容差
                        return true;
                }
            }
            return false;
        }
    
        /// <summary>
        /// 深拷贝。
        /// </summary>
        public GeometryData Clone()
        {
            return new GeometryData
            {
                Id = this.Id,
                Visible = this.Visible,
                Type = this.Type,
                Points = (SKPoint[])this.Points.Clone(),
                Path = this.Path != null ? new SKPath(this.Path) : null,
                FillPaint = this.FillPaint?.Clone(),
                StrokePaint = this.StrokePaint?.Clone(),
                Transform = this.Transform
            };
        }
    }
}