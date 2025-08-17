// IAsyncRenderScheduler.cs
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AvaloniaAsyncDrawing.Drawing
{
    /// <summary>
    /// 异步绘制调度器接口，定义异步渲染调度方法与完成事件。
    /// </summary>
    public interface IAsyncRenderScheduler
    {
        /// <summary>
        /// 启动一次异步渲染任务，自动排队，线程安全。
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>表示异步渲染过程的 Task</returns>
        Task BeginAsyncRender(CancellationToken cancellationToken = default);

        /// <summary>
        /// 渲染完成事件。
        /// </summary>
        event EventHandler RenderCompleted;
    }
}