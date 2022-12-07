using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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

        public Node Parent { get; set; }

        // Node position in .cav file (0,1,2...)
        public int Position { get; set; }

        // Weight is the distance between current node and parent node
        public double Weight { get; set; }
        // G is the distance between the current node and the start node
        public double G { get; set; }
        // H is the heuristic — estimated distance from the current node to the end node
        public double H { get; set; }
        // F is the total cost of the node.
        public double F
        {
            get
            {
                if (G != -1 && H != -1)
                    return G + H;
                else 
                    return -1;
            }
        }

        public Node(Coordinates coordinates, int position)
        {
            Parent = null;
            Coordinates = coordinates;
            Position = position;

            G = 0;
            H = -1;
        }
    }

    public class AStar
    {
        public List<Node> Graph;
        public int[,] ConnectionMatrix;
        public int N;

        public AStar(List<Node> graph, int[,] connectionMatrix, int n)
        {
            Graph = graph;
            ConnectionMatrix = connectionMatrix;
            N = n;
        }

        public List<int> FindRoute()
        //public void FindRoute()
        {
            // Open list is all generated nodes
            PriorityQueue<Node, double> openList = new PriorityQueue<Node, double>();
            // Closed list is all visited nodes (might need bloom filter)
            List<Node> closedList = new List<Node>();

            Node startNode = Graph.First();
            Node endNode = Graph.Last();
            Node current = startNode;

            // Add the start node to queue
            openList.Enqueue(startNode, startNode.F);

            while (openList.Count != 0 && !closedList.Exists(target => target.Coordinates == endNode.Coordinates))
            {
                // Get current node
                current = openList.Dequeue();
                closedList.Add(current);

                // Generate children
                List<Node> children = ChildrenNodes(current, endNode, closedList);

                foreach (Node child in children)
                {
                    // Check child hasn't already been visited
                    if (!closedList.Exists(x => x.Position == child.Position))
                    {
                        // If child isn't in open list
                        bool childPresent = false;
                        foreach (var node in openList.UnorderedItems)
                        {
                            if (node.Element.Position == child.Position)
                            {
                                childPresent = true;
                            }
                        }

                        if (!childPresent)
                        {
                            child.Parent = current;
                            child.Weight = EuclideanDistance(child, current);
                            child.G = child.Parent.G + child.Weight;
                            child.H = EuclideanDistance(child, endNode);

                            openList.Enqueue(child, child.F);
                        }
                    }
                }
            }

            // Determine path
            List<int> Path = new List<int>();
            Node temp = closedList[closedList.IndexOf(current)];
            double Length = 0;

            if (temp == null) return Path;
            do
            {
                Path.Add(temp.Position + 1);
                Length += temp.Weight;
                temp = temp.Parent;
                
            } while (temp.Position != startNode.Position && temp != null);
            Path.Add(1);
            Path.Reverse();

            Console.WriteLine("Length: " + Length);

            return Path;
        }

        public double EuclideanDistance(Node a, Node b)
        {
            return Math.Sqrt((Math.Pow(a.Coordinates.X - b.Coordinates.X, 2) + Math.Pow(a.Coordinates.Y - b.Coordinates.Y, 2)));
        }

        public List<Node> ChildrenNodes(Node current, Node endNode, List<Node> closedList)
        {
            List<Node> children = new List<Node>();

            for (int i = 0; i < N; i++)
            {
                if (ConnectionMatrix[i, current.Position] == 1)
                {
                    Node child = Graph[i];
                    if (current.Parent == null || child.Position != current.Parent.Position)
                    {
                        children.Add(child);
                    }
                }
            }

            return children;
        }
    }
}
