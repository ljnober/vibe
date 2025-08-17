// AvaloniaAsyncDrawing/ViewModels/ImageViewModel.cs
using SkiaSharp;
using AvaloniaAsyncDrawing.Models;

namespace AvaloniaAsyncDrawing.ViewModels
{
    /// <summary>
    /// 图片对象视图模型，支持属性通知与 Model 绑定。
    /// </summary>
    public class ImageViewModel : BaseViewModel
    {
        private readonly ImageData _model;
        public ImageData Model => _model;

        public ImageViewModel(ImageData model)
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

        // 其它属性如 Bitmap、DestRect、Paint、Transform 可按需扩展并实现通知

        public void Render(SKCanvas canvas)
        {
            Model.Render(canvas);
        }
    }
}