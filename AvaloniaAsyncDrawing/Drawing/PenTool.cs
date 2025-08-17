using System.Collections.Generic;
using AvaloniaAsyncDrawing.Models;
using SkiaSharp;

namespace AvaloniaAsyncDrawing.Drawing
{
    /// <summary>
    /// 画笔工具，实现自由路径绘制。
    /// </summary>
    public class PenTool : BaseTool, IDrawingTool
    {
        private List<SKPoint> _currentPoints;
        private bool _isDrawing;

        public IReadOnlyList<SKPoint> CurrentPoints => _currentPoints.AsReadOnly();

        public PenTool() : base("Pen", "画笔")
        {
            _currentPoints = new List<SKPoint>();
            _isDrawing = false;
        }

        public void OnPointerDown(double x, double y)
        {
            _currentPoints.Clear();
            _currentPoints.Add(new SKPoint((float)x, (float)y));
            _isDrawing = true;
        }

        public void OnPointerMove(double x, double y)
        {
            if (_isDrawing)
            {
                _currentPoints.Add(new SKPoint((float)x, (float)y));
            }
        }

        public void OnPointerUp(double x, double y)
        {
            if (_isDrawing)
            {
                _currentPoints.Add(new SKPoint((float)x, (float)y));
                _isDrawing = false;
            }
        }

        public override void Reset()
        {
            base.Reset();
            _currentPoints.Clear();
            _isDrawing = false;
        }

        /// <summary>
        /// 获取当前路径为 Path 模型，可用于添加到图层。
        /// </summary>
        public AvaloniaAsyncDrawing.Models.Path? ToPath()
        {
            if (_currentPoints.Count < 3)
            {
                System.Diagnostics.Debug.WriteLine($"[PenTool.ToPath] Not enough points: {_currentPoints.Count}");
                return null;
            }
            var path = new AvaloniaAsyncDrawing.Models.Path();
            if (_currentPoints.Count > 0)
            {
                // 首点 MoveTo
                path.Commands.Add(new AvaloniaAsyncDrawing.Models.PathCommand(
                    AvaloniaAsyncDrawing.Models.PathCommandType.MoveTo,
                    new[] { _currentPoints[0] }
                ));
                // 后续点 LineTo
                for (int i = 1; i < _currentPoints.Count; i++)
                {
                    path.Commands.Add(new AvaloniaAsyncDrawing.Models.PathCommand(
                        AvaloniaAsyncDrawing.Models.PathCommandType.LineTo,
                        new[] { _currentPoints[i] }
                    ));
                }
            }
            System.Diagnostics.Debug.WriteLine($"[PenTool.ToPath] Path created with {_currentPoints.Count} points, PathObj:{System.Text.Json.JsonSerializer.Serialize(path)}");
            return path;
        }
    }
}