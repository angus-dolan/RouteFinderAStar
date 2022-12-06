using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RouteFinderAStar.AI
{
    public class Coordinates
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Coordinates(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public class Node
    {
        public Coordinates Coordinates { get; set; }

        // G is the distance between the current node and the start node
        public float G;
        // H is the heuristic — estimated distance from the current node to the end node
        public float H;
        // F is the total cost of the node.
        public float F
        {
            get
            {
                if (G != -1 && H != -1)
                    return G + H;
                else 
                    return -1;
            }
        }

        public Node(Coordinates coordinates)
        {
            Coordinates = coordinates;

            G = -1;
            H = -1;
        }
    }

    public class AStar
    {
        List<Node> Graph;

        public AStar(List<Node> graph)
        {
            Graph = graph;
        }

        public Stack<Node> FindRoute()
        {
            return new Stack<Node>();
        }
    }
}
