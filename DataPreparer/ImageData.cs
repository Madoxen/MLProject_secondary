namespace DataPreparer
{
    public class ImageData
    {


        /// <summary>
        /// Array containing raw data
        /// n = R, n+1 = G, n+2 = B;
        /// </summary>
        public ReadOnlyCollection<byte> rawData;


        /// <summary>
        /// Array containing only R (red) pixel data
        /// </summary>
        public ReadOnlyCollection<byte>  R;
        /// <summary>
        /// Array containing only G (green) pixel data
        /// </summary>
        public ReadOnlyCollection<byte>  G;
        /// <summary>
        /// Array containing only B (blue) pixel data
        /// </summary>
        public ReadOnlyCollection<byte>  B;

        /// <summary>
        /// Label for this image
        /// </summary>
        public string label;


        public readonly int Width;
        public readonly int Height;

        ImageData(int Width, int Height)
        {
            this.Width = Width;
            this.Height = Height;
        }

    }
}
