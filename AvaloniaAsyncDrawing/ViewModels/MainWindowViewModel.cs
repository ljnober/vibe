// MainWindowViewModel.cs
using AvaloniaAsyncDrawing.Utils;
using AvaloniaAsyncDrawing.Models;
using System.Windows.Input;
using System.Threading.Tasks;
using Avalonia.Controls;
using System;
using System.ComponentModel;
using System.IO;
using System.Collections.ObjectModel;

namespace AvaloniaAsyncDrawing.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly IImportExportService _importExportService;
        public ICommand ImportCommand { get; }
        public ICommand ExportCommand { get; }

        private string _statusMessage;
        public string StatusMessage
        {
            get => _statusMessage;
            set { _statusMessage = value; OnPropertyChanged(nameof(StatusMessage)); }
        }

        public ObservableCollection<ICanvasElement> Elements { get; } = new System.Collections.ObjectModel.ObservableCollection<ICanvasElement>();

        public MainWindowViewModel(IImportExportService importExportService)
        {
            _importExportService = importExportService;
            ImportCommand = new RelayCommand(async _ => await ImportAsync());
            ExportCommand = new RelayCommand(async _ => await ExportAsync());
            StatusMessage = "就绪";
        }

        private async Task ImportAsync()
        {
            try
            {
                var dialog = new OpenFileDialog
                {
                    Title = "选择导入文件",
                    AllowMultiple = false,
                    Filters =
                    {
                        new FileDialogFilter { Name = "图片/矢量", Extensions = { "svg", "png", "jpg", "jpeg" } }
                    }
                };
                var window = GetActiveWindow();
                var result = await dialog.ShowAsync(window);
                if (result == null || result.Length == 0)
                {
                    StatusMessage = "导入已取消";
                    return;
                }
                var filePath = result[0];
                var format = GetFormatFromExtension(System.IO.Path.GetExtension(filePath));
                if (format == null)
                {
                    StatusMessage = "不支持的文件格式";
                    return;
                }
                using var stream = File.OpenRead(filePath);
                var elements = await _importExportService.ImportAsync(stream, format.Value);
                Elements.Clear();
                foreach (var el in elements)
                    Elements.Add(el);
                StatusMessage = $"导入成功：{System.IO.Path.GetFileName(filePath)}";
            }
            catch (Exception ex)
            {
                StatusMessage = $"导入失败：{ex.Message}";
            }
        }

        private async Task ExportAsync()
        {
            try
            {
                if (Elements.Count == 0)
                {
                    StatusMessage = "无可导出的内容";
                    return;
                }
                var dialog = new SaveFileDialog
                {
                    Title = "导出文件",
                    Filters =
                    {
                        new FileDialogFilter { Name = "SVG 矢量", Extensions = { "svg" } },
                        new FileDialogFilter { Name = "PNG 图片", Extensions = { "png" } },
                        new FileDialogFilter { Name = "JPG 图片", Extensions = { "jpg", "jpeg" } }
                    }
                };
                var window = GetActiveWindow();
                var filePath = await dialog.ShowAsync(window);
                if (string.IsNullOrEmpty(filePath))
                {
                    StatusMessage = "导出已取消";
                    return;
                }
                var format = GetFormatFromExtension(System.IO.Path.GetExtension(filePath));
                if (format == null)
                {
                    StatusMessage = "不支持的导出格式";
                    return;
                }
                using var stream = File.Open(filePath, FileMode.Create, FileAccess.Write);
                await _importExportService.ExportAsync(Elements, stream, format.Value);
                StatusMessage = $"导出成功：{System.IO.Path.GetFileName(filePath)}";
            }
            catch (Exception ex)
            {
                StatusMessage = $"导出失败：{ex.Message}";
            }
        }

        private static Avalonia.Controls.Window? GetActiveWindow()
        {
            return Avalonia.Application.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop
                ? desktop.MainWindow
                : null;
        }

        private static ImageFormat? GetFormatFromExtension(string ext)
        {
            ext = ext.ToLowerInvariant().TrimStart('.');
            return ext switch
            {
                "svg" => ImageFormat.Svg,
                "png" => ImageFormat.Png,
                "jpg" => ImageFormat.Jpg,
                "jpeg" => ImageFormat.Jpg,
                _ => null
            };
        }
    }
}