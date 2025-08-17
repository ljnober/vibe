using System.Collections.Generic;

namespace AvaloniaAsyncDrawing.Commands
{
    /// <summary>
    /// 向集合添加元素的可撤销命令。
    /// </summary>
    /// <typeparam name="T">元素类型</typeparam>
    public class AddElementCommand<T> : IUndoableCommand
    {
        private readonly IList<T> _collection;
        private readonly T _element;
        private int _index = -1;
        private bool _executed = false;

        public AddElementCommand(IList<T> collection, T element)
        {
            _collection = collection;
            _element = element;
        }

        public bool CanExecute()
        {
            return _collection != null && _element != null;
        }

        public void Execute()
        {
            if (!_executed && CanExecute())
            {
                _collection.Add(_element);
                _index = _collection.Count - 1;
                _executed = true;
            }
        }

        public void Undo()
        {
            if (_executed && _index >= 0 && _index < _collection.Count && EqualityComparer<T>.Default.Equals(_collection[_index], _element))
            {
                _collection.RemoveAt(_index);
                _executed = false;
            }
        }

        public void Redo()
        {
            if (!_executed && _index >= 0 && _index <= _collection.Count)
            {
                _collection.Insert(_index, _element);
                _executed = true;
            }
        }
    }
}