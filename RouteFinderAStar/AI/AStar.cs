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

        // G is the distance between the current node and the start node
        public double G;
        // H is the heuristic — estimated distance from the current node to the end node
        public double H;
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

            G = -1;
            H = -1;
        }
    }

    public class AStar
    {
        List<Node> Graph;
        int[,] ConnectionMatrix;
        int N;

        public AStar(List<Node> graph, int[,] connectionMatrix, int n)
        {
            Graph = graph;
            ConnectionMatrix = connectionMatrix;
            N = n;
        }

        public Stack<Node> FindRoute()
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
                List<Node> children = ChildrenNodes(current);

                foreach (Node child in children)
                {
                    // Check child hasn't already been visited
                    if (!closedList.Exists(x => x.Position == child.Position))
                    {
                        // Calculate G,H
                        child.G = EuclideanDistance(child, current);
                        child.H = EuclideanDistance(child, endNode);

                        // If child isn't in open list
                        bool childInOpenList = false;
                        foreach (var node in openList.UnorderedItems)
                        {
                            if (node.Element.Position == child.Position)
                            {
                                childInOpenList = true;
                            }
                        }

                        if (childInOpenList)
                        {
                            // Is this child's G value lower?
                        } else
                        {
                            // Add child to open list
                            child.Parent = current;
                            openList.Enqueue(child, child.F);
                        }
                    }
                }
            }

            // Determine path
            Stack<Node> Path = new Stack<Node>();
            Node temp = closedList[closedList.IndexOf(current)];
            if (temp == null) return null;
            do
            {
                Path.Push(temp);
                temp = temp.Parent;
            } while (temp != startNode && temp != null);
            return Path;
        }

        public double EuclideanDistance(Node a, Node b)
        {
            return Math.Sqrt((Math.Pow(a.Coordinates.X - b.Coordinates.X, 2) + Math.Pow(a.Coordinates.Y - b.Coordinates.Y, 2)));
        }

        public List<Node> ChildrenNodes(Node current)
        {
            List<Node> children = new List<Node>();

            for (int i = 0; i < N; i++)
            {
                if (ConnectionMatrix[i, current.Position] == 1)
                    children.Add(Graph[i]);
            }

            return children;
        }
    }
}
