// AvaloniaAsyncDrawing/Models/ImageData.cs
using SkiaSharp;

namespace AvaloniaAsyncDrawing.Models
{
    /// <summary>
    /// 图片对象数据类型声明，实现 ICanvasElement 与 IRenderable。
    /// </summary>
    public class ImageData : ICanvasElement, IRenderable
    {
        /// <summary>
        /// ICanvasElement/IRenderable 渲染接口实现（供多态调用）。
        /// </summary>
        public void Render(SKCanvas canvas)
        {
            Render(canvas, DestRect, Paint);
        }
        /// <summary>
        /// 对象唯一标识。
        /// </summary>
        public string Id { get; set; } = string.Empty;
    
        /// <summary>
        /// 是否可见。
        /// </summary>
        public bool Visible { get; set; }
    
        /// <summary>
        /// 位图像素数据。
        /// </summary>
        public SKBitmap? Bitmap { get; set; }
    
        /// <summary>
        /// 目标绘制区域。
        /// </summary>
        public SKRect DestRect { get; set; }
    
        /// <summary>
        /// 绘制参数。
        /// </summary>
        public SKPaint? Paint { get; set; }
    
        /// <summary>
        /// 变换矩阵。
        /// </summary>
        public SKMatrix Transform { get; set; }
    
        /// <summary>
        /// 渲染方法。
        /// </summary>
        public void Render(SKCanvas canvas, SKRect destRect, SKPaint? paint = null)
        {
            if (!Visible || Bitmap == null) return;
            canvas.Save();
            canvas.SetMatrix(Transform);
            var usePaint = paint ?? Paint ?? new SKPaint { };
#if SKIA_USE_SAMPLINGOPTIONS
            canvas.DrawBitmap(Bitmap, destRect, SKSamplingOptions.Default, usePaint);
#else
            canvas.DrawBitmap(Bitmap, destRect, usePaint);
#endif
            canvas.Restore();
        }
    
        /// <summary>
        /// 命中测试。
        /// </summary>
        public bool HitTest(SKPoint point)
        {
            if (!Visible) return false;
            return DestRect.Contains(point);
        }
    
        /// <summary>
        /// 深拷贝。
        /// </summary>
        public ImageData Clone()
        {
            return new ImageData
            {
                Id = this.Id,
                Visible = this.Visible,
                Bitmap = this.Bitmap?.Copy(),
                DestRect = this.DestRect,
                Paint = this.Paint?.Clone(),
                Transform = this.Transform
            };
        }
    }
}