using System.Collections.Generic;
using UnityEngine;
using Laresistance.LevelGeneration;
using NUnit.Framework;

namespace Laresistance.Tests
{
    public class AStar_Tests : MonoBehaviour
    {
        [Test]
        public void When_ComputingAStarAlgorithm()
        {
            List<AStarNode> nodes = new List<AStarNode>();
            XYPair gridSize = new XYPair() { x = 3, y = 3 };
            for (int i = 0; i < gridSize.y; ++i)
            {
                for (int j = 0; j < gridSize.x; ++j)
                {
                    nodes.Add(new AStarNode(j + i*gridSize.x, gridSize, 1f));
                }
            }
            AStarGrid grid = new AStarGrid(gridSize, nodes);
            grid.SetNeighbours();
            Assert.AreEqual(3, nodes[0].Neighbours.Length);
            Assert.AreEqual(5, nodes[1].Neighbours.Length);
            Assert.AreEqual(3, nodes[2].Neighbours.Length);
            Assert.AreEqual(5, nodes[3].Neighbours.Length);
            Assert.AreEqual(8, nodes[4].Neighbours.Length);
            Assert.AreEqual(5, nodes[5].Neighbours.Length);
            Assert.AreEqual(3, nodes[6].Neighbours.Length);
            Assert.AreEqual(5, nodes[7].Neighbours.Length);
            Assert.AreEqual(3, nodes[8].Neighbours.Length);

            CustomAStar astar = new CustomAStar();
            AStarNode[] path = astar.Compute(nodes[0], nodes[8]);
            Assert.IsNotNull(path);
            Assert.AreEqual(3, path.Length);
        }
    }
}