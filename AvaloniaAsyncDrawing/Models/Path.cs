// AvaloniaAsyncDrawing/Models/Path.cs
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AvaloniaAsyncDrawing.Models
{
    /// <summary>
    /// 路径命令类型。
    /// </summary>
    public enum PathCommandType
    {
        MoveTo,
        LineTo,
        QuadTo,
        CubicTo,
        Close
    }

    /// <summary>
    /// 路径命令，类型安全。
    /// </summary>
    public record PathCommand(PathCommandType Type, SKPoint[] Points);

    /// <summary>
    /// 路径数据结构，实现 ICanvasElement/IRenderable。
    /// </summary>
    public class Path : ICanvasElement, IRenderable, IHitTestable
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public bool Visible { get; set; } = true;
        public string Type { get; set; } = "Path";

        /// <summary>
        /// 路径命令集。
        /// </summary>
        public List<PathCommand> Commands { get; set; } = new List<PathCommand>();

        /// <summary>
        /// 路径所有点集（自动生成，仅读）。
        /// </summary>
        [JsonIgnore]
        public IReadOnlyList<SKPoint> Points => Commands.SelectMany(c => c.Points).ToArray();

        /// <summary>
        /// 填充颜色（可空）。
        /// </summary>
        [System.Text.Json.Serialization.JsonConverter(typeof(SKColorJsonConverter))]
        public SKColor? FillColor { get; set; }

        /// <summary>
        /// 描边颜色（可空）。
        /// </summary>
        [System.Text.Json.Serialization.JsonConverter(typeof(SKColorJsonConverter))]
        public SKColor? StrokeColor { get; set; }

        /// <summary>
        /// 描边粗细。
        /// </summary>
        public float StrokeWidth { get; set; } = 1f;

        /// <summary>
        /// 变换矩阵。
        /// </summary>
        public SKMatrix Transform { get; set; } = SKMatrix.CreateIdentity();

        /// <summary>
        /// 构建 Skia 路径对象。
        /// </summary>
        [JsonIgnore]
        public SKPath SkPath
        {
            get
            {
                var path = new SKPath();
                foreach (var cmd in Commands)
                {
                    switch (cmd.Type)
                    {
                        case PathCommandType.MoveTo:
                            path.MoveTo(cmd.Points[0]);
                            break;
                        case PathCommandType.LineTo:
                            path.LineTo(cmd.Points[0]);
                            break;
                        case PathCommandType.QuadTo:
                            path.QuadTo(cmd.Points[0], cmd.Points[1]);
                            break;
                        case PathCommandType.CubicTo:
                            path.CubicTo(cmd.Points[0], cmd.Points[1], cmd.Points[2]);
                            break;
                        case PathCommandType.Close:
                            path.Close();
                            break;
                    }
                }
                return path;
            }
        }

        /// <summary>
        /// 渲染方法。
        /// </summary>
        public void Render(SKCanvas canvas)
        {
            if (!Visible) return;
            canvas.Save();
            canvas.SetMatrix(Transform);

            if (FillColor.HasValue)
            {
                using var fillPaint = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    Color = FillColor.Value,
                    IsAntialias = true
                };
                canvas.DrawPath(SkPath, fillPaint);
            }
            if (StrokeColor.HasValue && StrokeWidth > 0)
            {
                using var strokePaint = new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    Color = StrokeColor.Value,
                    StrokeWidth = StrokeWidth,
                    IsAntialias = true
                };
                canvas.DrawPath(SkPath, strokePaint);
            }
            canvas.Restore();
        }

        /// <summary>
        /// 命中测试。
        /// </summary>
        public bool HitTest(SKPoint point)
        {
            if (!Visible) return false;
            using var path = new SKPath();
            path.AddPath(SkPath, Transform);
            if (FillColor.HasValue && path.Contains(point.X, point.Y))
                return true;
            if (StrokeColor.HasValue && StrokeWidth > 0)
            {
                using var paint = new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    StrokeWidth = StrokeWidth + 3f // 容差
                };
                // SkiaSharp 无 StrokeContains，退化为 Contains
                return path.Contains(point.X, point.Y);
            }
            return false;
        }

        /// <summary>
        /// 深拷贝。
        /// </summary>
        public Path Clone()
        {
            return new Path
            {
                Id = this.Id,
                Visible = this.Visible,
                Type = this.Type,
                Commands = this.Commands.Select(c => new PathCommand(c.Type, c.Points.ToArray())).ToList(),
                FillColor = this.FillColor,
                StrokeColor = this.StrokeColor,
                StrokeWidth = this.StrokeWidth,
                Transform = this.Transform
            };
        }

        /// <summary>
        /// 序列化为 JSON。
        /// </summary>
        public string Serialize()
        {
            return JsonSerializer.Serialize(this, GetType(), new JsonSerializerOptions
            {
                WriteIndented = false,
                IncludeFields = true
            });
        }

        /// <summary>
        /// 反序列化。
        /// </summary>
        public static Path? Deserialize(string json)
        {
            var path = JsonSerializer.Deserialize<Path>(json);
            System.Diagnostics.Debug.WriteLine($"[Path.Deserialize] Json:{json} ResultId:{path?.Id}");
            return path;
        }
    }
}