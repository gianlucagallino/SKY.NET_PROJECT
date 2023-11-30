namespace SkyNet.Entidades.Mapa
{

    //TODO: Testear que todo ande, primero que nada.  Abstraer patrones repetitivos a funciones y reducir valores hardcodeados. 
    //Preferiblemente tambien, refuncionar la misma funcion para que se adapte a los 4 patrones de busqueda A*
    //(terrestre con optimizacion de peligro, terrestre sin importar peligro, aereo con optimizacion de peligro, aereo sin importar peligro)
    //Diria de incluir el patron manhattan, pero seria solo util en caso de unidades aereas, por lo que lo veo innecesario. 


    //En otras palabras, hay que refaccionar el moveTo, otra vez (para variar)

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
        static bool IsValidAndSafelW(Node n, Node[,] grid)
        {
            return n.NodeLocation.LocationX >= 0 && n.NodeLocation.LocationX < grid.GetLength(0) &&
                   n.NodeLocation.LocationY >= 0 && n.NodeLocation.LocationY < grid.GetLength(1) &&
                   !grid[n.NodeLocation.LocationX, n.NodeLocation.LocationY].IsObstacle && !(grid[n.NodeLocation.LocationX, n.NodeLocation.LocationY].TerrainType == 2);
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
                   !grid[n.NodeLocation.LocationX, n.NodeLocation.LocationY].IsObstacle;
        }

        // Explore neighboring nodes and update their costs+
        //REFERENCIAS DE NOTACION
        //F = Total estimated cost (G + H)
        //G = Cost from the start node to the current node
        //H = Heuristic estimate from the current node to the goal node
        //Parent = Reference to the previous node in the path
        static void ExploreNeighbourNodes(Node currentNode, Node goal, List<Node> openSet, HashSet<Node> closedSet, Node[,] grid)
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
                }
            }
        }

        // Find the path using the A* algorithm
        public List<Node> FindPath(Node start, Node goal, Node[,] grid)
        {
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

                ExploreNeighbourNodes(currentNode, goal, openSet, closedSet, grid);
            }

            // If no path is found
            return null;
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
