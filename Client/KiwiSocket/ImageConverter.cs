using System;
using System.Collections.Generic;
using System.Drawing;
using System.DrawingCore;
using System.DrawingCore.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
   public static class ImageConverter
    {
        public static Bitmap GetImageFromBase64(string base64string)
        {
            byte[] b = Convert.FromBase64String(base64string);
            MemoryStream ms = new MemoryStream(b);
            Bitmap bitmap = new Bitmap(ms);
            return bitmap;
        }
        public static string GetBase64FromImagePath(string imagefile)
        {
            string strbaser64 = "";
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    Bitmap bmp = new Bitmap(imagefile);
                    bmp.Save(ms, ImageFormat.Png);
                    byte[] arr = new byte[ms.Length];
                    ms.Position = 0;
                    ms.Read(arr, 0, (int)ms.Length);                 
                    strbaser64 = Convert.ToBase64String(arr);
                }               
            }
            catch (Exception)
            {
                throw new Exception("Something wrong during convert!");
            }
            return strbaser64;
        }       
    }
}
