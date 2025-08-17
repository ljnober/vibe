using System;

namespace AvaloniaAsyncDrawing.Drawing
{
    /// <summary>
    /// 绘图工具通用接口，定义工具的激活、操作、状态管理等方法。
    /// </summary>
    public interface ITool
    {
        /// <summary>
        /// 工具唯一标识。
        /// </summary>
        string Id { get; }

        /// <summary>
        /// 工具显示名称。
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// 工具是否处于激活状态。
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// 激活工具。
        /// </summary>
        void Activate();

        /// <summary>
        /// 取消激活工具。
        /// </summary>
        void Deactivate();

        /// <summary>
        /// 重置工具状态。
        /// </summary>
        void Reset();
    }
}