using System.Collections.Generic;

namespace Laresistance.LevelGeneration
{
    public class AStarNode
    {
        private int index;
        private XYPair gridSize;
        private List<AStarNode> neighbours;

        public float f; // Total value (the less the better)
        public float g; // Value from start to this node
        public float h; // Value from this node to the end (following an heuristic)
        public AStarNode Parent;
        public float TravelCost { get; private set; }
        public AStarNode[] Neighbours => neighbours.ToArray();
        public int Index => index;
        
        
        public XYPair Coordinates => MapGenerationUtils.IndexToCoordinates(index, gridSize.x);

        public AStarNode(int index, XYPair gridSize, float travelCost)
        {
            this.index = index;
            this.gridSize = gridSize;
            f = float.PositiveInfinity;
            g = float.PositiveInfinity;
            h = float.PositiveInfinity;
            TravelCost = travelCost;
            neighbours = new List<AStarNode>();
        }

        public void SetNeighbours(List<AStarNode> neighbours)
        {
            this.neighbours = neighbours;
        }
    }
}