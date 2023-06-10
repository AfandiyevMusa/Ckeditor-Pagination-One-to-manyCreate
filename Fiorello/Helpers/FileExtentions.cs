using System;
namespace Fiorello.Helpers
{
	public static class FileExtentions
	{
		public static bool CheckFileType(this IFormFile file, string pattern)
		{
			return file.ContentType.Contains(pattern);
		}

        public static bool CheckFileSize(this IFormFile file, long size)
        {
            return file.Length / 1024 > size;
        }

		public static async Task SaveFileAsync(this IFormFile file, string fileName, string basePath, string folder)
		{
            string path = Path.Combine(basePath + folder + fileName);

            using (FileStream stream = new(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }
	}
}

