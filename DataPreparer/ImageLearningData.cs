using System.Collections.ObjectModel;


namespace DataPreparer
{
    public struct ImageLearningData
    {


        /// <summary>
        /// Array containing raw data
        /// in BGR format
        /// </summary>
        public ReadOnlyCollection<double> data;


        /// <summary>
        /// Label for this image
        /// </summary>
        public string label;
        /// <summary>
        /// Sample number of this image
        /// </summary>
        public int number;

        public readonly int width;
        public readonly int height;


        /// <summary>
        /// Creates new instance of Image data
        /// </summary>ImageData result = 
        /// in R = nth pixel; G = (n+1)th pixel; B = (n+2)th pixel</param>
        public ImageLearningData(int Width, int Height, byte[] rawData, string label, int number)
        {
            this.width = Width;
            this.height = Height;
            double[] d = new double[rawData.Length];
            
            for(int i = 0; i < rawData.Length; i++)
            {
                d[i] = ((double)rawData[i]/255.0);
            }

            this.data = new ReadOnlyCollection<double>(d);
            //Assign label
            this.label = label;
            this.number = number;
        }

    }
}
