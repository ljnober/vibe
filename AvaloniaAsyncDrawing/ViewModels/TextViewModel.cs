// AvaloniaAsyncDrawing/ViewModels/TextViewModel.cs
using SkiaSharp;
using AvaloniaAsyncDrawing.Models;

namespace AvaloniaAsyncDrawing.ViewModels
{
    /// <summary>
    /// 文本对象视图模型，支持属性通知与 Model 绑定。
    /// </summary>
    public class TextViewModel : BaseViewModel
    {
        private readonly TextData _model;
        public TextData Model => _model;

        public TextViewModel(TextData model)
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

        public string Content
        {
            get => _model.Content;
            set { if (_model.Content != value) { _model.Content = value; OnPropertyChanged(nameof(Content)); } }
        }

        // 其它属性如 Font、Position、Color 可按需扩展并实现通知

        public void Render(SKCanvas canvas)
        {
            Model.Render(canvas);
        }
    }
}