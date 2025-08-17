using System;

namespace AvaloniaAsyncDrawing.Drawing
{
    /// <summary>
    /// 工具抽象基类，实现 ITool 通用逻辑，便于扩展具体工具。
    /// </summary>
    public abstract class BaseTool : ITool
    {
        public string Id { get; }
        public string DisplayName { get; }
        public bool IsActive { get; private set; }

        protected BaseTool(string id, string displayName)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
            IsActive = false;
        }

        public virtual void Activate()
        {
            IsActive = true;
            OnActivated();
        }

        public virtual void Deactivate()
        {
            IsActive = false;
            OnDeactivated();
        }

        public virtual void Reset()
        {
            OnReset();
        }

        /// <summary>
        /// 激活时的扩展逻辑。
        /// </summary>
        protected virtual void OnActivated() { }

        /// <summary>
        /// 取消激活时的扩展逻辑。
        /// </summary>
        protected virtual void OnDeactivated() { }

        /// <summary>
        /// 重置时的扩展逻辑。
        /// </summary>
        protected virtual void OnReset() { }
    }
}