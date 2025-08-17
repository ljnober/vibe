// AvaloniaAsyncDrawing/Models/TextData.cs
using SkiaSharp;

namespace AvaloniaAsyncDrawing.Models
{
    /// <summary>
    /// 文本对象数据类型声明，实现 ICanvasElement 与 IRenderable。
    /// </summary>
    public class TextData : ICanvasElement, IRenderable
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
        /// 文本内容。
        /// </summary>
        public string Content { get; set; } = string.Empty;
    
        /// <summary>
        /// 字体。
        /// </summary>
        public SKFont? Font { get; set; }
    
        /// <summary>
        /// 位置。
        /// </summary>
        public SKPoint Position { get; set; }
    
        /// <summary>
        /// 颜色。
        /// </summary>
        public SKColor Color { get; set; }
    
        /// <summary>
        /// 渲染方法。
        /// </summary>
        public void Render(SKCanvas canvas)
        {
            if (!Visible || string.IsNullOrEmpty(Content)) return;
            if (Font != null)
            {
                using var paint = new SKPaint { Color = Color };
                canvas.DrawText(Content, Position.X, Position.Y, SKTextAlign.Left, Font, paint);
            }
            else
            {
                using var paint = new SKPaint { Color = Color };
                canvas.DrawText(Content, Position.X, Position.Y, SKTextAlign.Left, new SKFont(), paint);
            }
        }
    
        /// <summary>
        /// 命中测试。
        /// </summary>
        public bool HitTest(SKPoint point)
        {
            if (!Visible || string.IsNullOrEmpty(Content)) return false;
            // 兼容 SKFont.MeasureText(string) 返回 float 的新 API，命中测试需自定义 bounds
            using var font = new SKFont();
            float width = font.MeasureText(Content);
            SKRect bounds = new SKRect(Position.X, Position.Y, Position.X + width, Position.Y + font.Size);
            return bounds.Contains(point);
        }
    
        /// <summary>
        /// 深拷贝。
        /// </summary>
        public TextData Clone()
        {
            return new TextData
            {
                Id = this.Id,
                Visible = this.Visible,
                Content = this.Content,
                Font = this.Font != null ? new SKFont(this.Font.Typeface, this.Font.Size) : null,
                Position = this.Position,
                Color = this.Color
            };
        }
    }
}