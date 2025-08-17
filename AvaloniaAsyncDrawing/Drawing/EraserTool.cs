// EraserTool：支持对象/路径擦除
using System;
using System.Collections.Generic;

namespace AvaloniaAsyncDrawing.Drawing
{
    public class EraserTool : BaseTool, IDrawingTool
    {
        public EraserTool() : base("EraserTool", "橡皮工具") { }

        public event Action<object>? ObjectErased;

        public void OnPointerDown(double x, double y)
        {
            var target = HitTest(x, y);
            if (target != null)
            {
                Erase(target);
                ObjectErased?.Invoke(target);
            }
        }

        public void OnPointerMove(double x, double y)
        {
            // 支持连续擦除（如拖动擦除）
        }

        public void OnPointerUp(double x, double y)
        {
            // 结束擦除操作
        }

        public void Erase(object obj)
        {
            // 实际擦除逻辑需集成画布对象管理
        }

        private object? HitTest(double x, double y)
        {
            // 需集成画布对象管理，暂留空实现
            return null;
        }

        public override void Reset()
        {
            base.Reset();
            this.Deactivate();
        }
    }
}