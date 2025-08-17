// SkiaImageDrawOperation.cs
// ICustomDrawOperation 典型实现：用于将 SKImage 绘制到指定区域，支持命中测试与资源释放。

using System;
using Avalonia;
using Avalonia.Media;
using SkiaSharp;

namespace AvaloniaAsyncDrawing.Drawing
{
    /// <summary>
    /// 典型自定义绘制操作，实现 ICustomDrawOperation。
    /// 用于将 SKImage 绘制到指定区域，支持命中测试与资源释放。
    /// </summary>
    public class SkiaImageDrawOperation : ICustomDrawOperation
    {
        private readonly SKImage _image;
        private readonly Rect _rect;
        private bool _disposed;

        /// <summary>
        /// 构造函数，指定绘制内容与区域。
        /// </summary>
        /// <param name="image">要绘制的 SKImage</param>
        /// <param name="rect">目标区域（设备坐标）</param>
        public SkiaImageDrawOperation(SKImage image, Rect rect)
        {
            _image = image ?? throw new ArgumentNullException(nameof(image));
            _rect = rect;
        }

        /// <inheritdoc/>
        public Rect Bounds => _rect;

        /// <inheritdoc/>
        public void Render(ImmediateDrawingContext context)
        {
            if (_disposed || _image == null) return;
            // 兼容 Avalonia 官方 Skia API，直接获取 SkiaSharp 绘图上下文
            var lease = context.GetType().GetMethod("GetSkiaSurface")?.Invoke(context, null);
            if (lease != null)
            {
                var canvasProp = lease.GetType().GetProperty("SkCanvas");
                var canvas = canvasProp?.GetValue(lease) as SKCanvas;
                if (canvas != null)
                {
                    canvas.DrawImage(_image, new SKRect(
                        (float)_rect.X, (float)_rect.Y,
                        (float)(_rect.X + _rect.Width), (float)(_rect.Y + _rect.Height)));
                }
            }
        }

        /// <inheritdoc/>
        public bool HitTest(Point point) => _rect.Contains(point);

        /// <inheritdoc/>
        public void Dispose()
        {
            if (!_disposed)
            {
                _image?.Dispose();
                _disposed = true;
            }
        }

        /// <inheritdoc/>
        public bool Equals(ICustomDrawOperation? other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (other is SkiaImageDrawOperation op)
                return Equals(_image, op._image) && _rect.Equals(op._rect);
            return false;
        }
    }
}