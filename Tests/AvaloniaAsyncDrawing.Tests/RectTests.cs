// Tests/AvaloniaAsyncDrawing.Tests/RectTests.cs
using Xunit;
using SkiaSharp;
using AvaloniaAsyncDrawing.Models;

namespace AvaloniaAsyncDrawing.Tests
{
    public class RectTests
    {
        [Fact]
        public void Rect_Property_Set_Get_Works()
        {
            var rect = new Rect
            {
                TopLeft = new SKPoint(10, 20),
                Width = 100,
                Height = 50,
                FillColor = SKColors.Red,
                StrokeColor = SKColors.Blue,
                StrokeWidth = 2.5f,
                Visible = false
            };

            Assert.Equal(new SKPoint(10, 20), rect.TopLeft);
            Assert.Equal(100, rect.Width);
            Assert.Equal(50, rect.Height);
            Assert.Equal(SKColors.Red, rect.FillColor);
            Assert.Equal(SKColors.Blue, rect.StrokeColor);
            Assert.Equal(2.5f, rect.StrokeWidth);
            Assert.False(rect.Visible);
        }

        [Fact]
        public void Rect_Render_Does_Not_Throw()
        {
            var rect = new Rect
            {
                TopLeft = new SKPoint(0, 0),
                Width = 10,
                Height = 10,
                FillColor = SKColors.Green,
                StrokeColor = SKColors.Black,
                StrokeWidth = 1f,
                Visible = true
            };
            using var bitmap = new SKBitmap(20, 20);
            using var canvas = new SKCanvas(bitmap);
            var ex = Record.Exception(() => rect.Render(canvas));
            Assert.Null(ex);
        }

        [Fact]
        public void Rect_HitTest_Works()
        {
            var rect = new Rect
            {
                TopLeft = new SKPoint(5, 5),
                Width = 10,
                Height = 10,
                Visible = true
            };
            // 命中
            Assert.True(rect.HitTest(new SKPoint(10, 10)));
            // 边界外
            Assert.False(rect.HitTest(new SKPoint(0, 0)));
            // 不可见
            rect.Visible = false;
            Assert.False(rect.HitTest(new SKPoint(10, 10)));
        }

        [Fact]
        public void Rect_Clone_Produces_Deep_Copy()
        {
            var rect = new Rect
            {
                TopLeft = new SKPoint(1, 2),
                Width = 3,
                Height = 4,
                FillColor = SKColors.Yellow,
                StrokeColor = SKColors.Purple,
                StrokeWidth = 5f,
                Visible = true
            };
            var clone = (Rect)rect.Clone();
            Assert.NotEqual(rect.Id, clone.Id);
            Assert.Equal(rect.TopLeft, clone.TopLeft);
            Assert.Equal(rect.Width, clone.Width);
            Assert.Equal(rect.Height, clone.Height);
            Assert.Equal(rect.FillColor, clone.FillColor);
            Assert.Equal(rect.StrokeColor, clone.StrokeColor);
            Assert.Equal(rect.StrokeWidth, clone.StrokeWidth);
            Assert.Equal(rect.Visible, clone.Visible);
            Assert.Equal(rect.Transform, clone.Transform);
        }

        [Fact]
        public void Rect_Serialize_And_Deserialize_Works()
        {
            var rect = new Rect
            {
                TopLeft = new SKPoint(7, 8),
                Width = 9,
                Height = 10,
                FillColor = SKColors.Orange,
                StrokeColor = SKColors.Brown,
                StrokeWidth = 2f,
                Visible = true
            };
            var json = rect.Serialize();
            var rect2 = Rect.Deserialize(json);
            Assert.NotNull(rect2);
            Assert.Equal(rect.TopLeft, rect2!.TopLeft);
            Assert.Equal(rect.Width, rect2.Width);
            Assert.Equal(rect.Height, rect2.Height);
            Assert.Equal(rect.FillColor, rect2.FillColor);
            Assert.Equal(rect.StrokeColor, rect2.StrokeColor);
            Assert.Equal(rect.StrokeWidth, rect2.StrokeWidth);
            Assert.Equal(rect.Visible, rect2.Visible);
        }
    }
}