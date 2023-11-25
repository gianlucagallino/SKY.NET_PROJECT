using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyNet
{
    internal class Search
    {

        // Heuristic function to calculate Manhattan distance between two nodes
        static int heuristic(Node a, Node b)
        {
            //HEURISTICA DE MANHATTAN: Distancia entre posiciones de X y de Y entre el nodo actual y fin
            return Math.Abs(a.NodeLocation.LocationX - b.NodeLocation.LocationX) + Math.Abs(a.NodeLocation.LocationY - b.NodeLocation.LocationY);
        }

        // Function to check if a node is valid for traversal

        //esta la usan las walking units, para optimo tomando en cuenta riesgos
        static bool isValidandDangerless(Node n, Node[,] grid)
        {
            // Check if the node is within the boundaries and not dangerous
            if (n.NodeLocation.LocationX >= 0 && n.NodeLocation.LocationX < grid.GetLength(0) && n.NodeLocation.LocationY >= 0 && n.NodeLocation.LocationY < grid.GetLength(1))
            {
                if (grid[n.NodeLocation.LocationX, n.NodeLocation.LocationY].IsDangerous== false)
                {
                    return true;
                }
            }
            return false;
        }

        static bool isValidandWalkable(Node n, Node[,] grid)
        {
            // Check if the node is within the boundaries and not an obstacle
            if (n.NodeLocation. LocationX >= 0 && n.NodeLocation.LocationX < grid.GetLength(0) && n.NodeLocation.LocationY >= 0 && n.NodeLocation.LocationY < grid.GetLength(1))
            {
                if (grid[n.NodeLocation.LocationX, n.NodeLocation.LocationY].IsObstacle == false)
                {
                    return true;
                }
            }
            return false;
        }


        // A* algorithm implementation

        //ESTA VA A SER PARA LAS UNIDADES QUE CAMINEN. Toma en cuenta riesgos. hay que hacer uno que no tome en cuenta riesgos, lo mismo para las flying units. 
        static void AStar(Node start, Node goal, Node[,] grid)
        {
            List<Node> openSet = new List<Node>();  // Nodes to be evaluated
            HashSet<Node> closedSet = new HashSet<Node>();  // Nodes already evaluated
            openSet.Add(start);  // Start with the initial node

            while (openSet.Count > 0)
            {
                Node currentNode = openSet[0];

                // Find the node in openSet with the lowest F cost
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].F < currentNode.F)
                    {
                        currentNode = openSet[i];
                    }
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                // Check if the goal is reached
                if (currentNode.NodeLocation.LocationX == goal.NodeLocation.LocationX && currentNode.NodeLocation.LocationX == goal.NodeLocation.LocationX)
                {
                    // If we reach the goal, we can use the Parent property to get the shortest path.
                    // Alternatively, we can also use a different approach to get the path.
                    Console.WriteLine("Path found.");
                    return;
                }

                // Explore neighboring nodes
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i == 0 && j == 0)
                        {
                            continue;  // Skip the current node
                        }

                        
                       if (currentNode.NodeLocation.LocationX+i >= 0 && currentNode.NodeLocation.LocationX + i < grid.GetLength(0) && currentNode.NodeLocation.LocationY +j >= 0 && currentNode.NodeLocation.LocationY +j < grid.GetLength(1))
                        {
                            Node neighbour = grid[currentNode.NodeLocation.LocationX + i, currentNode.NodeLocation.LocationY + j];
                            if (isValidandWalkable(neighbour, grid))
                            {
                                if (!closedSet.Contains(neighbour))
                                {
                                    // Update the costs and parent if the neighbor is not in the closed set
                                    neighbour.G = currentNode.G + 1;
                                    neighbour.H = heuristic(neighbour, goal);
                                    neighbour.F = neighbour.G + neighbour.H;
                                    neighbour.Parent = currentNode;

                                    // Add the neighbor to the open set if it's not already there
                                    if (!openSet.Contains(neighbour))
                                    {
                                        openSet.Add(neighbour);
                                    }
                                }
                            }
                        }
                        else
                        {
                            continue; //skips the neighbour node if its out of bounds
                        }
                       
                    }
                }
            }
        }
    }
}
