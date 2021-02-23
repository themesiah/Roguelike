using UnityEngine;

namespace Laresistance.LevelGeneration
{
    public class CustomAStar : AStarAlgorithm
    {
        protected override float D(AStarNode current, AStarNode neighbour)
        {
            float distance = Vector2.Distance(new Vector2(current.Coordinates.x, current.Coordinates.y), new Vector2(neighbour.Coordinates.x, neighbour.Coordinates.y));
            return distance * Mathf.Max(current.TravelCost, neighbour.TravelCost);
        }

        protected override float Heuristic(AStarNode current, AStarNode goal)
        {
            return Vector2.Distance(new Vector2(current.Coordinates.x, current.Coordinates.y), new Vector2(goal.Coordinates.x, goal.Coordinates.y));
        }
    }
}