namespace AvaloniaAsyncDrawing.Commands
{
    /// <summary>
    /// 支持撤销与重做的命令接口。
    /// </summary>
    public interface IUndoableCommand : ICommand
    {
        /// <summary>
        /// 撤销命令。
        /// </summary>
        void Undo();

        /// <summary>
        /// 重做命令。
        /// </summary>
        void Redo();
    }
}