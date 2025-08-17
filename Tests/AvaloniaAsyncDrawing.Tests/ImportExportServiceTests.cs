// Tests/AvaloniaAsyncDrawing.Tests/ImportExportServiceTests.cs
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvaloniaAsyncDrawing.Models;
using AvaloniaAsyncDrawing.Utils;
using SkiaSharp;
using Xunit;

namespace AvaloniaAsyncDrawing.Tests
{
    public class ImportExportServiceTests
    {
        [Fact]
        public async Task ImportExport_Png_Works()
        {
            // 构造简单位图
            using var bitmap = new SKBitmap(10, 10);
            using var canvas = new SKCanvas(bitmap);
            canvas.Clear(SKColors.Red);

            var imageData = new ImageData
            {
                Bitmap = bitmap,
                DestRect = new SKRect(0, 0, 10, 10),
                Visible = true,
                Id = "test"
            };

            var service = new ImportExportService();
            using var ms = new MemoryStream();
            await service.ExportAsync(new[] { imageData }, ms, ImageFormat.Png);

            ms.Seek(0, SeekOrigin.Begin);
            var imported = await service.ImportAsync(ms, ImageFormat.Png);
            Assert.Single(imported);
            Assert.IsType<ImageData>(imported.First());
            var importedImage = (ImageData)imported.First();
            Assert.NotNull(importedImage.Bitmap);
            Assert.Equal(10, importedImage.Bitmap.Width);
            Assert.Equal(10, importedImage.Bitmap.Height);
        }

        [Fact]
        public async Task ImportExport_Jpg_Works()
        {
            using var bitmap = new SKBitmap(8, 8);
            using var canvas = new SKCanvas(bitmap);
            canvas.Clear(SKColors.Blue);

            var imageData = new ImageData
            {
                Bitmap = bitmap,
                DestRect = new SKRect(0, 0, 8, 8),
                Visible = true,
                Id = "test"
            };

            var service = new ImportExportService();
            using var ms = new MemoryStream();
            await service.ExportAsync(new[] { imageData }, ms, ImageFormat.Jpg);

            ms.Seek(0, SeekOrigin.Begin);
            var imported = await service.ImportAsync(ms, ImageFormat.Jpg);
            Assert.Single(imported);
            Assert.IsType<ImageData>(imported.First());
            var importedImage = (ImageData)imported.First();
            Assert.NotNull(importedImage.Bitmap);
            Assert.Equal(8, importedImage.Bitmap.Width);
            Assert.Equal(8, importedImage.Bitmap.Height);
        }

        [Fact]
        public async Task ImportExport_Svg_Works()
        {
            var svg = "<svg width=\"20\" height=\"20\" xmlns=\"http://www.w3.org/2000/svg\"></svg>";
            var service = new ImportExportService();
            using var ms = new MemoryStream(Encoding.UTF8.GetBytes(svg));
            var imported = await service.ImportAsync(ms, ImageFormat.Svg);
            Assert.Single(imported);
            Assert.IsType<ImageData>(imported.First());

            using var exportMs = new MemoryStream();
            await service.ExportAsync(imported, exportMs, ImageFormat.Svg);
            exportMs.Seek(0, SeekOrigin.Begin);
            var exportedSvg = Encoding.UTF8.GetString(exportMs.ToArray());
            Assert.Contains("<svg", exportedSvg);
        }
        [Fact]
        public async Task ImportAsync_NullStream_Throws()
        {
            var service = new ImportExportService();
            await Assert.ThrowsAsync<ArgumentNullException>(() => service.ImportAsync((Stream?)null!, ImageFormat.Png));
        }

        [Fact]
        public async Task ExportAsync_NullElements_Throws()
        {
            var service = new ImportExportService();
            using var ms = new MemoryStream();
            await Assert.ThrowsAsync<ArgumentNullException>(() => service.ExportAsync((IEnumerable<ICanvasElement>?)null!, ms, ImageFormat.Png));
        }

        [Fact]
        public async Task ExportAsync_NullStream_Throws()
        {
            var service = new ImportExportService();
            var image = new ImageData { Bitmap = new SKBitmap(1, 1), DestRect = new SKRect(0, 0, 1, 1), Visible = true, Id = "x" };
            await Assert.ThrowsAsync<ArgumentNullException>(() => service.ExportAsync(new[] { image }, (Stream?)null!, ImageFormat.Png));
        }

        [Fact]
        public async Task ImportAsync_UnsupportedFormat_Throws()
        {
            var service = new ImportExportService();
            using var ms = new MemoryStream(new byte[] { 1, 2, 3 });
            await Assert.ThrowsAsync<NotSupportedException>(() => service.ImportAsync(ms, (ImageFormat)99));
        }

        [Fact]
        public async Task ExportAsync_UnsupportedFormat_Throws()
        {
            var service = new ImportExportService();
            var image = new ImageData { Bitmap = new SKBitmap(1, 1), DestRect = new SKRect(0, 0, 1, 1), Visible = true, Id = "x" };
            using var ms = new MemoryStream();
            await Assert.ThrowsAsync<NotSupportedException>(() => service.ExportAsync(new[] { image }, ms, (ImageFormat)99));
        }

        [Fact]
        public async Task ImportAsync_InvalidPng_Throws()
        {
            var service = new ImportExportService();
            using var ms = new MemoryStream(new byte[] { 0, 1, 2, 3 });
            await Assert.ThrowsAsync<InvalidDataException>(() => service.ImportAsync(ms, ImageFormat.Png));
        }

