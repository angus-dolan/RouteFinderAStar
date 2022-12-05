using System.IO;
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
        //    } catch (FileNotFoundException) { }
        //}
    }

    static void BuildSolution(string file)
    {
        // Parse file contents
        string[] stringArr = file.Split(",");
        int[] intArr = Array.ConvertAll(stringArr, s => int.Parse(s));

        // Define N (number of nodes)
        int N = intArr[0];

        // Store all node coordinates
        List<int[]> graph = new List<int[]>();
        for (int i = 1; i < N*2+1; i+=2)
        {
            int[] coordinates = { intArr[i], intArr[i+1] };
            graph.Add(coordinates);
        }

        // Store connection matrix
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

        // Output results to .csn file
    }
}
