using ConsoleApp1;

namespace TestProject2
{
    public class UnitTest1
    {
        [Theory]
        [InlineData("A-B-C", "Total Distance 9", new string[] { "AB5", "BC4", "CD8", "DC8", "DE6", "AD5", "CE2", "EB3", "AE7" })]
        [InlineData("A-D", "Total Distance 5", new string[] { "AB5", "BC4", "CD8", "DC8", "DE6", "AD5", "CE2", "EB3", "AE7" })]
        [InlineData("A-D-C", "Total Distance 13", new string[] { "AB5", "BC4", "CD8", "DC8", "DE6", "AD5", "CE2", "EB3", "AE7" })]
        [InlineData("A-E-B-C-D", "Total Distance 22", new string[] { "AB5", "BC4", "CD8", "DC8", "DE6", "AD5", "CE2", "EB3", "AE7" })]
        [InlineData("A-E-D", " NO SUCH ROUTE", new string[] { "AB5", "BC4", "CD8", "DC8", "DE6", "AD5", "CE2", "EB3", "AE7" })]
        public void Distance_To_Route_Test(string input, string expectedResponse,string[] map)
        {
            ConsoleApp1.trainRoutes trainRoutes = new ConsoleApp1.trainRoutes();
            
            trainRoutes.LoadMap(map);
            trainRoutes.Generate_Tree();

            Assert.Equal(trainRoutes.Distance_to_Route(input), expectedResponse);
            
        }


        [Theory]
        [InlineData('C','C',3, "2", new string[] { "AB5", "BC4", "CD8", "DC8", "DE6", "AD5", "CE2", "EB3", "AE7" })]
        public void Number_Of_Trips_Max_Stop(char StartingCity, char Destination, int MaxNumberStops, string expectedResponse, string[] map)
        {
            ConsoleApp1.trainRoutes trainRoutes = new ConsoleApp1.trainRoutes();

            trainRoutes.LoadMap(map);
            trainRoutes.Generate_Tree();

            var result = trainRoutes.Max_number_of_Stops(StartingCity, Destination, MaxNumberStops, 0, 0).ToString();

            Assert.Equal(result, expectedResponse);

        }
        
        [Theory]
        [InlineData('A','C',4, "3", new string[] { "AB5", "BC4", "CD8", "DC8", "DE6", "AD5", "CE2", "EB3", "AE7" })]
        public void Number_Of_Trips_Exact_Stops(char StartingCity, char Destination, int ExactNumberStops, string expectedResponse, string[] map)
        {
            ConsoleApp1.trainRoutes trainRoutes = new ConsoleApp1.trainRoutes();

            trainRoutes.LoadMap(map);
            trainRoutes.Generate_Tree();

            var resultofExactNumberOfSteps = trainRoutes.Exact_number_of_Stops(StartingCity, Destination, ExactNumberStops, 0, new List<string>());
            var result = resultofExactNumberOfSteps.Item1.Count().ToString();
            
            Assert.Equal(result, expectedResponse);

        }
        
        
        [Theory]
        [InlineData('A','C', "9", new string[] { "AB5", "BC4", "CD8", "DC8", "DE6", "AD5", "CE2", "EB3", "AE7" })]
        [InlineData('B','B', "9", new string[] { "AB5", "BC4", "CD8", "DC8", "DE6", "AD5", "CE2", "EB3", "AE7" })]
        public void Shortest_Path(char StartingCity, char Destination, string expectedResponse, string[] map)
        {
            ConsoleApp1.trainRoutes trainRoutes = new ConsoleApp1.trainRoutes();

            trainRoutes.LoadMap(map);
            trainRoutes.Generate_Tree();

            var resultofExactNumberOfSteps = trainRoutes.Shortest_Path(StartingCity, Destination, 0);
            var result = resultofExactNumberOfSteps.ToString();
            
            Assert.Equal(result, expectedResponse);

        }


        [Theory]
        [InlineData('C', 'C', 30, "7", new string[] { "AB5", "BC4", "CD8", "DC8", "DE6", "AD5", "CE2", "EB3", "AE7" })]
        public void Number_Of_Unique_Routes(char StartingCity, char Destination, int MaxDistance, string expectedResponse, string[] map)
        {
            ConsoleApp1.trainRoutes trainRoutes = new ConsoleApp1.trainRoutes();

            trainRoutes.LoadMap(map);
            trainRoutes.Generate_Tree();
            trainRoutes.clearGlobalRoutesCounter();

            var resultofExactNumberOfSteps = trainRoutes.Different_Paths(StartingCity, Destination, 0, MaxDistance);
            var result = trainRoutes.GlobalSuccesfulRoutesCounter.ToString();

            Assert.Equal(result, expectedResponse);

        }




    }

}