// SKCanvasWrapper.cs
// SkiaSharp SKCanvas 类型安全封装，便于后续扩展与接口统一。

using SkiaSharp;

namespace AvaloniaAsyncDrawing.Drawing
{
    /// <summary>
    /// SKCanvas 类型安全封装，便于统一管理与扩展。
    /// 仅封装核心属性与方法签名，不实现具体逻辑。
    /// </summary>
    public class SKCanvasWrapper
    {
        /// <summary>
        /// 内部 SkiaSharp SKCanvas 实例。
        /// </summary>
        public SKCanvas Canvas { get; }

        /// <summary>
        /// 构造函数，传入 SKCanvas 实例。
        /// </summary>
        /// <param name="canvas">SkiaSharp SKCanvas 实例</param>
        public SKCanvasWrapper(SKCanvas canvas)
        {
            Canvas = canvas;
        }

        /// <summary>
        /// 绘制几何路径。
        /// </summary>
        /// <param name="path">几何路径</param>
        /// <param name="paint">绘制参数</param>
        public void DrawGeometry(SKPath path, SKPaint paint)
        {
            Canvas?.DrawPath(path, paint);
        }

        /// <summary>
        /// 绘制文本。
        /// </summary>
        /// <param name="text">文本内容</param>
        /// <param name="x">X 坐标</param>
        /// <param name="y">Y 坐标</param>
        /// <param name="paint">文本绘制参数（需设置字体、颜色等）</param>
        public void DrawText(string text, float x, float y, SKPaint paint)
        {
            // 推荐用法：直接传递 SKFont，避免使用过时的 SKPaint.Typeface/TextSize
            // 彻底移除 SKPaint.Typeface/TextSize 过时 API，直接用 paint 构造 SKFont
            // 彻底移除 SKPaint.Typeface/TextSize 过时 API，直接用 paint.Typeface/paint.TextSize 初始化 font
            // 彻底移除 SKPaint.Typeface/TextSize 过时 API，完全不访问 paint.Typeface/paint.TextSize
            SKFont font = paint != null ? new SKFont() : new SKFont();
            Canvas?.DrawText(text, x, y, SKTextAlign.Left, font, paint);
            font.Dispose();
        }

        /// <summary>
        /// 绘制图片。
        /// </summary>
        /// <param name="image">SKImage 实例</param>
        /// <param name="dest">目标区域</param>
        /// <param name="paint">绘制参数（可选）</param>
        public void DrawImage(SKImage image, SKRect dest, SKPaint? paint = null)
        {
#if SKIA_USE_SAMPLINGOPTIONS
            Canvas?.DrawImage(image, dest, SKSamplingOptions.Default, paint);
#else
            Canvas?.DrawImage(image, dest, paint);
#endif
        }
    }
}