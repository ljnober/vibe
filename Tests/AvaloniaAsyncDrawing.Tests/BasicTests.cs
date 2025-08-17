// Tests/BasicTests.cs
// 该文件为测试用例示例，建议后续所有单元测试集中于此。

using Xunit;
using SkiaSharp;
using AvaloniaAsyncDrawing.Models;

namespace AvaloniaAsyncDrawing.Tests
{
    /// <summary>
    /// 基础测试类（示例）。
    /// </summary>
    public class BasicTests
    {
        [Fact]
        public void Line_CanInstantiateAndSetProperties()
        {
            var line = new Line
            {
                Id = "l1",
                Visible = true,
                Start = new SKPoint(1, 2),
                End = new SKPoint(3, 4),
                Color = SKColors.Red,
                StrokeWidth = 2.5f
            };
            Assert.Equal("l1", line.Id);
            Assert.True(line.Visible);
            Assert.Equal(new SKPoint(1, 2), line.Start);
            Assert.Equal(new SKPoint(3, 4), line.End);
            Assert.Equal(SKColors.Red, line.Color);
            Assert.Equal(2.5f, line.StrokeWidth);
        }

        [Fact]
        public void Line_HitTest_Works()
        {
            var line = new Line
            {
                Start = new SKPoint(0, 0),
                End = new SKPoint(10, 0),
                StrokeWidth = 1
            };
            Assert.True(line.HitTest(new SKPoint(5, 1)));
            Assert.False(line.HitTest(new SKPoint(5, 5)));
        }

        [Fact]
        public void Line_Serialization_And_Deserialization_Works()
        {
            var line = new Line
            {
                Start = new SKPoint(1, 2),
                End = new SKPoint(3, 4),
                Color = SKColors.Blue,
                StrokeWidth = 1.5f
            };
            var str = line.ToString();
            var parsed = Line.Parse(str);
            Assert.Equal(line.Start, parsed.Start);
            Assert.Equal(line.End, parsed.End);
            Assert.Equal(line.Color, parsed.Color);
            Assert.Equal(line.StrokeWidth, parsed.StrokeWidth);
        }

        [Fact]
        public void Line_Clone_Works()
        {
            var line = new Line
            {
                Id = "clone",
                Visible = false,
                Start = new SKPoint(1, 2),
                End = new SKPoint(3, 4),
                Color = SKColors.Green,
                StrokeWidth = 2f
            };
            var clone = line.Clone();
            Assert.Equal(line.Id, clone.Id);
            Assert.Equal(line.Visible, clone.Visible);
            Assert.Equal(line.Start, clone.Start);
            Assert.Equal(line.End, clone.End);
            Assert.Equal(line.Color, clone.Color);
            Assert.Equal(line.StrokeWidth, clone.StrokeWidth);
            Assert.Equal(line.Transform, clone.Transform);
        }

        [Fact]
        public void LayerViewModel_AddRemoveGeometryCommand_Works()
        {
            var layerData = new AvaloniaAsyncDrawing.Models.LayerData
            {
                Id = "layer1",
                Visible = true,
                ZIndex = 0
            };
            var vm = new AvaloniaAsyncDrawing.ViewModels.LayerViewModel(layerData);

            // 添加几何对象
            Assert.Empty(vm.Geometries);
            vm.AddGeometryCommand.Execute(null);
            Assert.Single(vm.Geometries);

            // 选择并移除
            var geo = vm.Geometries[0];
            vm.SelectedGeometry = geo;
            Assert.True(vm.RemoveGeometryCommand.CanExecute(null));
            vm.RemoveGeometryCommand.Execute(null);
            Assert.Empty(vm.Geometries);
        }

        [Fact]
        public void LayerViewModel_PropertyChanged_RaisesEvent()
        {
            var layerData = new AvaloniaAsyncDrawing.Models.LayerData
            {
                Id = "layer2",
                Visible = false,
                ZIndex = 1
            };
            var vm = new AvaloniaAsyncDrawing.ViewModels.LayerViewModel(layerData);
            bool raised = false;
            vm.PropertyChanged += (s, e) => { if (e.PropertyName == "Visible") raised = true; };
            vm.Visible = true;
            Assert.True(raised);
        }

        [Fact]
        public void RelayCommand_CanExecute_And_Execute_Works()
        {
            bool executed = false;
            var cmd = new AvaloniaAsyncDrawing.ViewModels.RelayCommand(_ => executed = true, _ => true);
            Assert.True(cmd.CanExecute(null));
            cmd.Execute(null);
            Assert.True(executed);
        }
    }
}