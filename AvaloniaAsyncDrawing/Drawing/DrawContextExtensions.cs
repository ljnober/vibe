// 渲染上下文扩展及自定义绘制操作接口定义
using System;
using Avalonia;
using Avalonia.Media;

namespace AvaloniaAsyncDrawing.Drawing
{
    /// <summary>
    /// Avalonia 渲染上下文扩展方法，支持自定义异步/同步绘制流程。
    /// </summary>
    public static class DrawContextExtensions
    {
        /// <summary>
        /// 在 DrawingContext 上执行自定义绘制操作。
        /// </summary>
        /// <param name="context">绘制上下文</param>
        /// <param name="custom">自定义绘制操作实例</param>
        public static void Custom(this DrawingContext context, ICustomDrawOperation custom)
        {
            // 仅接口声明，无实现
            throw new NotImplementedException();
        }
    }
}