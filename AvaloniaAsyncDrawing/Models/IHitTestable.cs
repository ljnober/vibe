// AvaloniaAsyncDrawing/Models/IHitTestable.cs
using SkiaSharp;

namespace AvaloniaAsyncDrawing.Models
{
    /// <summary>
    /// 命中测试接口，支持多形状扩展。
    /// </summary>
    public interface IHitTestable
    {
        /// <summary>
        /// 判断点是否命中当前对象。
        /// </summary>
        /// <param name="point">测试点（画布坐标）</param>
        /// <returns>是否命中</returns>
        bool HitTest(SKPoint point);
    }
}