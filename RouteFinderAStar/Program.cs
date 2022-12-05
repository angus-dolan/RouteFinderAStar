using System.IO;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 1)
        {
            try
            {
                string filepath = Directory.GetCurrentDirectory() + @"\" + args[0] + ".cav";
                string file = File.ReadAllText(filepath);

                BuildSolution(file);
            } catch (FileNotFoundException) { }
        }
    }

    static void BuildSolution(string file)
    {
        // Parse file contents

        // Solve with A*, returns 0 when no route can be found

        // Output results to .csn file
    }
}
