using System.IO;
using System.Numerics;
using RouteFinder.Dijkstra;

class Program
{
    static void Main(string[] args)
    {
        //string filepath = Directory.GetCurrentDirectory() + @"\" + "generated500-3" + ".cav";
        //string data = File.ReadAllText(filepath);
        //BuildSolution(data);

        // Get cav data from args and begin application
        if (args.Length == 1)
        {
            try
            {
                string filename = args[0];
                string filepath = Directory.GetCurrentDirectory() + @"\" + filename + ".cav";
                string data = File.ReadAllText(filepath);

                BuildSolution(data, filename);
            }
            catch (FileNotFoundException) { }
        }
    }

    static void BuildSolution(string data, string filename)
    {
        // Parse cav file contents
        string[] stringArr = data.Split(",");
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

        int[,] connectionMatrix = new int[N, N];
        int row = 0;
        for (int i = N * 2 + 1; i < intArr.Length; i += N)
        {

            for (int col = 0; col < N; col++)
            {
                connectionMatrix[row, col] = intArr[i + col];
            }
            row++;
        }

        // Use Dijkstra to find shortest path
        Dijkstra algorithm = new Dijkstra(graph, connectionMatrix, N);
        List<int> path = algorithm.FindRoute();

        // Output results to .csn file
        string filepath = Directory.GetCurrentDirectory() + @"\" + filename + ".csn";
        if (path.Count == 0)
        {
            // No path found
            File.WriteAllText(filepath, "0");
        }
        else
        {
            // Path found
            String strPath = "";
            foreach (int i in path)
            {
                strPath += i + " ";
            }

            File.WriteAllText(filepath, strPath);
        }
    }
}
