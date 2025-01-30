using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ECommerceAPI.Application.Helpers
{
    public static class FileUploadHelper
    {
        public static async Task<string> UploadImageAsync(IFormFile file, string directoryPath, string existingImageUrl = null)
        {
            if (file == null || file.Length == 0)
                return null;

            // Eski resmi silme işlemi (varsa)
            if (!string.IsNullOrEmpty(existingImageUrl))
            {
                var existingFilePath = Path.Combine(directoryPath, Path.GetFileName(existingImageUrl));
                if (File.Exists(existingFilePath))
                {
                    File.Delete(existingFilePath); // Eski dosyayı sil
                }
            }

            // Klasör yoksa oluştur
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            // Yeni dosya yolu oluştur
            var fileName = Path.GetFileName(file.FileName);
            var path = Path.Combine(directoryPath, fileName);

            // Dosyayı kaydet
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Yüklenen dosyanın URL'si
            return $"/images/{fileName}";
        }

    }
}
