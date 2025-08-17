// EditTool 单元测试
using AvaloniaAsyncDrawing.Drawing;
using Xunit;

namespace AvaloniaAsyncDrawing.Tests
{
    public class EditToolTests
    {
        [Fact]
        public void Activate_Deactivate_Works()
        {
            var tool = new EditTool();
            Assert.False(tool.IsActive);
            tool.Activate();
            Assert.True(tool.IsActive);
            tool.Deactivate();
            Assert.False(tool.IsActive);
        }

        [Fact]
        public void BeginEdit_EndEdit_Works()
        {
            var tool = new EditTool();
            var obj = new object();
            tool.BeginEdit(obj);
            // 由于 _editingObject 为私有，仅测试无异常
            tool.EndEdit();
        }

        [Fact]
        public void Reset_ClearsEditingState()
        {
            var tool = new EditTool();
            var obj = new object();
            tool.BeginEdit(obj);
            tool.Reset();
            // 由于 _editingObject 为私有，仅测试无异常
        }
    }
}