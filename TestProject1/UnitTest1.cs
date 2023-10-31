namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            ConsoleApp1.trainRoutes.LoadMap(Map);
            Distance_to_Route(input);
        }
    }
}