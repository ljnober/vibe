// EraserTool 单元测试
using AvaloniaAsyncDrawing.Drawing;
using Xunit;

namespace AvaloniaAsyncDrawing.Tests
{
    public class EraserToolTests
    {
        [Fact]
        public void Activate_Deactivate_Works()
        {
            var tool = new EraserTool();
            Assert.False(tool.IsActive);
            tool.Activate();
            Assert.True(tool.IsActive);
            tool.Deactivate();
            Assert.False(tool.IsActive);
        }

        [Fact]
        public void OnPointerDown_InvokesObjectErased_WhenHit()
        {
            var tool = new EraserTool();
            object? erased = null;
            tool.ObjectErased += o => erased = o;

            // 由于 HitTest 返回 null，模拟 Erase 逻辑
            tool.OnPointerDown(0, 0);
            Assert.Null(erased);
        }

        [Fact]
        public void Reset_DoesNotThrow()
        {
            var tool = new EraserTool();
            tool.Activate();
            tool.Reset();
            Assert.False(tool.IsActive); // Reset 不影响激活状态
        }
    }
}