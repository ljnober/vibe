// AsyncRenderScheduler.cs
// IAsyncRenderScheduler 实现：异步调度渲染任务，线程安全，支持事件回调。

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace AvaloniaAsyncDrawing.Drawing
{
    /// <summary>
    /// 异步绘制调度器，负责调度异步渲染任务，确保线程安全与数据一致性。
    /// </summary>
    public class AsyncRenderScheduler : IAsyncRenderScheduler
    {
        private readonly object _lock = new object();
        private readonly ConcurrentQueue<Func<Task>> _renderQueue = new ConcurrentQueue<Func<Task>>();
        private bool _isRendering = false;

        /// <inheritdoc/>
        public event EventHandler? RenderCompleted;

        /// <summary>
        /// 启动一次异步渲染任务，自动排队，线程安全。
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>表示异步渲染过程的 Task</returns>
        public Task BeginAsyncRender(CancellationToken cancellationToken = default)
        {
            var tcs = new TaskCompletionSource<bool>();
            _renderQueue.Enqueue(async () =>
            {
                try
                {
                    // 这里应调用实际的渲染逻辑（如离屏绘制、数据快照等）
                    await Task.Run(() =>
                    {
                        // ...渲染主流程（略，业务层注入）
                        // 模拟耗时
                        Thread.Sleep(10);
                        System.Diagnostics.Debug.WriteLine($"[AsyncRenderScheduler] Task running, cancellationToken.IsCancellationRequested={cancellationToken.IsCancellationRequested}");
                        cancellationToken.ThrowIfCancellationRequested();
                    }, cancellationToken);

                    RenderCompleted?.Invoke(this, EventArgs.Empty);
                    System.Diagnostics.Debug.WriteLine("[AsyncRenderScheduler] RenderCompleted event invoked.");
                    tcs.SetResult(true);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[AsyncRenderScheduler] Exception: {ex.GetType().Name} {ex.Message}");
                    tcs.SetException(ex);
                }
            });

            StartRenderLoop();
            return tcs.Task;
        }

        /// <summary>
        /// 渲染队列主循环，保证同一时刻仅有一个渲染任务在执行。
        /// </summary>
        private void StartRenderLoop()
        {
            lock (_lock)
            {
                if (_isRendering) return;
                _isRendering = true;
                Task.Run(async () =>
                {
                    while (_renderQueue.TryDequeue(out var renderTask))
                    {
                        await renderTask();
                    }
                    lock (_lock)
                    {
                        _isRendering = false;
                    }
                });
            }
        }
    }
}