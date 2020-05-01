using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataPreparer;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;


namespace DataPreparerTests
{
    [TestClass]
    public class DataPreparerTests
    {
        [TestMethod]
        public void TestDataCount()
        {
            ImageLearningData ld = DataPreparer.ImageDataPreparer.PrepareImage("Resources/test_1.jpg",200,100);

            Assert.AreEqual(200, ld.width);
            Assert.AreEqual(100, ld.height);
            Assert.AreEqual(60000, ld.data.Count); //200 * 100 * 3 (BGR)

        }

        [TestMethod]
        public void TestDataCorrectness()
        {
            var a = DataPreparer.ImageDataPreparer.PrepareImage("Resources/test_1.jpg",200,100);
            Bitmap b = new Bitmap("Resources/target_1.bmp");
            int dataPos = 0;
            for (int i = 0; i < b.Height; i++)
            {
                for (int j = 0; j < b.Width; j++)
                {
                    Color c = b.GetPixel(j,i);
                    Assert.AreEqual(c.B, a.data[dataPos] * 255, 1);
                    Assert.AreEqual(c.G, a.data[dataPos + 1] * 255, 1);
                    Assert.AreEqual(c.R, a.data[dataPos + 2] * 255, 1);
                    dataPos += 3;
                }
            }
        }
    }
}
