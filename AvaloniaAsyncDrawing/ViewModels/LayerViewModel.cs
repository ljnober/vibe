// AvaloniaAsyncDrawing/ViewModels/LayerViewModel.cs
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using SkiaSharp;
using AvaloniaAsyncDrawing.Models;

namespace AvaloniaAsyncDrawing.ViewModels
{
    /// <summary>
    /// 图层视图模型，支持属性通知、命令、与 LayerData 的同步。
    /// </summary>
    public class LayerViewModel : BaseViewModel
    {
        private string _id;
        private bool _visible;
        private int _zIndex;

        public string Id
        {
            get => _id;
            set { if (_id != value) { _id = value; OnPropertyChanged(nameof(Id)); } }
        }

        public bool Visible
        {
            get => _visible;
            set { if (_visible != value) { _visible = value; OnPropertyChanged(nameof(Visible)); } }
        }

        public int ZIndex
        {
            get => _zIndex;
            set { if (_zIndex != value) { _zIndex = value; OnPropertyChanged(nameof(ZIndex)); } }
        }

        public ObservableCollection<GeometryViewModel> Geometries { get; }
        public ObservableCollection<TextViewModel> Texts { get; }
        public ObservableCollection<ImageViewModel> Images { get; }

        private GeometryViewModel? _selectedGeometry;
        public GeometryViewModel? SelectedGeometry
        {
            get => _selectedGeometry;
            set { if (_selectedGeometry != value) { _selectedGeometry = value; OnPropertyChanged(nameof(SelectedGeometry)); } }
        }

        private TextViewModel? _selectedText;
        public TextViewModel? SelectedText
        {
            get => _selectedText;
            set { if (_selectedText != value) { _selectedText = value; OnPropertyChanged(nameof(SelectedText)); } }
        }

        private ImageViewModel? _selectedImage;
        public ImageViewModel? SelectedImage
        {
            get => _selectedImage;
            set { if (_selectedImage != value) { _selectedImage = value; OnPropertyChanged(nameof(SelectedImage)); } }
        }

        public ICommand AddGeometryCommand { get; }
        public ICommand RemoveGeometryCommand { get; }
        public ICommand AddTextCommand { get; }
        public ICommand RemoveTextCommand { get; }
        public ICommand AddImageCommand { get; }
        public ICommand RemoveImageCommand { get; }
        public ICommand SelectGeometryCommand { get; }
        public ICommand SelectTextCommand { get; }
        public ICommand SelectImageCommand { get; }

        public LayerData Model { get; }

        public LayerViewModel(LayerData model)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            _id = model.Id;
            _visible = model.Visible;
            _zIndex = model.ZIndex;

            Geometries = new ObservableCollection<GeometryViewModel>(model.Geometries.Select(g => new GeometryViewModel(g)));
            Texts = new ObservableCollection<TextViewModel>(model.Texts.Select(t => new TextViewModel(t)));
            Images = new ObservableCollection<ImageViewModel>(model.Images.Select(i => new ImageViewModel(i)));

            AddGeometryCommand = new RelayCommand(_ => AddGeometry());
            RemoveGeometryCommand = new RelayCommand(_ => RemoveGeometry(SelectedGeometry), _ => SelectedGeometry != null);
            AddTextCommand = new RelayCommand(_ => AddText());
            RemoveTextCommand = new RelayCommand(_ => RemoveText(SelectedText), _ => SelectedText != null);
            AddImageCommand = new RelayCommand(_ => AddImage());
            RemoveImageCommand = new RelayCommand(_ => RemoveImage(SelectedImage), _ => SelectedImage != null);

            SelectGeometryCommand = new RelayCommand(g => SelectedGeometry = g as GeometryViewModel);
            SelectTextCommand = new RelayCommand(t => SelectedText = t as TextViewModel);
            SelectImageCommand = new RelayCommand(i => SelectedImage = i as ImageViewModel);

            Geometries.CollectionChanged += (s, e) => SyncGeometries();
            Texts.CollectionChanged += (s, e) => SyncTexts();
            Images.CollectionChanged += (s, e) => SyncImages();
        }

        private void AddGeometry()
        {
            var geo = new GeometryData { Id = Guid.NewGuid().ToString(), Visible = true, Type = "Rectangle" };
            var vm = new GeometryViewModel(geo);
            Geometries.Add(vm);
            SelectedGeometry = vm;
        }

        private void RemoveGeometry(GeometryViewModel? vm)
        {
            if (vm != null) Geometries.Remove(vm);
            if (SelectedGeometry == vm) SelectedGeometry = null;
        }

        private void AddText()
        {
            var text = new TextData { Id = Guid.NewGuid().ToString(), Visible = true, Content = "文本" };
            var vm = new TextViewModel(text);
            Texts.Add(vm);
            SelectedText = vm;
        }

        private void RemoveText(TextViewModel? vm)
        {
            if (vm != null) Texts.Remove(vm);
            if (SelectedText == vm) SelectedText = null;
        }

        private void AddImage()
        {
            var img = new ImageData { Id = Guid.NewGuid().ToString(), Visible = true };
            var vm = new ImageViewModel(img);
            Images.Add(vm);
            SelectedImage = vm;
        }

        private void RemoveImage(ImageViewModel? vm)
        {
            if (vm != null) Images.Remove(vm);
            if (SelectedImage == vm) SelectedImage = null;
        }

        public void Render(SKCanvas canvas)
        {
            foreach (var geo in Geometries)
                geo.Render(canvas);
            foreach (var text in Texts)
                text.Render(canvas);
            foreach (var img in Images)
                img.Render(canvas);
        }

        private void SyncGeometries()
        {
            Model.Geometries.ToList().ForEach(g => Model.RemoveGeometry(g));
            foreach (var vm in Geometries)
                Model.AddGeometry(vm.Model);
        }

        private void SyncTexts()
        {
            Model.Texts.ToList().ForEach(t => Model.RemoveText(t));
            foreach (var vm in Texts)
                Model.AddText(vm.Model);
        }

        private void SyncImages()
        {
            Model.Images.ToList().ForEach(i => Model.RemoveImage(i));
            foreach (var vm in Images)
                Model.AddImage(vm.Model);
        }
    }
}