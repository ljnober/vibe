using System.Collections.Generic;

namespace AvaloniaAsyncDrawing.Commands
{
    /// <summary>
    /// 命令管理器，支持命令执行、撤销与重做。
    /// </summary>
    public class CommandManager
    {
        private readonly Stack<IUndoableCommand> _undoStack = new Stack<IUndoableCommand>();
        private readonly Stack<IUndoableCommand> _redoStack = new Stack<IUndoableCommand>();

        /// <summary>
        /// 是否可以撤销。
        /// </summary>
        public bool CanUndo => _undoStack.Count > 0;

        /// <summary>
        /// 是否可以重做。
        /// </summary>
        public bool CanRedo => _redoStack.Count > 0;

        /// <summary>
        /// 执行命令，并加入撤销栈。
        /// </summary>
        /// <param name="command">待执行命令</param>
        public void Execute(IUndoableCommand command)
        {
            if (command.CanExecute())
            {
                command.Execute();
                _undoStack.Push(command);
                _redoStack.Clear();
            }
        }

        /// <summary>
        /// 撤销上一个命令。
        /// </summary>
        public void Undo()
        {
            if (CanUndo)
            {
                var command = _undoStack.Pop();
                command.Undo();
                _redoStack.Push(command);
            }
        }

        /// <summary>
        /// 重做上一个被撤销的命令。
        /// </summary>
        public void Redo()
        {
            if (CanRedo)
            {
                var command = _redoStack.Pop();
                command.Redo();
                _undoStack.Push(command);
            }
        }

        /// <summary>
        /// 清空所有命令历史。
        /// </summary>
        public void Clear()
        {
            _undoStack.Clear();
            _redoStack.Clear();
        }
    }
}