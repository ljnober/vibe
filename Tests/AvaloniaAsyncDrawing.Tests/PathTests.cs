// Tests/PathTests.cs
using Xunit;
using AvaloniaAsyncDrawing.Models;
using PathAlias = AvaloniaAsyncDrawing.Models.Path;
using SkiaSharp;

namespace AvaloniaAsyncDrawing.Tests
{
    public class PathTests
    {
        [Fact]
        public void Path_CanInstantiateAndSetProperties()
        {
            var path = new PathAlias
            {
                Id = "p1",
                Visible = true,
                FillColor = SKColors.Red,
                StrokeColor = SKColors.Blue,
                StrokeWidth = 2.5f
            };
            path.Commands.Add(new PathCommand(PathCommandType.MoveTo, new[] { new SKPoint(0, 0) }));
            path.Commands.Add(new PathCommand(PathCommandType.LineTo, new[] { new SKPoint(10, 0) }));
            path.Commands.Add(new PathCommand(PathCommandType.LineTo, new[] { new SKPoint(10, 10) }));
            path.Commands.Add(new PathCommand(PathCommandType.Close, Array.Empty<SKPoint>()));

            Assert.Equal("p1", path.Id);
            Assert.True(path.Visible);
            Assert.Equal(SKColors.Red, path.FillColor);
            Assert.Equal(SKColors.Blue, path.StrokeColor);
            Assert.Equal(2.5f, path.StrokeWidth);
            Assert.Equal(3, path.Points.Count);
            Assert.Equal(new SKPoint(0, 0), path.Points[0]);
        }

        [Fact]
        public void Path_HitTest_Works()
        {
            var path = new PathAlias
            {
                FillColor = SKColors.Green,
                StrokeColor = SKColors.Black,
                StrokeWidth = 2
            };
            path.Commands.Add(new PathCommand(PathCommandType.MoveTo, new[] { new SKPoint(0, 0) }));
            path.Commands.Add(new PathCommand(PathCommandType.LineTo, new[] { new SKPoint(20, 0) }));
            path.Commands.Add(new PathCommand(PathCommandType.LineTo, new[] { new SKPoint(20, 20) }));
            path.Commands.Add(new PathCommand(PathCommandType.LineTo, new[] { new SKPoint(0, 20) }));
            path.Commands.Add(new PathCommand(PathCommandType.Close, Array.Empty<SKPoint>()));

            Assert.True(path.HitTest(new SKPoint(10, 10))); // 内部
            Assert.False(path.HitTest(new SKPoint(30, 30))); // 外部
        }

        [Fact]
        public void Path_Serialization_And_Deserialization_Works()
        {
            var path = new PathAlias
            {
                FillColor = SKColors.Yellow,
                StrokeColor = SKColors.Purple,
                StrokeWidth = 3
            };
            path.Commands.Add(new PathCommand(PathCommandType.MoveTo, new[] { new SKPoint(1, 1) }));
            path.Commands.Add(new PathCommand(PathCommandType.LineTo, new[] { new SKPoint(5, 1) }));
            path.Commands.Add(new PathCommand(PathCommandType.LineTo, new[] { new SKPoint(5, 5) }));
            path.Commands.Add(new PathCommand(PathCommandType.Close, Array.Empty<SKPoint>()));

            var json = path.Serialize();
            var parsed = PathAlias.Deserialize(json)!;
            Assert.Equal(path.FillColor, parsed.FillColor);
            Assert.Equal(path.StrokeColor, parsed.StrokeColor);
            Assert.Equal(path.StrokeWidth, parsed.StrokeWidth);
            Assert.Equal(path.Commands.Count, parsed.Commands.Count);
            Assert.Equal(path.Commands[0].Type, parsed.Commands[0].Type);
            Assert.Equal(path.Commands[0].Points[0], parsed.Commands[0].Points[0]);
        }

        [Fact]
        public void Path_Clone_Works()
        {
            var path = new PathAlias
            {
                Id = "clone",
                Visible = false,
                FillColor = SKColors.Orange,
                StrokeColor = SKColors.Brown,
                StrokeWidth = 4
            };
            path.Commands.Add(new PathCommand(PathCommandType.MoveTo, new[] { new SKPoint(2, 2) }));
            path.Commands.Add(new PathCommand(PathCommandType.LineTo, new[] { new SKPoint(8, 2) }));
            path.Commands.Add(new PathCommand(PathCommandType.Close, Array.Empty<SKPoint>()));

            var clone = path.Clone();
            Assert.Equal(path.Id, clone.Id);
            Assert.Equal(path.Visible, clone.Visible);
            Assert.Equal(path.FillColor, clone.FillColor);
            Assert.Equal(path.StrokeColor, clone.StrokeColor);
            Assert.Equal(path.StrokeWidth, clone.StrokeWidth);
            Assert.Equal(path.Commands.Count, clone.Commands.Count);
            Assert.Equal(path.Commands[0].Type, clone.Commands[0].Type);
            Assert.Equal(path.Commands[0].Points[0], clone.Commands[0].Points[0]);
        }
    }
}