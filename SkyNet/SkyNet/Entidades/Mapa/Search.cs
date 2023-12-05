namespace SkyNet.Entidades.Mapa
{

    //REFERENCIAS DE NOTACION
    //F = Total estimated cost (G + H)
    //G = Cost from the start node to the current node
    //H = Heuristic estimate from the current node to the goal node
    //Parent = Reference to the previous node in the path

    public class AStarAlgorithm
    {
        // Heuristic function to estimate the cost from node 'a' to node 'b'
        static int Heuristic(Node a, Node b)
        {
            return Math.Abs(a.NodeLocation.LocationX - b.NodeLocation.LocationX) + Math.Abs(a.NodeLocation.LocationY - b.NodeLocation.LocationY);
        }

        // Check if a node is valid, and not water, for walking units, no danger consideration (optimal) (Code: 1)
        static bool IsValidAndOptimalW(Node n, Node[,] grid)
        {
            return n.NodeLocation.LocationX >= 0 && n.NodeLocation.LocationX < grid.GetLength(0) &&
                   n.NodeLocation.LocationY >= 0 && n.NodeLocation.LocationY < grid.GetLength(1) &&
                   !(grid[n.NodeLocation.LocationX, n.NodeLocation.LocationY].TerrainType == 2);
        }

        // Check if a node is safe, and not water, for walking units (danger-free) (Code: 2)
        static bool IsValidAndSafeW(Node n, Node[,] grid)
        {
            return n.NodeLocation.LocationX >= 0 && n.NodeLocation.LocationX < grid.GetLength(0) &&
                   n.NodeLocation.LocationY >= 0 && n.NodeLocation.LocationY < grid.GetLength(1) &&
                   !grid[n.NodeLocation.LocationX, n.NodeLocation.LocationY].IsDangerous && !(grid[n.NodeLocation.LocationX, n.NodeLocation.LocationY].TerrainType == 2);
        }

        // Check if a node is valid for Flying  units, no danger consideration (optimal) (Code: 3)
        static bool IsValidAndOptimalF(Node n, Node[,] grid)
        {
            return n.NodeLocation.LocationX >= 0 && n.NodeLocation.LocationX < grid.GetLength(0) &&
                   n.NodeLocation.LocationY >= 0 && n.NodeLocation.LocationY < grid.GetLength(1);
        }

        // Check if a node is safe for flying units (danger-free) (Code: 4)
        static bool IsValidAndSafeF(Node n, Node[,] grid)
        {
            return n.NodeLocation.LocationX >= 0 && n.NodeLocation.LocationX < grid.GetLength(0) &&
                   n.NodeLocation.LocationY >= 0 && n.NodeLocation.LocationY < grid.GetLength(1) &&
                   !grid[n.NodeLocation.LocationX, n.NodeLocation.LocationY].IsDangerous;
        }


        //Calls the pathfinding selector, depending on the search type. 
        private void UseSelectedPathfindingType(Node currentNode, Node goal, int SearchCode, List<Node> openSet, HashSet<Node> closedSet, Node neighbour, Node[,] grid)
        {
            if (SearchCode == 1)
            {
                if (IsValidAndOptimalW(neighbour, grid) && !closedSet.Contains(neighbour))
                {
                    int newG = currentNode.G + 1;
                    if (!openSet.Contains(neighbour) || newG < neighbour.G)
                    {
                        neighbour.G = newG;
                        neighbour.H = Heuristic(neighbour, goal);
                        neighbour.F = neighbour.G + neighbour.H;
                        neighbour.Parent = currentNode;

                        if (!openSet.Contains(neighbour)) openSet.Add(neighbour);
                    }
                }
            }
            else if (SearchCode == 2)
            {
                if (IsValidAndSafeW(neighbour, grid) && !closedSet.Contains(neighbour))
                {
                    int newG = currentNode.G + 1;
                    if (!openSet.Contains(neighbour) || newG < neighbour.G)
                    {
                        neighbour.G = newG;
                        neighbour.H = Heuristic(neighbour, goal);
                        neighbour.F = neighbour.G + neighbour.H;
                        neighbour.Parent = currentNode;

                        if (!openSet.Contains(neighbour)) openSet.Add(neighbour);
                    }
                }
            }
            else if (SearchCode == 3)
            {
                if (IsValidAndOptimalF(neighbour, grid) && !closedSet.Contains(neighbour))
                {
                    int newG = currentNode.G + 1;
                    if (!openSet.Contains(neighbour) || newG < neighbour.G)
                    {
                        neighbour.G = newG;
                        neighbour.H = Heuristic(neighbour, goal);
                        neighbour.F = neighbour.G + neighbour.H;
                        neighbour.Parent = currentNode;

                        if (!openSet.Contains(neighbour)) openSet.Add(neighbour);
                    }
                }
            }
            else if (SearchCode == 4)
            {
                if (IsValidAndSafeF(neighbour, grid) && !closedSet.Contains(neighbour))
                {
                    int newG = currentNode.G + 1;
                    if (!openSet.Contains(neighbour) || newG < neighbour.G)
                    {
                        neighbour.G = newG;
                        neighbour.H = Heuristic(neighbour, goal);
                        neighbour.F = neighbour.G + neighbour.H;
                        neighbour.Parent = currentNode;

                        if (!openSet.Contains(neighbour)) openSet.Add(neighbour);
                    }
                }
            }

        }

        // Explore neighboring nodes and update their costs
        private void ExploreNeighbourNodes(Node currentNode, Node goal, List<Node> openSet, HashSet<Node> closedSet, Node[,] grid, int SearchCode)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;

                    int neighborX = currentNode.NodeLocation.LocationX + i;
                    int neighborY = currentNode.NodeLocation.LocationY + j;

                    if (neighborX >= 0 && neighborX < grid.GetLength(0) && neighborY >= 0 && neighborY < grid.GetLength(1))
                    {
                        Node neighbour = grid[neighborX, neighborY];
                        UseSelectedPathfindingType(currentNode, goal, SearchCode, openSet, closedSet, neighbour, grid);
                    }
                }
            }
        }


        // Find the path using the A* algorithm
        public List<Node> FindPath(Node start, Node goal, Node[,] grid, bool safety, bool isWalkingUnit)
        {

            int SearchCode;
            //Gets the type of pathfinding to utilise
            if (safety)
            {
                if (isWalkingUnit) SearchCode = 2;
                else SearchCode = 4;
            }
            else
            {
                if (isWalkingUnit) SearchCode = 1;
                else SearchCode = 3;
            }

            //Starts pathfinding

            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(start);

            while (openSet.Count > 0) //While there are nodes to be evaluated,
            {
                Node currentNode = openSet[0]; //The current node will be equal to the first node from the open set. 

                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].F < currentNode.F) currentNode = openSet[i]; //Evaluates the lowest F cost from the set, and sets it as the current node. This is a movement.
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode.NodeLocation.LocationX == goal.NodeLocation.LocationX && //Evaluates if the path was found. 
                    currentNode.NodeLocation.LocationY == goal.NodeLocation.LocationY)
                {
                    // If the goal is reached, construct the path
                    return ConstructPath(currentNode);
                }

                ExploreNeighbourNodes(currentNode, goal, openSet, closedSet, grid, SearchCode);
            }

            // If no path is found
            return new List<Node>();
        }

        // Construct the path from the goal node to the start node
        static List<Node> ConstructPath(Node goalNode)
        {
            List<Node> path = new List<Node>();
            Node current = goalNode;

            while (current != null)
            {
                path.Insert(0, current); // Insert at the beginning to maintain the correct order
                current = current.Parent;
            }

            return path;
        }
    }
}
