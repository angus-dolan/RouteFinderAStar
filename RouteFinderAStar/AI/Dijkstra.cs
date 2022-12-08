using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteFinder.Dijkstra
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
        public int Position { get; set; }
        public Node Parent { get; set; }
        public double MinDistance { get; set; }

        public List<Node> Children { get; set; }

        public Node(Coordinates coordinates, int position)
        {
            Coordinates = coordinates;
            Position = position;
        }
    }

    public class Dijkstra
    {
        private List<Node> graph;
        private int[,] connectionMatrix;
        private int N;

        public Dijkstra(List<Node> g, int[,] m, int n)
        {
            graph = g;
            connectionMatrix = m;
            N = n;
        }

        public List<int> FindRoute()
        {
            Node start = graph.First();
            Node end = graph.Last();

            // Prepare priority queue
            PriorityQueue<Node, double> priorityQueue = new PriorityQueue<Node, double>();
            double zero = 0.0;
            foreach (Node node in graph)
            {
                node.Parent = null;
                node.MinDistance = 1 / zero; // initialise to infinity
                node.Children = ChildrenNodes(node);

                if (node.Position != start.Position)
                {
                    priorityQueue.Enqueue(node, node.MinDistance);
                }
            }
            start.MinDistance = 0;
            priorityQueue.Enqueue(start, 0);

            // Perform search
            while (priorityQueue.Count != 0)
            {
                Node current = priorityQueue.Dequeue(); // decide current node

                foreach (Node child in current.Children)
                {
                    double childDistance = EuclideanDistance(current, child);
                    double movementCost = current.MinDistance + childDistance;

                    if (movementCost < graph[child.Position].MinDistance)
                    {
                        child.MinDistance = movementCost;
                        child.Parent = current;

                        priorityQueue.Enqueue(child, child.MinDistance);
                    }
                }
            }

            // Determine path
            List<int> path = new List<int>();

            // No path available
            if (end.Parent == null)
            {
                return path;
            }

            // Path available, backtrack to find shortest route
            Node pathNode = end;
            do
            {
                path.Add(pathNode.Position + 1);
                pathNode = pathNode.Parent;
            } while (pathNode != start && pathNode != null);
            path.Add(start.Position + 1);
            path.Reverse();

            return path;
        }

        public double EuclideanDistance(Node a, Node b)
        {
            return Math.Sqrt((Math.Pow(a.Coordinates.X - b.Coordinates.X, 2) + Math.Pow(a.Coordinates.Y - b.Coordinates.Y, 2)));
        }

        public List<Node> ChildrenNodes(Node current)
        {
            List<Node> children = new List<Node>();

            int fromNode = current.Position;
            int toNode;
            int walkable;

            for (int i = 0; i < N; i++)
            {
                toNode = i;
                walkable = connectionMatrix[toNode, fromNode];

                if (walkable == 1)
                {
                    Node child = graph[i];
                    children.Add(child);
                }
            }

            return children;
        }
    }
}
