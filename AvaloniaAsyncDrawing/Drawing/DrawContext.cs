// DrawContext.cs
// 绘制上下文基础类，供自定义绘制与渲染流程使用。

using Avalonia;
using Avalonia.Media;
using SkiaSharp;

namespace AvaloniaAsyncDrawing.Drawing
{
    /// <summary>
    /// 绘制上下文基础类，封装与 SkiaSharp/Avalonia 相关的绘制环境。
    /// 仅定义核心属性与方法签名，不实现具体逻辑。
    /// </summary>
    public abstract class DrawContext
    {
        /// <summary>
        /// 当前 SKCanvas 实例。
        /// </summary>
        public abstract SKCanvas Canvas { get; }

        /// <summary>
        /// 通过自定义操作进行绘制（参考 Avalonia DrawContext.Custom）。
        /// </summary>
        /// <param name="custom">自定义绘制操作</param>
        public abstract void Custom(ICustomDrawOperation custom);
    }
}