        [Fact]
        public async Task ExportAsync_NoImageData_Throws()
        {
            var service = new ImportExportService();
            using var ms = new MemoryStream();
            var geo = new GeometryData { Id = "g", Visible = true, Type = "Rectangle", Points = new SKPoint[4] };
            await Assert.ThrowsAsync<InvalidDataException>(() => service.ExportAsync(new[] { geo }, ms, ImageFormat.Png));
        }

        [Fact]
        public async Task ImportAsync_EmptySvg_ReturnsImageData()
        {
            var service = new ImportExportService();
            var svg = "<svg width=\"10\" height=\"10\" xmlns=\"http://www.w3.org/2000/svg\"></svg>";
            using var ms = new MemoryStream(Encoding.UTF8.GetBytes(svg));
            var result = await service.ImportAsync(ms, ImageFormat.Svg);
            Assert.Single(result);
            Assert.IsType<ImageData>(result.First());
        }

        [Fact]
        public async Task ImportAsync_RectSvg_ReturnsGeometryData()
        {
            var service = new ImportExportService();
            var svg = "<svg><rect x=\"1\" y=\"2\" width=\"3\" height=\"4\" /></svg>";
            using var ms = new MemoryStream(Encoding.UTF8.GetBytes(svg));
            var result = await service.ImportAsync(ms, ImageFormat.Svg);
            Assert.Single(result);
            Assert.IsType<GeometryData>(result.First());
            var geo = (GeometryData)result.First();
            Assert.Equal("Rectangle", geo.Type);
            Assert.Equal(4, geo.Points.Length);
        }

        [Fact]
        public async Task ImportAsync_EllipseSvg_ReturnsGeometryData()
        {
            var service = new ImportExportService();
            var svg = "<svg><ellipse cx=\"5\" cy=\"6\" rx=\"7\" ry=\"8\" /></svg>";
            using var ms = new MemoryStream(Encoding.UTF8.GetBytes(svg));
            var result = await service.ImportAsync(ms, ImageFormat.Svg);
            Assert.Single(result);
            Assert.IsType<GeometryData>(result.First());
            var geo = (GeometryData)result.First();
            Assert.Equal("Ellipse", geo.Type);
            Assert.Equal(4, geo.Points.Length);
        }

        [Fact]
        public async Task ExportAsync_Svg_GeometryData()
        {
            var service = new ImportExportService();
            var geo = new GeometryData
            {
                Id = "g",
                Visible = true,
                Type = "Rectangle",
                Points = new[] { new SKPoint(1, 2), new SKPoint(4, 2), new SKPoint(4, 6), new SKPoint(1, 6) }
            };
            using var ms = new MemoryStream();
            await service.ExportAsync(new[] { geo }, ms, ImageFormat.Svg);
            ms.Seek(0, SeekOrigin.Begin);
            var svg = Encoding.UTF8.GetString(ms.ToArray());
            Assert.Contains("<rect", svg);
            Assert.Contains("x=\"1\"", svg);
            Assert.Contains("width=\"3\"", svg);
            Assert.Contains("height=\"4\"", svg);
        }

        [Fact]
        public async Task ExportAsync_Svg_Ellipse()
        {
            var service = new ImportExportService();
            var geo = new GeometryData
            {
                Id = "g",
                Visible = true,
                Type = "Ellipse",
                Points = new[] { new SKPoint(3, 4), new SKPoint(7, 4), new SKPoint(5, 2), new SKPoint(5, 6) }
            };
            using var ms = new MemoryStream();
            await service.ExportAsync(new[] { geo }, ms, ImageFormat.Svg);
            ms.Flush(); // 补充，确保流内容完整
            ms.Seek(0, SeekOrigin.Begin);
            var svg = Encoding.UTF8.GetString(ms.ToArray());
            Console.WriteLine($"SVG Length: {svg.Length}");
            Console.WriteLine($"SVG Content: {svg}");
            Assert.Contains("<ellipse", svg);
            Assert.Contains("cx=\"5\"", svg);
            Assert.Contains("cy=\"4\"", svg);
            Assert.Contains("rx=\"2\"", svg);
            Assert.Matches(@"ry\s*=\s*""2""", svg);
        }

        [Fact]
        public async Task ExportAsync_Svg_MixedElements_OnlyGeometryExported()
        {
            var service = new ImportExportService();
            var geo = new GeometryData
            {
                Id = "g",
                Visible = true,
                Type = "Rectangle",
                Points = new[] { new SKPoint(1, 2), new SKPoint(4, 2), new SKPoint(4, 6), new SKPoint(1, 6) }
            };
            var image = new ImageData
            {
                Id = "img",
                Visible = true,
                Bitmap = new SKBitmap(2, 2),
                DestRect = new SKRect(0, 0, 2, 2)
            };
            using var ms = new MemoryStream();
            await service.ExportAsync(new ICanvasElement[] { geo, image }, ms, ImageFormat.Svg);
            ms.Seek(0, SeekOrigin.Begin);
            var svg = Encoding.UTF8.GetString(ms.ToArray());
            Assert.Contains("<rect", svg);
            Assert.DoesNotContain("image", svg); // 不导出ImageData为SVG
        }
    }
}