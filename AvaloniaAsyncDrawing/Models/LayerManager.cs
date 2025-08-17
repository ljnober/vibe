// AvaloniaAsyncDrawing/Models/LayerManager.cs
using System;
using System.Collections.Generic;
using System.Linq;
using SkiaSharp;

namespace AvaloniaAsyncDrawing.Models
{
    /// <summary>
    /// 图层管理服务，支持多图层增删改查、层级调整、批量命中与协作。
    /// </summary>
    public class LayerManager
    {
        private readonly List<LayerData> _layers = new List<LayerData>();

        /// <summary>
        /// 所有图层（只读）。
        /// </summary>
        public IReadOnlyList<LayerData> Layers => _layers.OrderBy(l => l.ZIndex).ToList();

        /// <summary>
        /// 添加图层。
        /// </summary>
        public void AddLayer(LayerData layer)
        {
            if (layer != null && !_layers.Contains(layer))
                _layers.Add(layer);
        }

        /// <summary>
        /// 移除图层。
        /// </summary>
        public void RemoveLayer(LayerData layer)
        {
            if (layer != null)
                _layers.Remove(layer);
        }

        /// <summary>
        /// 通过 Id 查找图层。
        /// </summary>
        public LayerData? FindLayer(string id)
        {
            return _layers.FirstOrDefault(l => l.Id == id);
        }

        /// <summary>
        /// 调整图层层级。
        /// </summary>
        public void SetLayerZIndex(LayerData layer, int zIndex)
        {
            if (layer != null)
                layer.ZIndex = zIndex;
        }

        /// <summary>
        /// 批量命中测试，返回命中的图层及元素。
        /// </summary>
        public List<(LayerData Layer, object Element)> HitTestAll(SKPoint point)
        {
            var result = new List<(LayerData, object)>();
            foreach (var layer in Layers.Reverse())
            {
                if (!layer.Visible) continue;
                foreach (var geo in layer.Geometries)
                {
                    bool isHit = geo is IHitTestable hit && hit.HitTest(point);
                    System.Diagnostics.Debug.WriteLine($"[HitTestAll] Layer:{layer.Id} Geo:{geo?.GetType().Name} Hit:{isHit} Point:{point} GeoObj:{System.Text.Json.JsonSerializer.Serialize(geo)}");
                    if (isHit)
                        result.Add((layer, geo));
                }
                foreach (var text in layer.Texts)
                {
                    bool isHit = text is IHitTestable hit && hit.HitTest(point);
                    System.Diagnostics.Debug.WriteLine($"[HitTestAll] Layer:{layer.Id} Text:{text?.GetType().Name} Hit:{isHit} Point:{point} TextObj:{System.Text.Json.JsonSerializer.Serialize(text)}");
                    if (isHit)
                        result.Add((layer, text));
                }
                foreach (var img in layer.Images)
                {
                    bool isHit = img is IHitTestable hit && hit.HitTest(point);
                    System.Diagnostics.Debug.WriteLine($"[HitTestAll] Layer:{layer.Id} Image:{img?.GetType().Name} Hit:{isHit} Point:{point} ImgObj:{System.Text.Json.JsonSerializer.Serialize(img)}");
                    if (isHit)
                        result.Add((layer, img));
                }
            }
            System.Diagnostics.Debug.WriteLine($"[HitTestAll] Total hits: {result.Count}");
            return result;
        }

        /// <summary>
        /// 获取顶层命中元素（可用于单选）。
        /// </summary>
        public (LayerData Layer, object Element)? HitTestTop(SKPoint point)
        {
            foreach (var layer in Layers.Reverse())
            {
                if (!layer.Visible) continue;
                foreach (var geo in layer.Geometries)
                {
                    bool isHit = geo is IHitTestable hit && hit.HitTest(point);
                    System.Diagnostics.Debug.WriteLine($"[HitTestTop] Layer:{layer.Id} Geo:{geo?.GetType().Name} Hit:{isHit} Point:{point} GeoObj:{System.Text.Json.JsonSerializer.Serialize(geo)}");
                    if (isHit)
                        return (layer, geo);
                }
                foreach (var text in layer.Texts)
                {
                    bool isHit = text is IHitTestable hit && hit.HitTest(point);
                    System.Diagnostics.Debug.WriteLine($"[HitTestTop] Layer:{layer.Id} Text:{text?.GetType().Name} Hit:{isHit} Point:{point} TextObj:{System.Text.Json.JsonSerializer.Serialize(text)}");
                    if (isHit)
                        return (layer, text);
                }
                foreach (var img in layer.Images)
                {
                    bool isHit = img is IHitTestable hit && hit.HitTest(point);
                    System.Diagnostics.Debug.WriteLine($"[HitTestTop] Layer:{layer.Id} Image:{img?.GetType().Name} Hit:{isHit} Point:{point} ImgObj:{System.Text.Json.JsonSerializer.Serialize(img)}");
                    if (isHit)
                        return (layer, img);
                }
            }
            System.Diagnostics.Debug.WriteLine("[HitTestTop] No hit found.");
            return null;
        }
    }
}