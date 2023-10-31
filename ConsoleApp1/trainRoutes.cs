using Microsoft.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Services;
using System.Xml.Linq;

namespace ConsoleApp1
{
    public class trainRoutes
    {
        public static List<ConversionNode> Map = new List<ConversionNode>();
        public static List<Node> Tree = new List<Node>();

        public static int GlobalSuccesfulRoutesCounter = 0; //global counter to keep track of routes for last option

        static void Main(string[] args)
        {

            //string[] input = { "AB5", "BC4", "CD8", "DC8", "DE6", "AD5", "CE2", "EB3", "AE7" };
            //loading input into map
            //LoadMap(input);

            //loading input into map
            string input;
            while (true)
            {
                Console.WriteLine("Enter input (e.g., 'AB5', 'BC6') or 'exit' to quit:");
                input = Console.ReadLine();

                if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                if (input.Length == 3 && char.IsLetter(input[0]) && char.IsLetter(input[1]) && char.IsDigit(input[2]))
                {
                    char char1 = input[0];
                    char char2 = input[1];
                    int number = int.Parse(input[2].ToString());


                    ConversionNode temp = new ConversionNode();
                    temp.source = char.ToUpper(char1);
                    temp.destination = char.ToUpper(char2);
                    temp.distance = number;

                    Map.Add(temp);

                    Console.WriteLine($"Character 1: {char1}");
                    Console.WriteLine($"Character 2: {char2}");
                    Console.WriteLine($"Number: {number}");
                }
                else
                {
                    Console.WriteLine("Invalid input. Input should be in the format 'AB5'.");
                }
            }

            

            //creates the tree structure
            Generate_Tree();

            string mystring = null;
            bool run = true;

            string route = "";
            string source, destination = "";


            while (run) 
            {
                Console.WriteLine("Select Options");
                Console.WriteLine("1) get distance from route");
                Console.WriteLine("2) number trips from x to y with x MAXIMUM stops");
                Console.WriteLine("3) number trips from x to y with EXACTLY x stops");
                Console.WriteLine("4) Length of the shortest route from X to Y in distance");
                Console.WriteLine("5) Number of different routes from X to Y with CERTAIN distance");
                Console.Write("Enter option : ");
                mystring = Console.ReadLine();
                
                switch (mystring) 
                {
                    case "1":
                        Console.WriteLine("Enter Route");
                        route = Console.ReadLine().ToUpper();

                        //function that calculates the distance
                        Console.WriteLine(Distance_to_Route(route));

                        break;
                    case "2":
                        int maxNumberOfStops = 0;

                        //taking in input
                        Console.Write("Enter starting city: ");
                        source = Console.ReadLine().ToUpper();
                        Console.Write("Enter destination city: ");
                        destination = Console.ReadLine().ToUpper();
                        Console.Write("Enter MAXIMUM number of stops: ");
                        maxNumberOfStops = int.Parse(Console.ReadLine());

                        var resultsOfMaxNumberOfStops = Max_number_of_Stops(source[0], destination[0],maxNumberOfStops,0,0);
                        Console.WriteLine(resultsOfMaxNumberOfStops.ToString());

                        break;
                    case "3":
                        int ExactNumberOfStops = 0;

                        //taking in input
                        Console.Write("Enter starting city: ");
                        source = Console.ReadLine().ToUpper();
                        Console.Write("Enter destination city: ");
                        destination = Console.ReadLine().ToUpper();
                        Console.Write("Enter EXACT number of stops: ");
                        ExactNumberOfStops = int.Parse(Console.ReadLine());

                        var resultofExactNumberOfSteps = Exact_number_of_Stops(source[0], destination[0], ExactNumberOfStops,0,new List<string>());
                        int temp = resultofExactNumberOfSteps.Item1.Count();
                        Console.WriteLine(temp);
                        
                        break;
                    case "4":
                        
                        Console.Write("Enter starting city: ");
                        source = Console.ReadLine().ToUpper();
                        Console.Write("Enter destination city: ");
                        destination = Console.ReadLine().ToUpper();
                        Console.WriteLine(Shortest_Path(source[0], destination[0],0));
                        
                        break;
                    case "5":
                        GlobalSuccesfulRoutesCounter = 0; // reset global counter before every use
                        int MaxDistance = 0;
                        Console.Write("Enter starting city: ");
                        source = Console.ReadLine().ToUpper();
                        Console.Write("Enter destination city: ");
                        destination = Console.ReadLine().ToUpper();
                        Console.Write("Enter MAX Distance: ");
                        MaxDistance = int.Parse(Console.ReadLine());
                        Different_Paths(source[0], destination[0], 0, MaxDistance);
                        Console.WriteLine(GlobalSuccesfulRoutesCounter);

                        break;
                    default:
                        run = false;
                        break;

                }
            }

        }


