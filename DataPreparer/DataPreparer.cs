namespace DataPreparer
{
    //Prepares data from images
    public static class DataPreparer
    {


        ///<summary>
        ///Prepares one image
        ///</summary>
        public ImageData PrepareImage(string path)
        {
            Bitmap map = new Bitmap(Input);

            Rectangle rect = new Rectangle(0, 0, map.Width, map.Height);
            BitmapData data = map.LockBits(rect, ImageLockMode.ReadWrite, map.PixelFormat);
            int depth = Bitmap.GetPixelFormatSize(data.PixelFormat) / 8; //bytes per pixel
            byte[] buffer = new byte[data.Width * data.Height * depth];

            //copy pixels to buffer
            Marshal.Copy(data.Scan0, buffer, 0, buffer.Length);


            //Copy the buffer back to image
            Marshal.Copy(buffer, 0, data.Scan0, buffer.Length);
            map.UnlockBits(data);
            Output = map;
            
        }



        ///<summary>
        ///Prepares entire directory of images
        ///</summary>
        public ImageData[] PrepareImages(string path)
        {


        }
    }

}