using UnityEngine;
using System.Collections.Generic;

namespace Laresistance.LevelGeneration
{
    public class MapGenerationUtils
    {
        public static int CoordinatesToIndex(XYPair coord, int width)
        {
            return (coord.y * width) + coord.x;
        }

        public static XYPair IndexToCoordinates(int index, int width)
        {
            return new XYPair() { x = index % width, y = Mathf.FloorToInt(index / width) };
        }

        public static List<XYPair> NeighboursFromIndex(int index, XYPair gridSize)
        {
            return NeighboursFromCoordinates(IndexToCoordinates(index, gridSize.x), gridSize);
        }

        public static List<XYPair> NeighboursFromCoordinates(XYPair coord, XYPair gridSize)
        {
            List<XYPair> result = new List<XYPair>();

            if (coord.x > 0)
            {
                result.Add(new XYPair() { x = coord.x - 1, y = coord.y });
            }
            if (coord.x < gridSize.x-1)
            {
                result.Add(new XYPair() { x = coord.x + 1, y = coord.y });
            }
            if (coord.y > 0)
            {
                result.Add(new XYPair() { x = coord.x, y = coord.y - 1 });
            }
            if (coord.y < gridSize.y-1)
            {
                result.Add(new XYPair() { x = coord.x, y = coord.y + 1 });
            }

            return result;
        }
    }
}