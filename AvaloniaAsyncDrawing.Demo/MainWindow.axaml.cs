using Avalonia.Controls;

namespace AvaloniaAsyncDrawing.Demo
{public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        // 绑定 MainWindowViewModel，注入 ImportExportService
        DataContext = new AvaloniaAsyncDrawing.ViewModels.MainWindowViewModel(
            new AvaloniaAsyncDrawing.Utils.ImportExportService());
    }
}
}