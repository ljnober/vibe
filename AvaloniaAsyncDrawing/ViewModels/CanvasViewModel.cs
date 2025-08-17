// CanvasViewModel.cs
using System.Collections.ObjectModel;

namespace AvaloniaAsyncDrawing.ViewModels
{
    /// <summary>
    /// 画布视图模型，支持图层集合、缩放、偏移等属性，供 DrawingCanvasView 绑定。
    /// </summary>
    public class CanvasViewModel : BaseViewModel
    {
        public ObservableCollection<LayerViewModel> Layers { get; set; } = new ObservableCollection<LayerViewModel>();
        public double Zoom { get; set; } = 1.0;
        public double Offset { get; set; } = 0.0;
    }
}