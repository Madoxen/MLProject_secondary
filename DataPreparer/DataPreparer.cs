using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO;
using System;

namespace DataPreparer
{
    //Prepares data from images
    public static class DataPreparer
    {


        ///<summary>
        ///Prepares one image
        ///Uses file name as a label, label search terminates at '_' after '_' signifies sample number
        ///</summary>
        public static ImageLearningData PrepareImage(string path)
        {

            //Extract data
            Bitmap original = new Bitmap(path);
            // throw new Exception("Bad bitmap pixel format: not 24bppRGB");
            Rectangle rect = new Rectangle(0, 0, original.Width, original.Height);
            BitmapData data = original.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int depth = Bitmap.GetPixelFormatSize(data.PixelFormat) / 8; //bytes per pixel
            byte[] buffer = new byte[data.Width * data.Height * depth];
            //copy pixels to buffer
            Marshal.Copy(data.Scan0, buffer, 0, buffer.Length);
            original.UnlockBits(data);

            //Extract label
            string fileName = Path.GetFileNameWithoutExtension(path);
            string[] tokens = fileName.Split("_");
            string label = tokens[0];
            int number = Convert.ToInt32(tokens[1]);

            original.Dispose();
            return new ImageLearningData(data.Width, data.Height, buffer, label, number);
        }



        ///<summary>
        ///Prepares entire directory of images
        ///</summary>
        /// <param name="path">Path to directory that contains images</param>
        /// <returns></returns>
        public static ImageLearningData[] PrepareImages(string path)
        {
            string[] files = Directory.GetFiles(path, "*.png");
            ImageLearningData[] result = new ImageLearningData[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                result[i] = PrepareImage(files[i]);
            }
            return result;
        }




        public static Bitmap RemoveAlphaChannel(Bitmap bitmap)
        {
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            Bitmap bitmapDest = (Bitmap)new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format24bppRgb);
            BitmapData data = bitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData dataDest = bitmapDest.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            NativeMethods.CopyMemory(dataDest.Scan0, data.Scan0, (uint)data.Stride * (uint)data.Height);
            bitmap.UnlockBits(data);
            bitmapDest.UnlockBits(dataDest);
            return bitmapDest;
        }

    }



}