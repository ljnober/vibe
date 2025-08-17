// SelectionTool：支持对象选择与多选
using System;
using System.Collections.Generic;

namespace AvaloniaAsyncDrawing.Drawing
{
    public class SelectionTool : BaseTool, IDrawingTool
    {
        private readonly HashSet<object> _selectedObjects = new HashSet<object>();

        public IReadOnlyCollection<object> SelectedObjects => _selectedObjects;

        public SelectionTool() : base("SelectionTool", "选择工具") { }

        public void OnPointerDown(double x, double y)
        {
            // 示例：假设命中检测逻辑已由外部注入
            // 支持多选（如按Ctrl），此处简化为单选
            _selectedObjects.Clear();
            var hit = HitTest(x, y);
            if (hit != null)
                _selectedObjects.Add(hit);
        }

        public void OnPointerMove(double x, double y)
        {
            // 可扩展为拖拽选择框等
        }

        public void OnPointerUp(double x, double y)
        {
            // 结束选择操作
        }

        public void Select(object obj)
        {
            if (obj != null)
                _selectedObjects.Add(obj);
        }

        public void Deselect(object obj)
        {
            if (obj != null)
                _selectedObjects.Remove(obj);
        }

        public void ClearSelection()
        {
            _selectedObjects.Clear();
        }

        private object? HitTest(double x, double y)
        {
            // 需集成画布对象管理，暂留空实现
            return null;
        }

        public override void Reset()
        {
            base.Reset();
            _selectedObjects.Clear();
        }
    }
}