using Microsoft.AspNetCore.Http;

namespace CarSales.Web.Helpers
{
    public static class FileHelper
    {
        public static async Task<string?> SaveImageAsync(IFormFile? file, string webRootPath)
        {
            if (file == null || file.Length == 0)
                return null;

            string extension = Path.GetExtension(file.FileName).ToLower();
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };

            if (!allowedExtensions.Contains(extension))
                throw new Exception("Chỉ cho phép file jpg, jpeg, png, webp");

            string folderPath = Path.Combine(webRootPath, "uploads", "cars");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string fileName = Guid.NewGuid() + extension;
            string fullPath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return "/uploads/cars/" + fileName;
        }
    }
}