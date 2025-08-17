using System;

namespace AvaloniaAsyncDrawing.Drawing
{
    /// <summary>
    /// 绘图类工具接口，扩展 ITool，定义与绘图相关的操作。
    /// </summary>
    public interface IDrawingTool : ITool
    {
        /// <summary>
        /// 鼠标按下事件。
        /// </summary>
        void OnPointerDown(double x, double y);

        /// <summary>
        /// 鼠标移动事件。
        /// </summary>
        void OnPointerMove(double x, double y);

        /// <summary>
        /// 鼠标释放事件。
        /// </summary>
        void OnPointerUp(double x, double y);
    }
}