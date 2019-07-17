using System;
using System.IO;

namespace LeagueManager.Persistence.EntityFramework
{
    public class ImageFileLoader : IImageFileLoader
    {
        public byte[] LoadImage(string fileName)
        {
            string path = $"{ Environment.CurrentDirectory }\\..\\..\\..\\images\\country-flags\\png100px\\{ fileName }";
            return File.ReadAllBytes(path);
        }
    }
}