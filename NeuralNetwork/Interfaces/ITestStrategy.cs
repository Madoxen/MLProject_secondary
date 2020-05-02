namespace NeuralNetwork
{
    public interface ITestStrategy
    {
        double Test(double[][] input, double[][] expectedOutput);
        bool CheckHalt();
    }

}