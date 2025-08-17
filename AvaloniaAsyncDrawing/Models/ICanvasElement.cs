// AvaloniaAsyncDrawing/Models/ICanvasElement.cs
using SkiaSharp;

namespace AvaloniaAsyncDrawing.Models
{
    /// <summary>
    /// 所有可绘制元素的基础接口，定义统一的 Id、Visible 属性及渲染方法。
    /// </summary>
    public interface ICanvasElement
    {
        /// <summary>
        /// 元素唯一标识。
        /// </summary>
        string Id { get; }

        /// <summary>
        /// 是否可见。
        /// </summary>
        bool Visible { get; }

        /// <summary>
        /// 渲染方法。
        /// </summary>
        void Render(SKCanvas canvas);
    }
}