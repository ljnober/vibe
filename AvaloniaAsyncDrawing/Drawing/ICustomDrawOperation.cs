// ICustomDrawOperation.cs
// 参照 Avalonia 官方接口定义，供自定义绘制操作使用。

using System;
using Avalonia;
using Avalonia.Media;

namespace AvaloniaAsyncDrawing.Drawing
{
    /// <summary>
    /// 自定义绘制操作接口，需实现以支持 DrawContext.Custom 调用。
    /// 参考 Avalonia.IImmediateCustomDrawOperation。
    /// </summary>
    public interface ICustomDrawOperation : IDisposable, IEquatable<ICustomDrawOperation>
    {
        /// <summary>
        /// 获取绘制区域边界（设备坐标）。
        /// </summary>
        Rect Bounds { get; }

        /// <summary>
        /// 在指定上下文中执行自定义绘制。
        /// </summary>
        /// <param name="context">绘制上下文</param>
        void Render(ImmediateDrawingContext context);

        /// <summary>
        /// 判断指定点是否命中该操作区域。
        /// </summary>
        /// <param name="point">测试点</param>
        /// <returns>是否命中</returns>
        bool HitTest(Point point);
    }
}