        // this function is used to create the tree
        public static void Generate_Tree() 
        {
            foreach(var city in Map) 
            {
                int index = TreeContains(city.source);
                if (index != -1)
                {
                    Tree[index].destination.Add((city.destination, city.distance));
                }
                else 
                {
                    Node temp = new Node();
                    temp.source = city.source;
                    (char, int) item = (city.destination, city.distance);
                    temp.destination.Add(item);
                    Tree.Add(temp);
                }
            }
        }


        public static void LoadMap(string[] input) 
        {
            //load input into map
            foreach (string city in input)
            {
                ConversionNode temp = new ConversionNode();
                temp.source = city[0];
                temp.destination = city[1];
                temp.distance = city[2] - 48;

                Map.Add(temp);
            }
        }
        
        //Gets the index of the city in the tree
        public static int TreeContains(char city) 
        {
            for (int i = 0; i < Tree.Count; i++) 
            {
                if (Tree[i].source.Equals(city))
                    return i;
            }
            return -1;
        }
        //pretty straight forward just follow the route and add the distances
        //send the current city and next city on the route
        public static string Distance_to_Route(string mystring = null) 
        {
            
            string[] route = null;
            
            route = mystring.Split('-');
            int totalDistance = 0;

            for (int i = 0; i <= route.Length; i++) 
            {
                if (i + 1 >= route.Length) 
                {
                    return "Total Distance " + totalDistance;
                }
                else 
                {
                    int ans = Map_Traversal(route[i][0], route[i+1][0]);
                    if (ans < 0) 
                    {
                        return " NO SUCH ROUTE";
                    }
                    totalDistance += ans;
                }
            }
            return "unexpected error";
        }
        //traverse through the map and get the distance return -1 if source is not in map and -2 if no possible routes to next city
        public static int Map_Traversal(char source, char destination) 
        {
            int index = TreeContains(source);
            if (index == -1) 
            {
                return -1;
            }
            foreach (var t in Tree[index].destination) 
            {
                if (t.Item1.Equals(destination))
                    return t.Item2;
            }


            return -2;
        }

        /// <summary>
        /// recursively go though the map you find the destination then return the route and return based conditions set
        /// </summary>
        public static int Max_number_of_Stops(char currentCity, char destination,int maxNumberOfStops, int currentNumberOfStops, int SuccessfulRoutes)
        {

            //basecase if current number of stops is greater pop out
            if (maxNumberOfStops < currentNumberOfStops)
                return SuccessfulRoutes;

            //if you arrived at your destination pop out
            if (currentCity.Equals(destination) && currentNumberOfStops != 0)
            {
                SuccessfulRoutes++;
                return SuccessfulRoutes;
            }

            int index = TreeContains(currentCity);
            int result = SuccessfulRoutes;

            for (int i = 0; i < Tree[index].destination.Count; i++) 
            {
                //recursive call with the current city set to one of the route cities
                result = Max_number_of_Stops(Tree[index].destination[i].Item1, destination, maxNumberOfStops, currentNumberOfStops+1, SuccessfulRoutes);
                if (result != SuccessfulRoutes) SuccessfulRoutes = result;
            }
            return SuccessfulRoutes;
        }


