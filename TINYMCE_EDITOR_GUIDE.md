# TinyMCE Rich Text Editor Integration Guide

## Overview
TinyMCE is now integrated into your project. It provides a rich text editor for textarea elements, allowing users to format text, add links, images, and more.

## How to Use

### 1. **Basic Usage - Add Editor to Any Textarea**
Simply add the CSS class `rich-editor` to any textarea element:

```html
<textarea class="rich-editor" name="description"></textarea>
```

### 2. **In Razor Views - Example**
```razor
<div class="form-group">
    <label for="description">Mô tả sản phẩm</label>
    <textarea class="form-control rich-editor" id="description" name="description" required></textarea>
</div>
```

### 3. **With Pre-filled Content**
```razor
<textarea class="form-control rich-editor" name="content">@Model.ExistingContent</textarea>
```

### 4. **For Admin/Staff Areas**
```razor
<!-- In Areas/Admin/Views/CarsManagement/Create.cshtml -->
<textarea class="form-control rich-editor" name="carDescription"></textarea>

<!-- In Areas/Staff/Views/Dashboard/Index.cshtml -->
<textarea class="form-control rich-editor" name="notes"></textarea>
```

## Features Available

The editor includes these formatting tools:
- **Text Formatting**: Bold, Italic, Background Color
- **Alignment**: Left, Center, Right, Justify
- **Lists**: Bullet and Numbered Lists
- **Links & Media**: Insert links, images, and videos
- **Tables**: Create and format tables
- **Code**: View/edit HTML source code
- **Undo/Redo**: Full history management

## Configuration Details

**File Modified**: `wwwroot/js/site.js`
**CDN**: `https://cdn.tiny.cloud/1/no-api-key/tinymce/7/tinymce.min.js`

### Current Settings:
- **Height**: 400px (configurable)
- **Language**: Vietnamese (vi)
- **Plugins**: advlist, autolink, lists, link, image, charmap, preview, anchor, searchreplace, visualblocks, code, fullscreen, insertdatetime, media, table, help, wordcount
- **Toolbar**: Comprehensive with formatting, alignment, lists, media, and utilities

## Getting an API Key (Optional but Recommended)

The current setup uses `no-api-key`. For better features and reliability:

1. Visit: https://www.tiny.cloud/auth/signup/
2. Sign up for a free account
3. Get your API key
4. Replace `no-api-key` in `_Layout.cshtml` with your key:
```html
<script src="https://cdn.tiny.cloud/1/YOUR_API_KEY/tinymce/7/tinymce.min.js" referrerpolicy="origin"></script>
```

## Examples in Your Project

### Example 1: Product Description (CarsManagement)
```razor
<div class="form-group mb-3">
    <label for="carDescription">Chi tiết mô tả xe</label>
    <textarea class="form-control rich-editor" id="carDescription" name="description" rows="8">@Model.Car?.Description</textarea>
</div>
```

### Example 2: Content Page (CMS)
```razor
<div class="form-group">
    <label for="pageContent">Nội dung trang</label>
    <textarea class="form-control rich-editor" id="pageContent" name="content">@Model.ContentBlock?.Content</textarea>
</div>
```

### Example 3: Form Notes
```razor
<textarea class="form-control rich-editor" name="additionalNotes" placeholder="Ghi chú bổ sung..."></textarea>
```

## Retrieving Content

When the form is submitted, the textarea value will contain the rich HTML content:

### In Controller
```csharp
[HttpPost]
public IActionResult Create(string description)
{
    // description will contain HTML from TinyMCE
    var htmlContent = description;
    // Save to database...
}
```

### Display HTML Content in View
```razor
@Html.Raw(Model.Description)
```

## Styling the Editor

To customize the appearance, modify the editor initialization in `site.js`:

```javascript
tinymce.init({
    selector: 'textarea.rich-editor',
    height: 500,  // Change height
    content_style: 'body { font-family:Calibri,sans-serif; font-size:16px }', // Change fonts
    // ... other settings
});
```

## Browser Compatibility

TinyMCE 7 supports:
- Chrome/Edge (Latest)
- Firefox (Latest)
- Safari (Latest)
- Mobile browsers (with touch support)

## Troubleshooting

**Issue**: Editor not appearing?
- ✅ Check that textarea has class `rich-editor`
- ✅ Verify TinyMCE CDN is loaded (check browser console)
- ✅ Check browser console for JavaScript errors

**Issue**: Vietnamese language not working?
- The `language: 'vi'` is already configured
- Language file loads automatically from TinyMCE CDN

**Issue**: Images not uploading?
- TinyMCE free tier doesn't include image upload
- Configure image_upload_handler in site.js for custom upload
- Or use image picker from external URLs

## Security Notes

⚠️ **Important**: When displaying user-generated HTML content:
```razor
@Html.Raw(Model.Description)  // Only use with trusted content
```

For user input, sanitize before displaying to prevent XSS attacks:
```csharp
// Use a library like HtmlSanitizer
var sanitizer = new HtmlSanitizer();
var safe = sanitizer.Sanitize(userContent);
```

## Files Modified

1. ✅ `Views/Shared/_Layout.cshtml` - Added TinyMCE CDN script
2. ✅ `wwwroot/js/site.js` - Added TinyMCE initialization

## Next Steps

1. Find textareas in your project that need rich editing
2. Add class `rich-editor` to them
3. Test the editor functionality
4. Get an API key from tiny.cloud for production

Happy editing! 🎉
