// AvaloniaAsyncDrawing/Utils/IImportExportService.cs
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AvaloniaAsyncDrawing.Models;

namespace AvaloniaAsyncDrawing.Utils
{
    /// <summary>
    /// 支持主流矢量/位图格式的导入导出服务接口。
    /// </summary>
    public enum ImageFormat
    {
        Svg,
        Png,
        Jpg
    }

    public interface IImportExportService
    {
        /// <summary>
        /// 导入文件为 ImageData 或 LayerData。
        /// </summary>
        /// <param name="stream">输入流</param>
        /// <param name="format">文件格式</param>
        /// <returns>导入的图像数据或图层数据</returns>
        Task<IReadOnlyList<ICanvasElement>> ImportAsync(Stream stream, ImageFormat format);

        /// <summary>
        /// 导出为指定格式文件。
        /// </summary>
        /// <param name="elements">要导出的元素集合</param>
        /// <param name="stream">输出流</param>
        /// <param name="format">导出格式</param>
        /// <returns>异步任务</returns>
        Task ExportAsync(IEnumerable<ICanvasElement> elements, Stream stream, ImageFormat format);
    }
}