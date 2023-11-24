using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyNet
{
    internal class Search
    {

        class Node
        {
            public int X;
            public int Y;
            public int F;
            public int G;
            public int H;
            public Node Parent;

            public Node(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        class Program
        {
            static int heuristic(Node a, Node b)
            {
                return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
            }

            static bool isValid(Node n, Node[,] grid)
            {
                if (n.X >= 0 && n.X < grid.GetLength(0) && n.Y >= 0 && n.Y < grid.GetLength(1))
                {
                    if (grid[n.X, n.Y] == 0)
                    {
                        return true;
                    }
                }
                return false;
            }

            static void AStar(Node start, Node goal, Node[,] grid)
            {
                List<Node> openSet = new List<Node>();
                HashSet<Node> closedSet = new HashSet<Node>();
                openSet.Add(start);

                while (openSet.Count > 0)
                {
                    Node currentNode = openSet[0];
                    for (int i = 1; i < openSet.Count; i++)
                    {
                        if (openSet[i].F < currentNode.F)
                        {
                            currentNode = openSet[i];
                        }
                    }

                    openSet.Remove(currentNode);
                    closedSet.Add(currentNode);

                    if (currentNode.X == goal.X && currentNode.Y == goal.Y)
                    {
                        // If we reach the goal, we can use the Parent property to get the shortest path.
                        // Alternatively, we can also use a different approach to get the path.
                        Console.WriteLine("Path found.");
                        return;
                    }

                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            if (i == 0 && j == 0)
                            {
                                continue;
                            }

                            Node neighbor = new Node(currentNode.X + i, currentNode.Y + j);
                            if (isValid(neighbor, grid))
                            {
                                if (!closedSet.Contains(neighbor))
                                {
                                    neighbor.G = currentNode.G + 1;
                                    neighbor.H = heuristic(neighbor, goal);
                                    neighbor.F = neighbor.G + neighbor.H;
                                    neighbor.Parent = currentNode;

                                    if (!openSet.Contains(neighbor))
                                    {
                                        openSet.Add(neighbor);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            static void Main()
            {
                Node[,] grid = new Node[100, 100];
                for (int i = 0; i < 100; i++)
                {
                    for (int j = 0; j < 100; j++)
                    {
                        grid[i, j] = new Node(i, j);
                    }
                }

                Node start = grid[0, 0];
                Node goal = grid[99, 99];
                AStar(start, goal, grid);
            }
        }
    }


}
