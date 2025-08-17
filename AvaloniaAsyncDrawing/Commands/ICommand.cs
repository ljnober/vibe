namespace AvaloniaAsyncDrawing.Commands
{
    /// <summary>
    /// 命令模式基础接口，定义命令的执行与可执行性判断。
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        void Execute();

        /// <summary>
        /// 判断命令是否可执行。
        /// </summary>
        /// <returns>可执行返回 true，否则 false。</returns>
        bool CanExecute();
    }
}