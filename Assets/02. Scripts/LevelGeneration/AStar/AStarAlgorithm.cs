using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.LevelGeneration
{
    public abstract class AStarAlgorithm
    {
        protected List<AStarNode> openNodes;

        private AStarNode[] ReconstructPath(Dictionary<AStarNode, AStarNode> cameFrom, AStarNode current)
        {
            List<AStarNode> path = new List<AStarNode>();
            path.Add(current);
            while(cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                path.Insert(0, current);
            }
            return path.ToArray();
        }

        public AStarNode[] Compute(AStarNode start, AStarNode goal)
        {
            openNodes = new List<AStarNode>();
            openNodes.Add(start);
            Dictionary<AStarNode, AStarNode> cameFrom = new Dictionary<AStarNode, AStarNode>();
            Dictionary<AStarNode, float> gScore = new Dictionary<AStarNode, float>();
            Dictionary<AStarNode, float> fScore = new Dictionary<AStarNode, float>();
            gScore[start] = 0f;
            fScore[start] = Heuristic(start, goal);

            int cicles = 0;
            while(openNodes.Count > 0 && cicles < 5000)
            {
                AStarNode current = GetLowestFNode(fScore, openNodes);
                if (current == goal)
                {
                    return ReconstructPath(cameFrom, current);
                }
                openNodes.Remove(current);
                foreach(var neighbour in current.Neighbours)
                {
                    float tentativeGScore = GetScore(gScore, current) + D(current, neighbour);
                    if (tentativeGScore < GetScore(gScore, neighbour))
                    {
                        cameFrom[neighbour] = current;
                        gScore[neighbour] = tentativeGScore;
                        fScore[neighbour] = tentativeGScore + Heuristic(neighbour, goal);
                        if (!openNodes.Contains(neighbour))
                        {
                            openNodes.Add(neighbour);
                        }
                    }
                }
                cicles++;
            }
            return null;
        }

        private float GetScore(Dictionary<AStarNode, float> scores, AStarNode node)
        {
            if (scores.ContainsKey(node))
            {
                return scores[node];
            } else
            {
                return float.PositiveInfinity;
            }
        }

        private AStarNode GetLowestFNode(Dictionary<AStarNode, float> fscore, List<AStarNode> openList)
        {
            float lowestF = 0f;
            AStarNode lowestNode = null;
            foreach(var node in openList)
            {
                if (fscore.ContainsKey(node)) {
                    if (lowestNode == null || fscore[node] < lowestF)
                    {
                        lowestF = fscore[node];
                        lowestNode = node;
                    }
                }
            }
            return lowestNode;
        }

        protected abstract float Heuristic(AStarNode current, AStarNode goal);
        protected abstract float D(AStarNode current, AStarNode neighbour);
    }
}