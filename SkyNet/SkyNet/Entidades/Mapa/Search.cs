namespace SkyNet.Entidades.Mapa
{

    //REFERENCIAS DE NOTACION
    //F = Total estimated cost (G + H)
    //G = Cost from the start node to the current node
    //H = Heuristic estimate from the current node to the goal node
    //Parent = Reference to the previous node in the path

    public class AStarAlgorithm
    {

        //SEARCH TYPES:
        //Constants for each search type. 
        private const int WalkingSafetyCode = 2;
        private const int WalkingOptimalCode = 1;
        private const int NonWalkingSafetyCode = 4;
        private const int NonWalkingOptimalCode = 3;

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


        // Heuristic function (Manhattan distance) to estimate the cost from node 'a' to node 'b'
        static int Heuristic(Node a, Node b)
        {
            return Math.Abs(a.NodeLocation.LocationX - b.NodeLocation.LocationX) + Math.Abs(a.NodeLocation.LocationY - b.NodeLocation.LocationY);
        }


        //Calls the pathfinding selector, depending on search type. 
        private void UseSelectedPathfindingType(Node currentNode, Node goal, int searchCode, List<Node> openSet, HashSet<Node> closedSet, Node neighbour, Node[,] grid)
        {
            bool isValidNode;


            //Using a switch statement optimises speed and makes the code easier to read. Scalability is not an issue. 
            switch (searchCode)
            {
                case 1:
                    isValidNode = IsValidAndOptimalW(neighbour, grid);
                    break;
                case 2:
                    isValidNode = IsValidAndSafeW(neighbour, grid);
                    break;
                case 3:
                    isValidNode = IsValidAndOptimalF(neighbour, grid);
                    break;
                case 4:
                    isValidNode = IsValidAndSafeF(neighbour, grid);
                    break;
                default:
                    throw new ArgumentException("Invalid SearchCode"); //Control error, for edge cases. Nothing should get to it, though. 
            }

            if (isValidNode && !closedSet.Contains(neighbour))
            {
                int newG = currentNode.G + 1;
                if (!openSet.Contains(neighbour) || newG < neighbour.G)
                {
                    neighbour.G = newG;
                    neighbour.H = Heuristic(neighbour, goal);
                    neighbour.F = neighbour.G + neighbour.H;
                    neighbour.Parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }


        // Find the path using the A* algorithm
        public List<Node> FindPath(Node start, Node goal, Node[,] grid, bool safety, bool isWalkingUnit)
        {
            int searchCode = GetSearchCode(safety, isWalkingUnit);

            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(start);

            while (openSet.Count > 0)
            {
                Node currentNode = GetLowestFCostNode(openSet);

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (IsGoalReached(currentNode, goal))
                {
                    return ConstructPath(currentNode);
                }

                ExploreNeighbourNodes(currentNode, goal, openSet, closedSet, grid, searchCode);
            }

            return new List<Node>();
        }

        // Explore neighboring nodes and update their costs
        private void ExploreNeighbourNodes(Node currentNode, Node goal, List<Node> openSet, HashSet<Node> closedSet, Node[,] grid, int searchCode)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;

                    int neighborX = currentNode.NodeLocation.LocationX + i;
                    int neighborY = currentNode.NodeLocation.LocationY + j;

                    ExploreNeighbourNode(currentNode, goal, openSet, closedSet, grid, searchCode, neighborX, neighborY);
                }
            }
        }

        private void ExploreNeighbourNode(Node currentNode, Node goal, List<Node> openSet, HashSet<Node> closedSet, Node[,] grid, int searchCode, int neighborX, int neighborY)
        {
            if (IsWithinBounds(neighborX, neighborY, grid))
            {
                Node neighbour = grid[neighborX, neighborY];
                UseSelectedPathfindingType(currentNode, goal, searchCode, openSet, closedSet, neighbour, grid);
            }
        }

        private bool IsWithinBounds(int x, int y, Node[,] grid)
        {
            return x >= 0 && x < grid.GetLength(0) && y >= 0 && y < grid.GetLength(1);
        }

        // Pick the corresponding search code depending on safety and if the unit is terrestrial. 
        private int GetSearchCode(bool safety, bool isWalkingUnit)
        {
            if (safety)
            {
                return isWalkingUnit ? WalkingSafetyCode : NonWalkingSafetyCode;
            }
            else
            {
                return isWalkingUnit ? WalkingOptimalCode : NonWalkingOptimalCode;
            }
        }

        private Node GetLowestFCostNode(List<Node> openSet)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].F < currentNode.F)
                {
                    currentNode = openSet[i];
                }
            }
            return currentNode;
        }

        private bool IsGoalReached(Node currentNode, Node goal)
        {
            return currentNode.NodeLocation.LocationX == goal.NodeLocation.LocationX &&
                   currentNode.NodeLocation.LocationY == goal.NodeLocation.LocationY;
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
