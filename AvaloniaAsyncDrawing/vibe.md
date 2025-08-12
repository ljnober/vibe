# AvaloniaUI SkiaSharp Offscreen Rendering Control Design

## Overview
This document describes the design of a custom AvaloniaUI control that utilizes SkiaSharp for offscreen rendering. The control supports simultaneous rendering of images, geometries, and text, and features zoom functionality to maintain consistent visual line width. The concept of layers is introduced, allowing multi-layered content overlay rendering. Line width supports sub-pixel rendering, and images support pixel-level rendering (no interpolation).

## Features

- **Offscreen Rendering**: Uses SkiaSharp's `SKSurface` for offscreen drawing, improving performance and flexibility.
- **Multi-type Drawing**: Supports rendering:
    - Bitmap images (pixel-level rendering, no interpolation)
    - Geometric shapes (lines, rectangles, ellipses, paths, line width supports sub-pixel)
    - Text
- **Layer Support**: Content can be divided into multiple layers, each managed and rendered independently, with support for layer order and visibility settings.
- **Zoom Support**: Allows users to zoom the view; all content scales with zoom, but line width remains visually consistent.
- **High Performance**: Efficient redraws via offscreen buffering.

## Architecture Design

### 1. Control Structure

- Inherits from `Avalonia.Controls.Control`.
- Maintains an offscreen `SKSurface` for rendering.
- Exposes the following properties:
    - Layer collection (`IList<LayerData>`)
    - Zoom factor (`double Zoom`)

### 2. Layer Data Structure

```csharp
class LayerData {
    string Name;
    bool IsVisible;
    IList<SKBitmap> Images;
    IList<GeometryData> Geometries;
    IList<TextData> Texts;
}
```

### 3. Rendering Process

- Property changes or zoom trigger visual invalidation.
- In the `OnRender` method, all visible layers are traversed in order and drawn to `SKSurface`:
    - Apply zoom transformation to each layer's images, geometries, and text.
    - Set `SKFilterQuality.None` when drawing images to ensure pixel-level rendering without interpolation.
    - For geometries, divide `SKPaint.StrokeWidth` by the zoom factor to maintain consistent visual line width and support sub-pixel width.
- Render the `SKSurface` to Avalonia's drawing context.

### 4. Other Data Structures

```csharp
class GeometryData {
    SKPath Path;
    SKColor Color;
    float StrokeWidth; // Supports sub-pixel
}

class TextData {
    string Text;
    SKPoint Position;
    SKPaint Paint;
}
```

### 5. Zoom Handling

- All drawing coordinates and sizes are multiplied by the zoom factor.
- Line width is divided by the zoom factor, supporting sub-pixel:
    ```csharp
    paint.StrokeWidth = originalWidth / zoom;
    ```
- When drawing images:
    ```csharp
    canvas.DrawBitmap(bitmap, destRect, SKPaint { FilterQuality = SKFilterQuality.None });
    ```

## Usage Example

```csharp
var layer1 = new LayerData {
    Name = "Base Map",
    IsVisible = true,
    Images = new List<SKBitmap> { ... },
    Geometries = new List<GeometryData> { ... },
    Texts = new List<TextData> { ... }
};

var layer2 = new LayerData {
    Name = "Annotations",
    IsVisible = true,
    Images = new List<SKBitmap>(),
    Geometries = new List<GeometryData> { ... },
    Texts = new List<TextData> { ... }
};

var control = new SkiaOffscreenControl {
    Layers = new List<LayerData> { layer1, layer2 },
    Zoom = 2.0
};
```

## Extensibility

- Support for more geometry types or effects.
- Customizable anti-aliasing and rendering options.
- Layers can support transparency, blend modes, and other advanced features.

## References

- [AvaloniaUI 文档](https://docs.avaloniaui.net/)
- [SkiaSharp 文档](https://docs.microsoft.com/en-us/dotnet/api/skiasharp)
