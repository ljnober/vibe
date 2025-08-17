// AvaloniaAsyncDrawing/ViewModels/GeometryViewModel.cs
using SkiaSharp;
using AvaloniaAsyncDrawing.Models;

namespace AvaloniaAsyncDrawing.ViewModels
{
    /// <summary>
    /// 几何对象视图模型，支持属性通知与 Model 绑定。
    /// </summary>
    public class GeometryViewModel : BaseViewModel
    {
        private readonly GeometryData _model;
        public GeometryData Model => _model;

        public GeometryViewModel(GeometryData model)
        {
            _model = model;
        }

        public string Id
        {
            get => _model.Id;
            set { if (_model.Id != value) { _model.Id = value; OnPropertyChanged(nameof(Id)); } }
        }

        public bool Visible
        {
            get => _model.Visible;
            set { if (_model.Visible != value) { _model.Visible = value; OnPropertyChanged(nameof(Visible)); } }
        }

        public string Type
        {
            get => _model.Type;
            set { if (_model.Type != value) { _model.Type = value; OnPropertyChanged(nameof(Type)); } }
        }

        // 其它属性如 Points、Path、FillPaint、StrokePaint、Transform 可按需扩展并实现通知

        public void Render(SKCanvas canvas)
        {
            Model.Render(canvas);
        }
    }
}