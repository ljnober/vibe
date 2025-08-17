using System.Collections.Generic;
using AvaloniaAsyncDrawing.Commands;
using Xunit;

namespace AvaloniaAsyncDrawing.Tests
{
    public class CommandManagerTests
    {
        [Fact]
        public void Execute_AddElementCommand_AddsElement()
        {
            var list = new List<int>();
            var cmd = new AddElementCommand<int>(list, 42);
            var manager = new CommandManager();

            manager.Execute(cmd);

            Assert.Single(list);
            Assert.Equal(42, list[0]);
            Assert.True(manager.CanUndo);
            Assert.False(manager.CanRedo);
        }

        [Fact]
        public void Undo_AddElementCommand_RemovesElement()
        {
            var list = new List<string>();
            var cmd = new AddElementCommand<string>(list, "test");
            var manager = new CommandManager();

            manager.Execute(cmd);
            manager.Undo();

            Assert.Empty(list);
            Assert.False(manager.CanUndo);
            Assert.True(manager.CanRedo);
        }

        [Fact]
        public void Redo_AddElementCommand_RestoresElement()
        {
            var list = new List<double>();
            var cmd = new AddElementCommand<double>(list, 3.14);
            var manager = new CommandManager();

            manager.Execute(cmd);
            manager.Undo();
            manager.Redo();

            Assert.Single(list);
            Assert.Equal(3.14, list[0]);
            Assert.True(manager.CanUndo);
            Assert.False(manager.CanRedo);
        }

        [Fact]
        public void Clear_ClearsHistory()
        {
            var list = new List<int>();
            var cmd = new AddElementCommand<int>(list, 1);
            var manager = new CommandManager();

            manager.Execute(cmd);
            manager.Clear();

            Assert.True(list.Count == 1);
            Assert.False(manager.CanUndo);
            Assert.False(manager.CanRedo);
        }
    }
}