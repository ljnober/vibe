// AvaloniaAsyncDrawing/Tests/TypeSignatureTests.cs
// 针对 Drawing、Models、ViewModels、Views 层类型声明、接口签名、依赖关系的基础单元测试。
// 仅验证类型可被正确引用、实例化与接口实现，不涉及业务逻辑。

using Xunit;
using SkiaSharp;
using System;
using System.ComponentModel;
using AvaloniaAsyncDrawing.Drawing;
using AvaloniaAsyncDrawing.Models;
using AvaloniaAsyncDrawing.ViewModels;
using AvaloniaAsyncDrawing.Views;

namespace AvaloniaAsyncDrawing.Tests
{
    public class TypeSignatureTests
    {
        // Drawing 层
        [Fact]
        public void BaseDrawing_CanInstantiate()
        {
            var drawing = new BaseDrawing();
            Assert.NotNull(drawing);
        }

        [Fact]
        public void DrawContext_AbstractType_CanReference()
        {
            DrawContext? ctx = null;
            Assert.Null(ctx);
        }

        [Fact]
        public void ICustomDrawOperation_CanReferenceAndImplement()
        {
            var op = new DummyCustomDrawOperation();
            Assert.NotNull(op);
        }

        // Models 层
        [Fact]
        public void BaseModel_CanInstantiate()
        {
            var model = new BaseModel();
            Assert.NotNull(model);
        }

        [Fact]
        public void ICanvasElement_CanReferenceAndImplement()
        {
            var impl = new DummyCanvasElement();
            Assert.NotNull(impl);
        }

        [Fact]
        public void IRenderable_CanReferenceAndImplement()
        {
            var impl = new DummyRenderable();
            Assert.NotNull(impl);
        }

        [Fact]
        public void GeometryData_CanInstantiateAndSetProperties()
        {
            var geo = new GeometryData
            {
                Id = "g1",
                Visible = true,
                Type = "Rect",
                Points = new SKPoint[0],
                Path = null,
                FillPaint = null,
                StrokePaint = null,
                Transform = SKMatrix.CreateIdentity()
            };
            Assert.Equal("g1", geo.Id);
            Assert.True(geo.Visible);
        }

        [Fact]
        public void TextData_CanInstantiateAndSetProperties()
        {
            var text = new TextData
            {
                Id = "t1",
                Visible = false,
                Content = "abc",
                Font = null,
                Position = new SKPoint(1, 2),
                Color = SKColors.Black
            };
            Assert.Equal("t1", text.Id);
            Assert.False(text.Visible);
        }

        [Fact]
        public void ImageData_CanInstantiateAndSetProperties()
        {
            var img = new ImageData
            {
                Id = "img",
                Visible = true,
                Bitmap = null,
                DestRect = new SKRect(0, 0, 10, 10),
                Paint = null,
                Transform = SKMatrix.CreateIdentity()
            };
            Assert.Equal("img", img.Id);
            Assert.True(img.Visible);
        }

        // ViewModels 层
        [Fact]
        public void BaseViewModel_CanReferenceAndRaisePropertyChanged()
        {
            var vm = new DummyViewModel();
            bool raised = false;
            vm.PropertyChanged += (s, e) => raised = true;
            vm.RaiseTest();
            Assert.True(raised);
        }

        // Views 层
        [Fact]
        public void BaseView_AbstractType_CanReference()
        {
            BaseView? view = null;
            Assert.Null(view);
        }

        [Fact]
        public void Views_ICanvasElement_CanReferenceAndImplement()
        {
            var impl = new DummyViewsCanvasElement();
            Assert.NotNull(impl);
        }

        [Fact]
        public void Views_IRenderable_CanReferenceAndImplement()
        {
            var impl = new DummyViewsRenderable();
            Assert.NotNull(impl);
        }

        // Dummy 实现
        private class DummyCustomDrawOperation : ICustomDrawOperation
        {
            public Avalonia.Rect Bounds => new Avalonia.Rect();
            public void Dispose() { }
            public bool Equals(ICustomDrawOperation? other) => false;
            public bool HitTest(Avalonia.Point point) => false;
            public void Render(Avalonia.Media.ImmediateDrawingContext context) { }
        }

        private class DummyCanvasElement : ICanvasElement
        {
            public string Id => "dummy";
            public bool Visible => true;
            public void Render(SKCanvas canvas) { }
        }

        private class DummyRenderable : IRenderable
        {
            public void Render(SKCanvas canvas) { }
        }

        private class DummyViewModel : BaseViewModel
        {
            public void RaiseTest() => OnPropertyChanged("Test");
        }

        private class DummyViewsCanvasElement : AvaloniaAsyncDrawing.Models.ICanvasElement
        {
            public string Id { get; set; } = "dummy";
            public bool Visible { get; set; } = true;
            public void Render(SKCanvas canvas) { }
        }

        private class DummyViewsRenderable : AvaloniaAsyncDrawing.Models.IRenderable
        {
            public void Render(SKCanvas canvas) { }
        }
    }
}