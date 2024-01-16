namespace Shopwise.Helpers;

public static class FileUploadExtension
{
    public static async Task<string> GeneratePhoto(this IFormFile file, params string[] folders)
    {
        string folderPath = Path.Combine(folders);
        string fileName = Guid.NewGuid() + file.FileName;
        string fullPath = Path.Combine(folderPath, fileName);

        using (FileStream stream = new FileStream(fullPath, FileMode.CreateNew))
        {
            await file.CopyToAsync(stream);
        }
        return fileName;
    }


    public static bool ValidateSize(this IFormFile formFile, int MbSize)
    {
        if (formFile.Length > MbSize * 1024 * 1024)
        {
            return false;
        }
        return true;
    }


    public static bool ValidateType(this IFormFile formFile, string type = "image")
    {
        if (formFile.ContentType.Contains(type))
        {
            return true;
        }
        return false;
    }
    public static void DeleteImage(this string fileName, params string[] folders)
    {
        string folderPath = Path.Combine(folders);
        string fullPath = Path.Combine(folderPath, fileName);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }
}
