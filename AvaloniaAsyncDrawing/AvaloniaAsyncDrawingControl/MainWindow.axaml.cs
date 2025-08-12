using Avalonia.Controls;
using SkiaSharp;
using System.Collections.Generic;
using System.Net.Http;
using static AvaloniaAsyncDrawingControl.SkiaOffscreenControl;

namespace AvaloniaAsyncDrawingControl
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // 下载网络图片
            SKBitmap netBitmap = null;
            using (var httpClient = new HttpClient())
            {
                var imgBytes = httpClient.GetByteArrayAsync("https://images.unsplash.com/photo-1506744038136-46273834b3fb?auto=format&fit=crop&w=600&q=80").Result;
                using (var ms = new System.IO.MemoryStream(imgBytes))
                {
                    netBitmap = SKBitmap.Decode(ms);
                }
            }

            // 虚线几何
            var dashRect = new SKPath();
            dashRect.AddRect(new SKRect(50, 50, 550, 350));

            var layer = new LayerData
            {
                Name = "Demo",
                IsVisible = true,
                Images = new List<SKBitmap> { netBitmap },
                Geometries = new List<GeometryData>
                {
                    new GeometryData
                    {
                        Path = dashRect,
                        Color = SKColors.Red,
                        StrokeWidth = 4
                    }
                },
                Texts = new List<TextData>()
            };

            SkiaDemo.Layers = new List<LayerData> { layer };
        }
    }
}