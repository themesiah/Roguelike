using System.Collections.Generic;

namespace Laresistance.LevelGeneration
{
    public class AStarGrid
    {
        public XYPair GridSize { get; private set; }
        private List<AStarNode> nodes;

        public AStarNode[] Nodes => nodes.ToArray();

        public AStarGrid(XYPair gridSize, List<AStarNode> nodes)
        {
            GridSize = gridSize;
            this.nodes = nodes;
        }

        public void SetNeighbours()
        {
            foreach(var node in nodes)
            {
                XYPair coordinates = node.Coordinates;
                List<AStarNode> neighbours = new List<AStarNode>();
                for (int i = -1; i <= 1; ++i)
                {
                    for (int j = -1; j <= 1; ++j)
                    {
                        if (i == 0 && j == 0)
                            continue; // Same node
                        XYPair potentialNeighbourCoordinates = new XYPair() { x = coordinates.x + i, y = coordinates.y + j };
                        if (potentialNeighbourCoordinates.x >= 0 && potentialNeighbourCoordinates.x < GridSize.x && potentialNeighbourCoordinates.y >= 0 && potentialNeighbourCoordinates.y < GridSize.y)
                        {
                            AStarNode neighbour = nodes[MapGenerationUtils.CoordinatesToIndex(potentialNeighbourCoordinates, GridSize.x)];
                            neighbours.Add(neighbour);
                        }
                    }
                }
                node.SetNeighbours(neighbours);
            }
        }

        public void SetNeighboursNoDiagonals()
        {
            foreach(var node in nodes)
            {
                XYPair coordinates = node.Coordinates;
                List<AStarNode> neighbours = new List<AStarNode>();
                for (int i = -1; i <= 1; ++i)
                {
                    for (int j = -1; j <= 1; ++j)
                    {
                        if (i == j)
                            continue; // Diagonals
                        XYPair potentialNeighbourCoordinates = new XYPair() { x = coordinates.x + i, y = coordinates.y + j };
                        if (potentialNeighbourCoordinates.x >= 0 && potentialNeighbourCoordinates.x < GridSize.x && potentialNeighbourCoordinates.y >= 0 && potentialNeighbourCoordinates.y < GridSize.y)
                        {
                            AStarNode neighbour = nodes[MapGenerationUtils.CoordinatesToIndex(potentialNeighbourCoordinates, GridSize.x)];
                            neighbours.Add(neighbour);
                        }
                    }
                }
                node.SetNeighbours(neighbours);
            }
        }
    }
}