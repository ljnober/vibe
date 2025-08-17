// ViewModels/BaseViewModel.cs
// 该文件为视图模型层基类，实现属性通知等 MVVM 基础功能。

using System;
using System.ComponentModel;

namespace AvaloniaAsyncDrawing.ViewModels
{
    /// <summary>
    /// 视图模型层基类，提供属性变更通知机制，便于 MVVM 数据绑定。
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 属性变更事件，供界面绑定监听。
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// 触发属性变更通知。
        /// </summary>
        /// <param name="propertyName">属性名</param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}