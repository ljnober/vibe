// AvaloniaAsyncDrawing/Models/LayerData.cs
using SkiaSharp;
using System.Collections.Generic;

namespace AvaloniaAsyncDrawing.Models
{
    /// <summary>
    /// 图层数据类型声明，实现 ICanvasElement 与 IRenderable。
    /// </summary>
    public class LayerData : ICanvasElement, IRenderable, IHitTestable
    {
        /// <summary>
        /// 图层唯一标识。
        /// </summary>
        public string Id { get; set; } = string.Empty;
    
        /// <summary>
        /// 是否可见。
        /// </summary>
        public bool Visible { get; set; }
    
        /// <summary>
        /// 层级索引。
        /// </summary>
        public int ZIndex { get; set; }
    
        private readonly List<GeometryData> _geometries = new List<GeometryData>();
        private readonly List<TextData> _texts = new List<TextData>();
        private readonly List<ImageData> _images = new List<ImageData>();
    
        /// <summary>
        /// 几何对象集合（只读）。
        /// </summary>
        public IReadOnlyList<GeometryData> Geometries => _geometries.AsReadOnly();
    
        /// <summary>
        /// 文本对象集合（只读）。
        /// </summary>
        public IReadOnlyList<TextData> Texts => _texts.AsReadOnly();
    
        /// <summary>
        /// 图片对象集合（只读）。
        /// </summary>
        public IReadOnlyList<ImageData> Images => _images.AsReadOnly();
    
        /// <summary>
        /// 添加几何对象。
        /// </summary>
        public void AddGeometry(GeometryData? geometry)
        {
            if (geometry != null && !_geometries.Contains(geometry))
                _geometries.Add(geometry);
        }
    
        /// <summary>
        /// 移除几何对象。
        /// </summary>
        public void RemoveGeometry(GeometryData? geometry)
        {
            if (geometry != null)
                _geometries.Remove(geometry);
        }
    
        /// <summary>
        /// 添加文本对象。
        /// </summary>
        public void AddText(TextData? text)
        {
            if (text != null && !_texts.Contains(text))
                _texts.Add(text);
        }
    
        /// <summary>
        /// 移除文本对象。
        /// </summary>
        public void RemoveText(TextData? text)
        {
            if (text != null)
                _texts.Remove(text);
        }
    
        /// <summary>
        /// 添加图片对象。
        /// </summary>
        public void AddImage(ImageData? image)
        {
            if (image != null && !_images.Contains(image))
                _images.Add(image);
        }
    
        /// <summary>
        /// 移除图片对象。
        /// </summary>
        public void RemoveImage(ImageData? image)
        {
            if (image != null)
                _images.Remove(image);
        }
    
        /// <summary>
        /// 渲染方法。
        /// </summary>
        public void Render(SKCanvas canvas)
        {
            if (!Visible) return;
            foreach (var geo in _geometries)
                geo?.Render(canvas);
            foreach (var text in _texts)
                text?.Render(canvas);
            foreach (var img in _images)
                img?.Render(canvas, img.DestRect, img.Paint);
        }
    
        /// <summary>
        /// 命中测试：批量遍历所有元素，任一命中即返回 true。
        /// </summary>
        public bool HitTest(SKPoint point)
        {
            foreach (var geo in _geometries)
                if (geo is IHitTestable hit && hit.HitTest(point))
                    return true;
            foreach (var text in _texts)
                if (text is IHitTestable hit && hit.HitTest(point))
                    return true;
            foreach (var img in _images)
                if (img is IHitTestable hit && hit.HitTest(point))
                    return true;
            return false;
        }

        /// <summary>
        /// 深拷贝（快照）。
        /// </summary>
        public LayerData Clone()
        {
            var clone = new LayerData
            {
                Id = this.Id,
                Visible = this.Visible,
                ZIndex = this.ZIndex
            };
            foreach (var geo in _geometries)
                clone.AddGeometry(geo?.Clone());
            foreach (var text in _texts)
                clone.AddText(text?.Clone());
            foreach (var img in _images)
                clone.AddImage(img?.Clone());
            return clone;
        }
    }
}