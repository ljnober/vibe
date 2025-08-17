// AvaloniaAsyncDrawing/Views/DrawingCanvasView.cs
// 核心自定义绘图控件，支持 MVVM 绑定与异步/同步渲染。

using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using SkiaSharp;
using Avalonia.Controls.Presenters;
using Avalonia.Visuals;
using AvaloniaAsyncDrawing.ViewModels;
using AvaloniaAsyncDrawing.Models;
using AvaloniaAsyncDrawing.Drawing;

namespace AvaloniaAsyncDrawing.Views
{
    /// <summary>
    /// 自定义绘图控件，支持依赖属性声明、MVVM 绑定、数据驱动 UI 响应、对接 Drawing 层渲染接口。
    /// </summary>
    public class DrawingCanvasView : Control, ICanvasElement, IRenderable
    {
        // 依赖属性：ViewModel，支持 MVVM 绑定
        public static readonly StyledProperty<CanvasViewModel> ViewModelProperty =
            AvaloniaProperty.Register<DrawingCanvasView, CanvasViewModel>(nameof(ViewModel));

        /// <summary>
        /// 绑定的 ViewModel，驱动 UI 响应。
        /// </summary>
        public CanvasViewModel ViewModel
        {
            get => GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        // 依赖属性：缩放比例
        public static readonly StyledProperty<double> ZoomProperty =
            AvaloniaProperty.Register<DrawingCanvasView, double>(nameof(Zoom), 1.0);

        /// <summary>
        /// 当前缩放比例，支持数据绑定。
        /// </summary>
        public double Zoom
        {
            get => GetValue(ZoomProperty);
            set => SetValue(ZoomProperty, value);
        }

        // 依赖属性：偏移量
        public static readonly StyledProperty<SKPoint> OffsetProperty =
            AvaloniaProperty.Register<DrawingCanvasView, SKPoint>(nameof(Offset), new SKPoint(0, 0));

        /// <summary>
        /// 当前画布偏移量，支持数据绑定。
        /// </summary>
        public SKPoint Offset
        {
            get => GetValue(OffsetProperty);
            set => SetValue(OffsetProperty, value);
        }

        // ICanvasElement 实现
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public bool Visible { get; set; } = true;

        // 渲染缓存与调度
        private SKImage? _offscreenCache;
        private int _cacheVersion;
        private readonly object _cacheLock = new object();

        /// <summary>
        /// 构造函数，初始化绑定与事件。
        /// </summary>
        public DrawingCanvasView()
        {
            this.GetObservable(ViewModelProperty).Subscribe(new AnonymousObserver<CanvasViewModel>(vm =>
            {
                if (vm != null)
                {
                    vm.PropertyChanged += (s, e) =>
                    {
                        if (e.PropertyName == nameof(CanvasViewModel.Layers) ||
                            e.PropertyName == nameof(CanvasViewModel.Zoom) ||
                            e.PropertyName == nameof(CanvasViewModel.Offset))
                        {
                            InvalidateVisual();
                        }
                    };
                }
            }));
        }

        /// <summary>
        /// MVVM 数据驱动的 UI 响应，属性变更时刷新。
        /// </summary>
        protected void OnPropertyChanged<T>(AvaloniaPropertyChangedEventArgs<T> change)
        {
            if (change.Property == ZoomProperty || change.Property == OffsetProperty)
            {
                InvalidateVisual();
            }
        }

        /// <summary>
        /// Avalonia 渲染入口，UI 线程同步调用，调用 Drawing 层接口。
        /// </summary>
        public override void Render(DrawingContext context)
        {
            SKImage? cache;
            int version;
            lock (_cacheLock)
            {
                cache = _offscreenCache;
                version = _cacheVersion;
            }
            if (cache != null)
            {
                context.Custom(new SkiaImageDrawOperation(cache, new Avalonia.Rect(0, 0, Bounds.Width, Bounds.Height)));
            }
        }

        /// <summary>
        /// 触发异步渲染，数据变更时调用。
        /// </summary>
        public async void BeginAsyncRender()
        {
            if (ViewModel == null) return;
            var layersSnapshot = ViewModel.Layers.ToList();
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                var info = new SKImageInfo((int)Bounds.Width, (int)Bounds.Height, SKImageInfo.PlatformColorType, SKAlphaType.Premul);
                using var surface = SKSurface.Create(info);
                var canvas = surface.Canvas;
                canvas.Clear(SKColors.Transparent);
                canvas.Save();
                canvas.Scale((float)Zoom);
                canvas.Translate(Offset.X, Offset.Y);
                foreach (var layer in layersSnapshot)
                {
                    layer.Render(canvas);
                }
                canvas.Restore();
                lock (_cacheLock)
                {
                    _offscreenCache?.Dispose();
                    _offscreenCache = surface.Snapshot();
                    _cacheVersion++;
                }
                InvalidateVisual();
            });
        }

        /// <summary>
        /// ICanvasElement/IRenderable 渲染接口实现（供多态调用）。
        /// </summary>
        public void Render(SKCanvas canvas)
        {
            if (_offscreenCache != null)
            {
                canvas.DrawImage(_offscreenCache, 0, 0);
            }
            else if (ViewModel != null)
            {
                foreach (var layer in ViewModel.Layers)
                {
                    layer.Render(canvas);
                }
            }
        }
    }

    // 辅助类：将 Action<T> 转为 IObserver<T>
    internal class AnonymousObserver<T> : IObserver<T>
    {
        private readonly Action<T> _onNext;
        public AnonymousObserver(Action<T> onNext) => _onNext = onNext;
        public void OnNext(T value) => _onNext(value);
        public void OnError(Exception error) { }
        public void OnCompleted() { }
    }
}