namespace NeuralNetwork
{
    public interface ITestStrategy
    {
        double CurrentRecord {get;}
        double Test(double[][] input, double[][] expectedOutput);
        bool CheckHalt();
        bool CheckRecord();
    }

}