        //recursivly go though tree until destination is found exactly at the number of stops
        public static (List<string>,int) Exact_number_of_Stops(char currentCity, char destination, int exactNumberOfStops, int currentNumberOfStops, List<string> SuccessfulRoutes)
        {
            //if you arrived at your destination and the exact number of stops pop out
            if (currentCity.Equals(destination) && currentNumberOfStops != 0 && currentNumberOfStops == exactNumberOfStops )
            {
               string temp = currentCity.ToString();
               SuccessfulRoutes.Add(temp);
               return (SuccessfulRoutes, 1);
            }

            //basecase if current number of stops is greater than exact stops needed
            if (exactNumberOfStops < currentNumberOfStops)
                return (SuccessfulRoutes, -1);

            int index = TreeContains(currentCity);
            var result = (SuccessfulRoutes, 0);

            for (int i = 0; i < Tree[index].destination.Count; i++)
            {
                if (result.Item2 == 1)
                {
                    SuccessfulRoutes[SuccessfulRoutes.Count-1] = currentCity+ SuccessfulRoutes[SuccessfulRoutes.Count - 1];
                }
                result = Exact_number_of_Stops(Tree[index].destination[i].Item1, destination, exactNumberOfStops, currentNumberOfStops + 1, SuccessfulRoutes);
                if (result.Item2 == 1)
                {
                    SuccessfulRoutes[SuccessfulRoutes.Count - 1] = currentCity + SuccessfulRoutes[SuccessfulRoutes.Count - 1];
                }
            }
            return result;
        }


        //this is just UCS uniform cost search basically just have a sorted destination list by distance of course and go, you can optimize this by having a global fringe and check that before you pick your city
        //but i did not realize that until 4:20AM and I just wanted to finish this
         public static int Shortest_Path(char currentCity, char destination, int currentdistance,int firstTime = 0)
        {
            //if you arrived at your destination and pop out
            if (currentCity.Equals(destination) && firstTime != 0)
            {
               return currentdistance;
            }

            int index = TreeContains(currentCity);
            var result = 0;

            //inorder to find the shortest path you have to sort the destinations by distance least to greatest
            Tree[index].destination.Sort((x, y) => x.Item2.CompareTo(y.Item2));

            for (int i = 0; i < Tree[index].destination.Count; i++)
            {
                result = Shortest_Path(Tree[index].destination[i].Item1, destination ,currentdistance + Tree[index].destination[i].Item2,1);
                if (result != 0 && result != -1)
                {
                    return result;
                }
                
            }
            return result;
        }


        //basically the shortest path except with the sorting and the only condition is to reach the max distance
        public static int Different_Paths(char currentCity, char destination, int currentdistance, int maxDistance, int firstTime = 0)
        {

            if (currentdistance >= maxDistance) 
            {
                return -1; //return -1 on negative cases
            }
            
            //if you arrived at your destination and pop out
            if (currentCity.Equals(destination) && firstTime != 0)
            {
                ++GlobalSuccesfulRoutesCounter; // a global counter to keep track of Successful routes
            }

            int index = TreeContains(currentCity);
            var result = 0;

            for (int i = 0; i < Tree[index].destination.Count; i++)
            {
                result = Different_Paths(Tree[index].destination[i].Item1, destination, currentdistance + Tree[index].destination[i].Item2, maxDistance, 1);
                if (result == -1)
                {
                    result = 0;
                }

            }
            return result;
        }

        public static void clearGlobalRoutesCounter() { GlobalSuccesfulRoutesCounter = 0; }//made only for the unit test this is how come i wanted to avoid using global variables but simply fix

    }




    //node to load in all values
    public class ConversionNode
    {
        public char source;
        public char destination;
        public int distance = 0;
    }

    //the node that the we construct the tree with
    public class Node 
    {
        public char source;
        public List<(char, int)> destination = new List<(char, int)>();
        
    }

}
