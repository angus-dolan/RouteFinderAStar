using System.IO;
using System.Numerics;
using RouteFinderAStar.AI;

class Program
{
    static void Main(string[] args)
    {
        string filepath = Directory.GetCurrentDirectory() + @"\" + "input1" + ".cav";
        string file = File.ReadAllText(filepath);
        BuildSolution(file);

        // Get cav file from args and begin application
        //if (args.Length == 1)
        //{
        //    try
        //    {
        //        string filepath = Directory.GetCurrentDirectory() + @"\" + args[0] + ".cav";
        //        string file = File.ReadAllText(filepath);

        //        BuildSolution(file);
        //    }
        //    catch (FileNotFoundException) { }
        //}
    }

    static void BuildSolution(string file)
    {
        // Parse file contents
        string[] stringArr = file.Split(",");
        int[] intArr = Array.ConvertAll(stringArr, s => int.Parse(s));

        // Define N (number of nodes)
        int N = intArr[0];

        // Create node graph
        List<Node> graph = new List<Node>();
        for (int i = 1; i < N * 2 + 1; i += 2)
        {
            Coordinates coordinates = new Coordinates(intArr[i], intArr[i + 1]);

            graph.Add(new Node(coordinates, graph.Count));
        }

        // Create connection matrix
        int[,] connectionMatrix = new int[N, N];
        int row = 0;
        for (int i = N*2+1; i < intArr.Length; i+=N)
        {

            for (int col = 0; col < N; col++)
            {
                connectionMatrix[row, col] = intArr[i + col];
            }
            row++;
        }


        // Solve with A*, returns 0 when no route can be found
        AStar algorithm = new AStar(graph, connectionMatrix, N);
        Stack<Node> path = algorithm.FindRoute();
        //double distance = algorithm.EuclideanDistance(graph[4], graph[2]);

        // Output results to .csn file
    }
}
