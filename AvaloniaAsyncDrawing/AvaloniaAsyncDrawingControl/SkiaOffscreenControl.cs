using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using SkiaSharp;
using System.Collections.Generic;

namespace AvaloniaAsyncDrawingControl
{
    public class SkiaOffscreenControl : Control
    {
        public IList<LayerData> Layers
        {
            get => GetValue(LayersProperty);
            set => SetValue(LayersProperty, value);
        }
        public static readonly StyledProperty<IList<LayerData>> LayersProperty =
            AvaloniaProperty.Register<SkiaOffscreenControl, IList<LayerData>>(nameof(Layers), new List<LayerData>());

        public SkiaOffscreenControl()
        {
            this.PropertyChanged += (s, e) =>
            {
                Console.WriteLine($"[PropertyChanged] property={e.Property?.Name}");
                if (e.Property == LayersProperty)
                    this.InvalidateVisual();
                if (e.Property == RenderTransformProperty)
                    this.InvalidateVisual();
            };
        }

        public override void Render(DrawingContext context)
        {
            var pixelSize = new PixelSize((int)Bounds.Width, (int)Bounds.Height);
            using var surface = SKSurface.Create(new SKImageInfo(pixelSize.Width, pixelSize.Height));
            var canvas = surface.Canvas;
            canvas.Clear(SKColors.LightGray);

            // 统一通过 RenderTransform.Value.Matrix 获取缩放和平移分量
            double zoom = 1.0;
            double offsetX = 0, offsetY = 0;
            if (this.RenderTransform != null)
            {
                var m = this.RenderTransform.Value;
                zoom = m.M11;
                offsetX = m.M31;
                offsetY = m.M32;
            }

            Console.WriteLine($"[Render] zoom={zoom}, offset=({offsetX},{offsetY}), Layers={Layers?.Count ?? 0}");

            canvas.Save();
            canvas.Scale((float)zoom);
            canvas.Translate((float)(offsetX / zoom), (float)(offsetY / zoom));

            foreach (var layer in Layers)
            {
                if (!layer.IsVisible) continue;

                // Draw images（像素块放大，目标区域按缩放因子放大，FilterQuality.None，禁用抗锯齿）
                foreach (var img in layer.Images)
                {
                    var destRect = new SKRect(0, 0, img.Width, img.Height);
                    var paint = new SKPaint { FilterQuality = SKFilterQuality.None, IsAntialias = false };
                    Console.WriteLine($"[DrawBitmap] size={img.Width}x{img.Height}, zoom={zoom}, destRect={destRect}");
                    canvas.DrawBitmap(img, destRect, paint);
                }

                // Draw geometries（线宽随缩放变化，视觉粗细不变）
                foreach (var geo in layer.Geometries)
                {
                    var paint = new SKPaint
                    {
                        Color = geo.Color,
                        Style = SKPaintStyle.Stroke,
                        StrokeWidth = (float)(geo.StrokeWidth / zoom),
                        IsAntialias = true
                    };
                    if (geo.Color == SKColors.Red && geo.StrokeWidth == 4)
                        paint.PathEffect = SKPathEffect.CreateDash(new float[] { 10, 5 }, 0);
                    var scaledPath = geo.Path;
                    Console.WriteLine($"[DrawPath] color={geo.Color}, stroke={paint.StrokeWidth}, zoom={zoom}");
                    canvas.DrawPath(scaledPath, paint);
                }

                // Draw texts（坐标原始，无需加偏移）
                foreach (var txt in layer.Texts)
                {
                    var paint = txt.Paint.Clone();
                    var font = new SKFont();
                    var scaledPos = new SKPoint(txt.Position.X, txt.Position.Y);
                    Console.WriteLine($"[DrawText] text={txt.Text}, pos={scaledPos}, zoom={zoom}");
                    canvas.DrawText(txt.Text, scaledPos.X, scaledPos.Y, SKTextAlign.Left, font, paint);
                }
            }
            canvas.Restore();

            using var snapshot = surface.Snapshot();
            using var data = snapshot.Encode(SKEncodedImageFormat.Png, 100);
            var skBitmap = SKBitmap.Decode(data.AsStream());

            using (var ms = new System.IO.MemoryStream())
            {
                skBitmap.Encode(ms, SKEncodedImageFormat.Png, 100);
                ms.Seek(0, System.IO.SeekOrigin.Begin);
                var avaloniaBitmap = new Avalonia.Media.Imaging.Bitmap(ms);
                var destRect = new Rect(0, 0, Bounds.Width, Bounds.Height);
                context.DrawImage(avaloniaBitmap, destRect);
            }
        }

        public class LayerData
        {
            public string Name { get; set; } = string.Empty;
            public bool IsVisible { get; set; } = true;
            public IList<SKBitmap> Images { get; set; } = new List<SKBitmap>();
            public IList<GeometryData> Geometries { get; set; } = new List<GeometryData>();
            public IList<TextData> Texts { get; set; } = new List<TextData>();
        }

        public class GeometryData
        {
            public SKPath Path { get; set; } = new SKPath();
            public SKColor Color { get; set; } = SKColors.Black;
            public float StrokeWidth { get; set; } = 1.0f;
        }

        public class TextData
        {
            public string Text { get; set; } = string.Empty;
            public SKPoint Position { get; set; } = new SKPoint();
            public SKPaint Paint { get; set; } = new SKPaint();
        }
    }
}