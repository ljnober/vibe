# Utils

工具层，存放通用辅助方法、扩展函数等，便于全局复用。

---

## 导入导出服务 API 文档

### 1. 接口定义

[`IImportExportService`](IImportExportService.cs:19) 提供主流矢量/位图格式的导入导出能力，支持 SVG、PNG、JPG。

#### 枚举类型

```csharp
public enum ImageFormat
{
    Svg,
    Png,
    Jpg
}
```

#### 接口方法

```csharp
Task<IReadOnlyList<ICanvasElement>> ImportAsync(Stream stream, ImageFormat format)
```
- **参数**
  - `stream`：输入流，需包含有效的图片或 SVG 数据。
  - `format`：指定导入格式（Svg/Png/Jpg）。
- **返回**
  - 导入的 `ICanvasElement` 列表，可能为 `ImageData` 或 `GeometryData`。
- **异常**
  - `ArgumentNullException`：stream 为空。
  - `InvalidDataException`：数据解码失败或格式不支持。
  - `NotSupportedException`：不支持的格式。

```csharp
Task ExportAsync(IEnumerable<ICanvasElement> elements, Stream stream, ImageFormat format)
```
- **参数**
  - `elements`：要导出的元素集合（支持 `ImageData`、`GeometryData`）。
  - `stream`：输出流。
  - `format`：导出格式（Svg/Png/Jpg）。
- **返回**
  - 无（异步任务）。
- **异常**
  - `ArgumentNullException`：参数为空。
  - `InvalidDataException`：无可导出内容或编码失败。
  - `NotSupportedException`：不支持的格式。

### 2. 实现说明

[`ImportExportService`](ImportExportService.cs:14) 实现了上述接口，支持如下特性：

- **SVG 导入**：基础支持 `<rect>`、`<ellipse>` 元素，自动转为 `GeometryData`。
- **PNG/JPG 导入**：解码为 `ImageData`。
- **SVG 导出**：将所有 `GeometryData` 导出为 SVG 图形。
- **PNG/JPG 导出**：导出第一个 `ImageData`，或合成所有元素为图片（当前实现导出第一个）。

#### 典型用法

```csharp
// 导入 PNG
using var fs = File.OpenRead("test.png");
var service = new ImportExportService();
var elements = await service.ImportAsync(fs, ImageFormat.Png);

// 导出为 SVG
using var outStream = File.Create("out.svg");
await service.ExportAsync(elements, outStream, ImageFormat.Svg);
```

---

## 导入导出功能使用说明

### 1. 主界面操作指引

1. 在主界面点击“导入”按钮，选择 SVG/PNG/JPG 文件，即可将图像或图形导入画布。
2. 编辑完成后，点击“导出”按钮，选择导出格式（SVG/PNG/JPG），保存到本地。
3. 支持批量导入，导入后元素自动添加到当前图层。

### 2. 支持格式

- **导入**：SVG（基础图形）、PNG、JPG
- **导出**：SVG（仅图形）、PNG、JPG

### 3. 常见问题

- **Q: 导入复杂 SVG 无法完全还原？**  
  A: 当前仅支持基础 `<rect>`、`<ellipse>`，复杂路径暂不支持。
- **Q: 导出 PNG/JPG 只包含部分内容？**  
  A: 目前仅导出第一个 `ImageData`，后续可扩展为合成所有元素。
- **Q: 导入失败/格式不支持？**  
  A: 请确认文件格式及内容有效，或联系开发者扩展支持。

---

## 开发者扩展指引

### 1. 新增格式适配

如需支持更多格式（如 BMP、GIF），可扩展 `ImageFormat` 枚举，并在 [`ImportExportService`](ImportExportService.cs:14) 的 `ImportAsync`/`ExportAsync` 方法中添加对应分支：

```csharp
// 1. 扩展枚举
public enum ImageFormat
{
    Svg,
    Png,
    Jpg,
    Bmp // 新增
}

// 2. 在 ImportAsync/ExportAsync 中添加 case
case ImageFormat.Bmp:
    // 实现 BMP 解析或编码逻辑
    break;
```

### 2. 扩展服务方法

如需支持更多元素类型或自定义导入导出逻辑，可：

- 扩展 `ICanvasElement` 及其实现。
- 在 `ImportExportService` 中解析/生成对应类型。
- 保持接口兼容性，建议通过继承或组合方式扩展。

### 3. 单元测试建议

- 建议在 `Tests/AvaloniaAsyncDrawing.Tests/ImportExportServiceTests.cs` 中补充新格式的导入导出测试用例，确保兼容性与稳定性。

---

如需进一步扩展或遇到问题，欢迎提交 Issue 或 PR。