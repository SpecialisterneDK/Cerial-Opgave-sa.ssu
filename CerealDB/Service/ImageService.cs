using CerealDB.Models;
using System.Drawing;
using System.IO;
using static CerealDB.CerealContext;

namespace CerealDB.Service
{
    public class ImageService
    {
        static public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new();
#pragma warning disable CA1416 // Validate platform compatibility
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
#pragma warning restore CA1416 // Validate platform compatibility
            return ms.ToArray();
        }
        static public List<ProductImage> LoadImages(string Path)
        {
            int i = 1;
            var list = new List<ProductImage>();
            foreach (string file in Directory.GetFiles(Path))
            {
#pragma warning disable CA1416 // Validate platform compatibility
                Image image = Image.FromFile(file);
#pragma warning restore CA1416 // Validate platform compatibility
                ProductImage imagedata = new() { Id = i, Imagefile = ImageToByteArray(image)};

                list.Add(imagedata);
                i++;
            }
            return list;
        }
    }
}
