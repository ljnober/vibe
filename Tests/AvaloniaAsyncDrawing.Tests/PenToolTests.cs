using Xunit;
using SkiaSharp;
using AvaloniaAsyncDrawing.Drawing;
using AvaloniaAsyncDrawing.Models;

namespace AvaloniaAsyncDrawing.Tests
{
    /// <summary>
    /// PenTool（画笔工具）核心行为单元测试。
    /// </summary>
    public class PenToolTests
    {
        [Fact]
        public void PenTool_Activation_State_Transitions()
        {
            var tool = new PenTool();
            Assert.False(tool.IsActive);

            tool.Activate();
            Assert.True(tool.IsActive);

            tool.Deactivate();
            Assert.False(tool.IsActive);
        }

        [Fact]
        public void PenTool_Pointer_Collects_Points()
        {
            var tool = new PenTool();
            tool.OnPointerDown(1, 2);
            tool.OnPointerMove(3, 4);
            tool.OnPointerMove(5, 6);
            tool.OnPointerUp(7, 8);

            Assert.Equal(4, tool.CurrentPoints.Count);
            Assert.Equal(new SKPoint(1, 2), tool.CurrentPoints[0]);
            Assert.Equal(new SKPoint(3, 4), tool.CurrentPoints[1]);
            Assert.Equal(new SKPoint(5, 6), tool.CurrentPoints[2]);
            Assert.Equal(new SKPoint(7, 8), tool.CurrentPoints[3]);
        }

        [Fact]
        public void PenTool_Reset_Clears_State()
        {
            var tool = new PenTool();
            tool.OnPointerDown(1, 2);
            tool.OnPointerMove(3, 4);
            tool.OnPointerUp(5, 6);

            Assert.True(tool.CurrentPoints.Count > 0);

            tool.Reset();
            Assert.Empty(tool.CurrentPoints);
        }

        [Fact]
        public void PenTool_ToPath_Returns_Path_When_Enough_Points()
        {
            var tool = new PenTool();
            tool.OnPointerDown(1, 2);
            tool.OnPointerMove(3, 4);
            tool.OnPointerUp(5, 6);

            var path = tool.ToPath();
            Assert.NotNull(path);
            Assert.True(path.Points.Count >= 3);
        }

        [Fact]
        public void PenTool_ToPath_Returns_Null_When_Not_Enough_Points()
        {
            var tool = new PenTool();
            tool.OnPointerDown(1, 2);
            tool.OnPointerUp(1, 2);

            var path = tool.ToPath();
            Assert.Null(path);
        }
    }
}