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
        public static ImageLearningData PrepareImage(string path, int targetWidth, int targetHeight,int id)
        {

            int paddingRatio = 4;
            //Extract data
            Bitmap original = new Bitmap(path);

            Rectangle rect = new Rectangle(original.Width / paddingRatio,
            original.Height / paddingRatio,
            original.Width - (2*(original.Width / paddingRatio)),
            original.Height - (2*(original.Height / paddingRatio)));

            //Perform cropping and resize
            Bitmap cropped = cropImage(original, rect);
            Bitmap resized = new Bitmap(cropped, new Size(targetWidth, targetHeight));

            cropped.Save("../../../TestResults/gen/c" + id + ".png");
            resized.Save("../../../TestResults/gen/s" + id + ".png");

            BitmapData data = original.LockBits(new Rectangle(0, 0, resized.Width, resized.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
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


            //Free GDI handles
            resized.Dispose();
            cropped.Dispose();
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
            string[] files = Directory.GetFiles(path, "*.jpg");
            ImageLearningData[] result = new ImageLearningData[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                result[i] = PrepareImage(files[i],200,100,i);
            }
            return result;
        }

        private static Bitmap cropImage(Bitmap img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            return bmpImage.Clone(cropArea, bmpImage.PixelFormat);
        }

    }




}