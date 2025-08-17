// Tests/AvaloniaAsyncDrawing.Tests/LayerManagerTests.cs
using Xunit;
using SkiaSharp;
using AvaloniaAsyncDrawing.Models;
using System.Linq;

namespace AvaloniaAsyncDrawing.Tests
{
    public class LayerManagerTests
    {
        [Fact]
        public void AddAndRemoveLayer_WorksCorrectly()
        {
            var manager = new LayerManager();
            var layer = new LayerData { Id = "layer1", Visible = true, ZIndex = 0 };
            manager.AddLayer(layer);
            Assert.Single(manager.Layers);
            manager.RemoveLayer(layer);
            Assert.Empty(manager.Layers);
        }

        [Fact]
        public void SetLayerZIndex_ChangesOrder()
        {
            var manager = new LayerManager();
            var l1 = new LayerData { Id = "l1", Visible = true, ZIndex = 0 };
            var l2 = new LayerData { Id = "l2", Visible = true, ZIndex = 1 };
            manager.AddLayer(l1);
            manager.AddLayer(l2);
            manager.SetLayerZIndex(l1, 2);
            Assert.Equal(2, l1.ZIndex);
            Assert.Equal(new[] { l2, l1 }, manager.Layers);
        }

        [Fact]
        public void HitTestAll_ReturnsCorrectElements()
        {
            var manager = new LayerManager();
            var layer = new LayerData { Id = "layer", Visible = true, ZIndex = 0 };
            var rectPoints = new[] {
                new SKPoint(0, 0),
                new SKPoint(100, 0),
                new SKPoint(100, 100),
                new SKPoint(0, 100)
            };
            var rectData = new GeometryData
            {
                Id = "rect1",
                Visible = true,
                Type = "Rectangle",
                Points = rectPoints
            };
            layer.AddGeometry(rectData);
            manager.AddLayer(layer);

            var hits = manager.HitTestAll(new SKPoint(50, 50));
            Assert.Single(hits);
            Assert.Equal(rectData, hits[0].Element);
        }

        [Fact]
        public void HitTestTop_ReturnsTopElement()
        {
            var manager = new LayerManager();
            var l1 = new LayerData { Id = "l1", Visible = true, ZIndex = 0 };
            var l2 = new LayerData { Id = "l2", Visible = true, ZIndex = 1 };
            var rect1Points = new[] {
                new SKPoint(0, 0),
                new SKPoint(100, 0),
                new SKPoint(100, 100),
                new SKPoint(0, 100)
            };
            var rect2Points = new[] {
                new SKPoint(0, 0),
                new SKPoint(100, 0),
                new SKPoint(100, 100),
                new SKPoint(0, 100)
            };
            var rectData1 = new GeometryData
            {
                Id = "rect1",
                Visible = true,
                Type = "Rectangle",
                Points = rect1Points
            };
            var rectData2 = new GeometryData
            {
                Id = "rect2",
                Visible = true,
                Type = "Rectangle",
                Points = rect2Points
            };
            l1.AddGeometry(rectData1);
            l2.AddGeometry(rectData2);
            manager.AddLayer(l1);
            manager.AddLayer(l2);

            var top = manager.HitTestTop(new SKPoint(50, 50));
            Assert.NotNull(top);
            Assert.Equal(l2, top?.Layer);
            Assert.Equal(rectData2, top?.Element);
        }
    }
}