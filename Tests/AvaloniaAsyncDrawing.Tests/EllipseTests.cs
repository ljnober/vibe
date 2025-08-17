// Tests/AvaloniaAsyncDrawing.Tests/EllipseTests.cs
using Xunit;
using SkiaSharp;
using AvaloniaAsyncDrawing.Models;

namespace AvaloniaAsyncDrawing.Tests
{
    public class EllipseTests
    {
        [Fact]
        public void Ellipse_Property_Set_Get_Works()
        {
            var ellipse = new Ellipse
            {
                Center = new SKPoint(50, 60),
                RadiusX = 30,
                RadiusY = 20,
                FillColor = SKColors.Red,
                StrokeColor = SKColors.Blue,
                StrokeWidth = 2.5f,
                Visible = false
            };

            Assert.Equal(new SKPoint(50, 60), ellipse.Center);
            Assert.Equal(30, ellipse.RadiusX);
            Assert.Equal(20, ellipse.RadiusY);
            Assert.Equal(SKColors.Red, ellipse.FillColor);
            Assert.Equal(SKColors.Blue, ellipse.StrokeColor);
            Assert.Equal(2.5f, ellipse.StrokeWidth);
            Assert.False(ellipse.Visible);
        }

        [Fact]
        public void Ellipse_Render_Does_Not_Throw()
        {
            var ellipse = new Ellipse
            {
                Center = new SKPoint(20, 20),
                RadiusX = 10,
                RadiusY = 5,
                FillColor = SKColors.Green,
                StrokeColor = SKColors.Black,
                StrokeWidth = 1f,
                Visible = true
            };
            using var bitmap = new SKBitmap(40, 40);
            using var canvas = new SKCanvas(bitmap);
            var ex = Record.Exception(() => ellipse.Render(canvas));
            Assert.Null(ex);
        }

        [Fact]
        public void Ellipse_HitTest_Works()
        {
            var ellipse = new Ellipse
            {
                Center = new SKPoint(10, 10),
                RadiusX = 5,
                RadiusY = 3,
                Visible = true
            };
            // 命中
            Assert.True(ellipse.HitTest(new SKPoint(10, 10)));
            // 边界外
            Assert.False(ellipse.HitTest(new SKPoint(0, 0)));
            // 不可见
            ellipse.Visible = false;
            Assert.False(ellipse.HitTest(new SKPoint(10, 10)));
        }

        [Fact]
        public void Ellipse_Clone_Produces_Deep_Copy()
        {
            var ellipse = new Ellipse
            {
                Center = new SKPoint(1, 2),
                RadiusX = 3,
                RadiusY = 4,
                FillColor = SKColors.Yellow,
                StrokeColor = SKColors.Purple,
                StrokeWidth = 5f,
                Visible = true
            };
            var clone = (Ellipse)ellipse.Clone();
            Assert.NotEqual(ellipse.Id, clone.Id);
            Assert.Equal(ellipse.Center, clone.Center);
            Assert.Equal(ellipse.RadiusX, clone.RadiusX);
            Assert.Equal(ellipse.RadiusY, clone.RadiusY);
            Assert.Equal(ellipse.FillColor, clone.FillColor);
            Assert.Equal(ellipse.StrokeColor, clone.StrokeColor);
            Assert.Equal(ellipse.StrokeWidth, clone.StrokeWidth);
            Assert.Equal(ellipse.Visible, clone.Visible);
            Assert.Equal(ellipse.Transform, clone.Transform);
        }

        [Fact]
        public void Ellipse_Serialize_And_Deserialize_Works()
        {
            var ellipse = new Ellipse
            {
                Center = new SKPoint(7, 8),
                RadiusX = 9,
                RadiusY = 10,
                FillColor = SKColors.Orange,
                StrokeColor = SKColors.Brown,
                StrokeWidth = 2f,
                Visible = true
            };
            var json = ellipse.Serialize();
            var ellipse2 = Ellipse.Deserialize(json);
            Assert.NotNull(ellipse2);
            Assert.Equal(ellipse.Center, ellipse2!.Center);
            Assert.Equal(ellipse.RadiusX, ellipse2.RadiusX);
            Assert.Equal(ellipse.RadiusY, ellipse2.RadiusY);
            Assert.Equal(ellipse.FillColor, ellipse2.FillColor);
            Assert.Equal(ellipse.StrokeColor, ellipse2.StrokeColor);
            Assert.Equal(ellipse.StrokeWidth, ellipse2.StrokeWidth);
            Assert.Equal(ellipse.Visible, ellipse2.Visible);
        }
    }
}