using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AvaloniaAsyncDrawing.Drawing;
using Xunit;

namespace AvaloniaAsyncDrawing.Tests
{
    public class ThreadSafeCollectionTests
    {
        [Fact]
        public void Add_Remove_Clear_BasicUsage()
        {
            var col = new ThreadSafeCollection<int>();
            col.Add(1);
            col.Add(2);
            Assert.Equal(2, col.Count);
            Assert.True(col.Remove(1));
            Assert.False(col.Remove(3));
            Assert.Single(col);
            col.Clear();
            Assert.Empty(col);
        }

        [Fact]
        public void Snapshot_ReturnsCopy()
        {
            var col = new ThreadSafeCollection<string>();
            col.Add("a");
            col.Add("b");
            var snap = col.Snapshot();
            Assert.Equal(2, snap.Length);
            col.Clear();
            Assert.Equal(2, snap.Length);
        }

        [Fact]
        public void ThreadSafety_MultiThreadedAdd()
        {
            var col = new ThreadSafeCollection<int>();
            int total = 1000;
            Parallel.For(0, total, i => col.Add(i));
            Assert.Equal(total, col.Count);
            var set = new HashSet<int>(col.Snapshot());
            for (int i = 0; i < total; i++)
                Assert.Contains(i, set);
        }

        [Fact]
        public void ThreadSafety_MultiThreadedRemove()
        {
            var col = new ThreadSafeCollection<int>();
            for (int i = 0; i < 100; i++) col.Add(i);
            Parallel.For(0, 100, i => col.Remove(i));
            Assert.Empty(col);
        }
    }
}