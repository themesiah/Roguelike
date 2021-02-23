using UnityEngine;

namespace Laresistance.LevelGeneration
{
    public class RoomGenerationAStar : AStarAlgorithm
    {
        protected override float D(AStarNode current, AStarNode neighbour)
        {
            float distance = Vector2.Distance(new Vector2(current.Coordinates.x, current.Coordinates.y), new Vector2(neighbour.Coordinates.x, neighbour.Coordinates.y));
            return distance * Mathf.Max(current.TravelCost, neighbour.TravelCost);
        }

        protected override float Heuristic(AStarNode current, AStarNode goal)
        {
            int x = System.Math.Abs(current.Coordinates.x - goal.Coordinates.x);
            int y = System.Math.Abs(current.Coordinates.y - goal.Coordinates.y) * 2;
            return x + y;
        }
    }
}