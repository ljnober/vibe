// SelectionTool 单元测试
using AvaloniaAsyncDrawing.Drawing;
using Xunit;

namespace AvaloniaAsyncDrawing.Tests
{
    public class SelectionToolTests
    {
        [Fact]
        public void Activate_Deactivate_Works()
        {
            var tool = new SelectionTool();
            Assert.False(tool.IsActive);
            tool.Activate();
            Assert.True(tool.IsActive);
            tool.Deactivate();
            Assert.False(tool.IsActive);
        }

        [Fact]
        public void Select_Deselect_ClearSelection_Works()
        {
            var tool = new SelectionTool();
            var obj1 = new object();
            var obj2 = new object();

            tool.Select(obj1);
            Assert.Contains(obj1, tool.SelectedObjects);

            tool.Select(obj2);
            Assert.Contains(obj2, tool.SelectedObjects);

            tool.Deselect(obj1);
            Assert.DoesNotContain(obj1, tool.SelectedObjects);

            tool.ClearSelection();
            Assert.Empty(tool.SelectedObjects);
        }

        [Fact]
        public void Reset_ClearsSelection()
        {
            var tool = new SelectionTool();
            var obj = new object();
            tool.Select(obj);
            Assert.NotEmpty(tool.SelectedObjects);
            tool.Reset();
            Assert.Empty(tool.SelectedObjects);
        }
    }
}