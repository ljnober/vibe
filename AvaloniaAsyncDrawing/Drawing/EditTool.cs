// EditTool：支持节点/属性编辑
using System;

namespace AvaloniaAsyncDrawing.Drawing
{
    public class EditTool : BaseTool, IDrawingTool
    {
        private object? _editingObject;
        private bool _isEditing;

        public EditTool() : base("EditTool", "编辑工具") { }

        public void OnPointerDown(double x, double y)
        {
            _editingObject = HitTest(x, y);
            _isEditing = _editingObject != null;
        }

        public void OnPointerMove(double x, double y)
        {
            if (_isEditing && _editingObject != null)
            {
                // 节点/属性编辑逻辑（如拖动节点、修改属性等）
            }
        }

        public void OnPointerUp(double x, double y)
        {
            _isEditing = false;
            _editingObject = null;
        }

        public void BeginEdit(object obj)
        {
            _editingObject = obj;
            _isEditing = obj != null;
        }

        public void EndEdit()
        {
            _isEditing = false;
            _editingObject = null;
        }

        private object? HitTest(double x, double y)
        {
            // 需集成画布对象管理，暂留空实现
            return null;
        }

        public override void Reset()
        {
            base.Reset();
            _isEditing = false;
            _editingObject = null;
        }
    }
}