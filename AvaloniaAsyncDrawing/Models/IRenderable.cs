// AvaloniaAsyncDrawing/Models/IRenderable.cs
using SkiaSharp;

namespace AvaloniaAsyncDrawing.Models
{
    /// <summary>
    /// 渲染行为接口，支持自定义渲染流程。
    /// </summary>
    public interface IRenderable
    {
        /// <summary>
        /// 渲染方法。
        /// </summary>
        void Render(SKCanvas canvas);
    }
}