﻿namespace WebFood.Utility
{
    static public class FileUploadHelper
    {
        static public async Task<string> Upload(IFormFile file)
        {
            if (file != null)
            {
                var filename = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                using var fs = new FileStream(@$"wwwroot/uploads/{filename}", FileMode.Create);
                await file.CopyToAsync(fs);
               return @$"/uploads/{filename}"; 
            }

            throw new Exception("File was not uploaded!");
        }

    }
}
