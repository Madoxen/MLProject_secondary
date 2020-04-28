using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataPreparer;
using System.Diagnostics;

namespace DataPreparerTests
{
    [TestClass]
    public class DataPreparerTests
    {
        [TestMethod]
        public void Test()
        {
            var a = DataPreparer.DataPreparer.PrepareImages("Resources");
        }
    }
}
