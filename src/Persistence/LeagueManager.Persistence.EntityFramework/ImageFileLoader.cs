using System;
using System.IO;

namespace LeagueManager.Persistence.EntityFramework
{
    public class ImageFileLoader : IImageFileLoader
    {
        public byte[] LoadImage(string filePath)
        {
            string basePath = $"{ Environment.CurrentDirectory }\\..\\..\\..\\..\\images";
            string imagePath = Path.Combine(basePath, filePath);
            return File.ReadAllBytes(imagePath);
        }
    }
}