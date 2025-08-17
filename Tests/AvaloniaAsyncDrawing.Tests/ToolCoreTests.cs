using System;
using AvaloniaAsyncDrawing.Drawing;
using Xunit;

namespace AvaloniaAsyncDrawing.Tests
{
    /// <summary>
    /// 针对 ITool、IDrawingTool、BaseTool 的核心行为单元测试。
    /// </summary>
    public class ToolCoreTests
    {
        private class DummyTool : BaseTool
        {
            public bool Activated { get; private set; }
            public bool Deactivated { get; private set; }
            public bool ResetCalled { get; private set; }

            public DummyTool(string id, string name) : base(id, name) { }

            protected override void OnActivated() => Activated = true;
            protected override void OnDeactivated() => Deactivated = true;
            protected override void OnReset() => ResetCalled = true;
        }

        private class DummyDrawingTool : DummyTool, IDrawingTool
        {
            public bool PointerDown { get; private set; }
            public bool PointerMove { get; private set; }
            public bool PointerUp { get; private set; }

            public DummyDrawingTool(string id, string name) : base(id, name) { }

            public void OnPointerDown(double x, double y) => PointerDown = true;
            public void OnPointerMove(double x, double y) => PointerMove = true;
            public void OnPointerUp(double x, double y) => PointerUp = true;
        }

        [Fact]
        public void BaseTool_Activation_State_Transitions()
        {
            var tool = new DummyTool("dummy", "Dummy Tool");
            Assert.False(tool.IsActive);

            tool.Activate();
            Assert.True(tool.IsActive);
            Assert.True(tool.Activated);

            tool.Deactivate();
            Assert.False(tool.IsActive);
            Assert.True(tool.Deactivated);
        }

        [Fact]
        public void BaseTool_Reset_Calls_OnReset()
        {
            var tool = new DummyTool("dummy", "Dummy Tool");
            Assert.False(tool.ResetCalled);

            tool.Reset();
            Assert.True(tool.ResetCalled);
        }

        [Fact]
        public void IDrawingTool_Methods_Are_Invokable()
        {
            var tool = new DummyDrawingTool("draw", "Drawing Tool");
            tool.OnPointerDown(1, 2);
            tool.OnPointerMove(3, 4);
            tool.OnPointerUp(5, 6);

            Assert.True(tool.PointerDown);
            Assert.True(tool.PointerMove);
            Assert.True(tool.PointerUp);
        }

        [Fact]
        public void Tool_Properties_Are_Set_Correctly()
        {
            var tool = new DummyTool("id1", "Test Tool");
            Assert.Equal("id1", tool.Id);
            Assert.Equal("Test Tool", tool.DisplayName);
        }
    }
}