using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AvaloniaAsyncDrawing.Drawing;
using Xunit;

namespace AvaloniaAsyncDrawing.Tests
{
    public class AsyncRenderSchedulerTests
    {
        [Fact]
        public async Task BeginAsyncRender_TriggersRenderCompleted()
        {
            var scheduler = new AsyncRenderScheduler();
            bool completed = false;
            scheduler.RenderCompleted += (s, e) => completed = true;
            await scheduler.BeginAsyncRender();
            Assert.True(completed);
        }

        [Fact]
        public async Task BeginAsyncRender_MultipleCalls_AreQueued()
        {
            var scheduler = new AsyncRenderScheduler();
            int count = 0;
            scheduler.RenderCompleted += (s, e) => Interlocked.Increment(ref count);

            var tasks = new List<Task>();
            for (int i = 0; i < 5; i++)
                tasks.Add(scheduler.BeginAsyncRender());

            await Task.WhenAll(tasks);
            Assert.Equal(5, count);
        }

        [Fact]
        public async Task BeginAsyncRender_Cancellation_Throws()
        {
            var scheduler = new AsyncRenderScheduler();
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            {
                await scheduler.BeginAsyncRender(cts.Token);
            });
        }

        [Fact]
        public async Task BeginAsyncRender_ThreadSafety()
        {
            var scheduler = new AsyncRenderScheduler();
            int completed = 0;
            scheduler.RenderCompleted += (s, e) => Interlocked.Increment(ref completed);

            var tasks = new List<Task>();
            Parallel.For(0, 10, i =>
            {
                tasks.Add(scheduler.BeginAsyncRender());
            });

            await Task.WhenAll(tasks);
            Assert.Equal(10, completed);
        }
    }
}