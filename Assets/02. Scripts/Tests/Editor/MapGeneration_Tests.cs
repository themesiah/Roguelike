using NUnit.Framework;
using UnityEngine;
using Laresistance.LevelGeneration;

namespace Laresistance.Tests
{
    public class MapGeneration_Tests : MonoBehaviour
    {
        [Test]
        public void When_GeneratingMontecarloMinimalPath1()
        {
            XYPair size = new XYPair() { x = 4, y = 3 };
            MapData mapData = new MapData(size);
            mapData.GenerateMontecarloMinimalPath();
            RoomData[] minimalPath = mapData.GetMinimalPath();
            Assert.IsNotNull(minimalPath);
            Assert.IsTrue(minimalPath.Length > 0);
            Assert.AreEqual(minimalPath[0], mapData.FirstRoom);
            Assert.AreEqual(minimalPath[minimalPath.Length - 1], mapData.LastRoom);
            System.Text.StringBuilder nodesOrder = new System.Text.StringBuilder();
            for (int i = 0; i < minimalPath.Length; ++i)
            {
                nodesOrder.Append(minimalPath[i].RoomIndex.ToString());
                if (i < minimalPath.Length-1)
                {
                    nodesOrder.Append(" -> ");
                }
            }
            Debug.Log(nodesOrder.ToString());
        }

        [Test]
        public void When_GeneratingMontecarloMinimalPath2()
        {
            XYPair size = new XYPair() { x = 6, y = 5 };
            MapData mapData = new MapData(size);
            mapData.GenerateMontecarloMinimalPath();
            RoomData[] minimalPath = mapData.GetMinimalPath();
            Assert.IsNotNull(minimalPath);
            Assert.IsTrue(minimalPath.Length > 0);
            Assert.AreEqual(minimalPath[0], mapData.FirstRoom);
            Assert.AreEqual(minimalPath[minimalPath.Length - 1], mapData.LastRoom);
            System.Text.StringBuilder nodesOrder = new System.Text.StringBuilder();
            for (int i = 0; i < minimalPath.Length; ++i)
            {
                nodesOrder.Append(minimalPath[i].RoomIndex.ToString());
                if (i < minimalPath.Length-1)
                {
                    nodesOrder.Append(" -> ");
                }
            }
            Debug.Log(nodesOrder.ToString());
        }
    }
}