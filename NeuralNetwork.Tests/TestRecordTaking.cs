using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetwork;

namespace NeuralNetwork.Tests
{

    [TestClass]
    public class TestRecordTaking
    {
        [TestMethod]
        public void TestHighestHitTest()
        {
            Network net = new Network(0.05, 0.5, -1.0, 1.0, 4, new int[] { 4, 4, 4 }, 2);
            //    net.testStrategy = new HighestHitTest(net);
            net.testStrategy = new HighestHitTest(net);

            double[][][] data = Loader.Load("data.csv");
            net.Train(data, 3000);

        }

        [TestMethod]
        public void TestMeanErrorTest()
        {
            Network net = new Network(0.05, 0.5, -1.0, 1.0, 4, new int[] { 4, 4, 4 }, 2);
            //    net.testStrategy = new HighestHitTest(net);
            net.testStrategy = new MeanErrorTest(net);

            double[][][] data = Loader.Load("data.csv");
            net.Train(data, 3000);

        }
    }
}
