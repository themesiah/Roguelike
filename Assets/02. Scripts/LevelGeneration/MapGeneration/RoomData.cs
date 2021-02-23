using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.LevelGeneration
{
    [System.Serializable]
    public class RoomData
    {
        public struct NoiseData
        {
            public float noiseScale;
            public float noiseWeight;
            public Vector2 noiseOffset;
        }

        private static int MAX_AREA = 225;
        private static float AREA_PER_ENEMY = 36f;
        private static float AREA_PER_INTERACTABLE = 18f;
        private static float AREA_PER_MOVEMENT_TEST = 24f;
        private static float AREA_PER_ROOM_LINK = 18f;

        private static int LEFT_LINK_OFFSET = 1;
        private static int RIGHT_LINK_OFFSET = 1;
        private static int BOTTOM_LINK_OFFSET = 1;
        private static int TOP_LINK_OFFSET = 3;

        public static Vector2 NOISE_SCALE_MIN_MAX = new Vector2() { x = 0.1f, y = 10f };
        public static Vector2 NOISE_WEIGHT_MIN_MAX = new Vector2() { x = 0.1f, y = 10f };
        public static Vector2 NOISE_OFFSET_MIN_MAX = new Vector2() { x = 0f, y = 10f };
        public static int RANDOM_PATH_TRIES = 200;

        [SerializeField]
        private List<RoomLink> roomConnections;
        [SerializeField]
        private MovementTest movementTest;
        [SerializeField]
        private List<RoomInteractable> roomInteractables;
        [SerializeField]
        private List<RoomEnemy> roomEnemies;

        [SerializeField]
        private int roomIndex;
        [SerializeField]
        private bool haveMovementTest;
        [System.NonSerialized]
        private MapData mapData;
        [SerializeField]
        private XYPair roomSize;
        [SerializeField]
        private Vector2 roomPosition;
        public int RoomIndex => roomIndex;
        public bool HaveMovementTest => haveMovementTest;
        public bool IsPathEnd => !IsMinimalPathRoom && roomConnections.Count == 1;
        public bool IsMinimalPathRoom { get; private set; }
        public bool IsFirstRoom { get; private set; }
        public bool IsLastRoom { get; private set; }
        public List<AStarNode[]> RoomPaths { get; private set; }
        public AStarGrid Grid { get; private set; }
        public XYPair RoomSize => roomSize;

        private float noiseScale = 1f; // > 0
        private Vector2 noiseOffset = Vector2.zero; // 0~1
        private float noiseWeight = 1f;

        public RoomData(int roomIndex, MapData mapData)
        {
            this.roomIndex = roomIndex;
            this.mapData = mapData;
            roomConnections = new List<RoomLink>();
            roomInteractables = new List<RoomInteractable>();
            roomEnemies = new List<RoomEnemy>();
        }

        #region RoomInitialization
        public void AddLink(RoomLink roomLink)
        {
            if (!roomConnections.Exists((link) => link.linkedRoom == roomLink.linkedRoom)) // The room we want to link is not linked yet
            {
                roomConnections.Add(roomLink);
            }
        }

        public RoomLink GetLinkFromRoom(RoomData roomData)
        {
            foreach(var link in roomConnections)
            {
                if (link.linkedRoom == roomData)
                {
                    return link;
                }
            }
            return new RoomLink();
        }

        public void RemoveLink(int linkIndex)
        {
            RoomLink link = roomConnections[linkIndex].linkedRoom.GetLinkFromRoom(this);
            if (link.linkedRoom != null)
            {
                roomConnections[linkIndex].linkedRoom.RemoveLink(link);
            }
            roomConnections.RemoveAt(linkIndex);
        }

        public void RemoveLink(RoomLink link)
        {
            roomConnections.Remove(link);
        }

        public RoomLink[] GetLinks()
        {
            return roomConnections.ToArray();
        }

        public void AddInteractable(RoomInteractable roomInteractable)
        {
            roomInteractables.Add(roomInteractable);
        }

        public RoomInteractable[] GetInteractables()
        {
            return roomInteractables.ToArray();
        }

        public void AddEnemy(RoomEnemy roomEnemy)
        {
            roomEnemies.Add(roomEnemy);
        }

        public RoomEnemy[] GetRoomEnemies()
        {
            return roomEnemies.ToArray();
        }

        public void SetAsMinimalPathRoom()
        {
            IsMinimalPathRoom = true;
        }

        public void SetAsFirstRoom()
        {
            IsFirstRoom = true;
        }

        public void SetAsLastRoom()
        {
            IsLastRoom = true;
        }

        public void SetMovementTest(MovementTest movementTest)
        {
            haveMovementTest = true;
            this.movementTest = movementTest;
        }

        public MovementTest GetMovementTest()
        {
            return movementTest;
        }
        #endregion

        #region RoomGeneration
        public void GenerateRoom()
        {
            // Get necessary data
            int numberOfEnemies = roomEnemies.Count;
            bool movementTest = haveMovementTest;
            int numberOfConnections = roomConnections.Count;
            int numberOfInteractables = roomInteractables.Count;
            // Generate size
            roomSize = GetRoomSize(numberOfEnemies, numberOfInteractables, numberOfConnections, movementTest);
            UnityEngine.Assertions.Assert.IsTrue(roomSize.x > 0);
            UnityEngine.Assertions.Assert.IsTrue(roomSize.y > 0);
            UnityEngine.Assertions.Assert.IsTrue(roomSize.x * roomSize.y <= MAX_AREA);
            // Determinate center position. It works as an offset for all that is placed on the room.
            roomPosition = GetRoomPosition();
            UnityEngine.Assertions.Assert.IsTrue(roomPosition.x >= 0f);
            UnityEngine.Assertions.Assert.IsTrue(roomPosition.y >= 0f);
            // Determinate link points on the limits of the room
            SetLinksPositionInGrid();
            // Create at least one minimal path between every link point with each other link point. If the room have a movement test that blocks the path, the minimal path will have to follow other rules. For a minimal path between the bottom of the room and the top, there is some % that there is an elevator involved.
            RoomPaths = new List<AStarNode[]>();
            NoiseData bestNoiseData = new NoiseData();
            for (int z = 0; z < RANDOM_PATH_TRIES; ++z)
            {
                float newNoiseScale = Random.Range(NOISE_SCALE_MIN_MAX.x, NOISE_SCALE_MIN_MAX.y);
                float newNoiseWeight = Random.Range(NOISE_WEIGHT_MIN_MAX.x, NOISE_WEIGHT_MIN_MAX.y);
                Vector2 newNoiseOffset = new Vector2(Random.Range(NOISE_OFFSET_MIN_MAX.x, NOISE_OFFSET_MIN_MAX.y), Random.Range(NOISE_OFFSET_MIN_MAX.x, NOISE_OFFSET_MIN_MAX.y));
                SetNoiseData(newNoiseScale, newNoiseOffset, newNoiseWeight);
                List<AStarNode[]> paths = new List<AStarNode[]>();
                for (int i = 0; i < GetLinks().Length; ++i)
                {
                    for (int j = i + 1; j < GetLinks().Length; ++j)
                    {
                        var nodes = GetScenarioNodes();
                        Grid = new AStarGrid(roomSize, nodes);
                        Grid.SetNeighbours();
                        AStarAlgorithm algorithm = new RoomGenerationAStar();
                        int gridIndexStart = MapGenerationUtils.CoordinatesToIndex(GetLinks()[i].gridPosition, RoomSize.x);
                        int gridIndexEnd = MapGenerationUtils.CoordinatesToIndex(GetLinks()[j].gridPosition, RoomSize.x);
                        var path = algorithm.Compute(nodes[gridIndexStart], nodes[gridIndexEnd]);
                        paths.Add(path);
                    }
                }
                if (PathsLength(paths) > PathsLength(RoomPaths))
                {
                    RoomPaths = paths;
                    bestNoiseData.noiseOffset = noiseOffset;
                    bestNoiseData.noiseScale = noiseScale;
                    bestNoiseData.noiseWeight = noiseWeight;
                }
            }
            // Determinate rewards position
            // Create at least one minimal path between somewhere on the minimal path of the links and the rewards. If there are no minimal paths between links (just one link, end of path room), use the start of the room. As for the links minimal paths, there could be an elevator or a movement test.
            // Determinate nodes IN THE PATH that will contain enemies.
            // Setup enemies
            // Setup decoration

        }

        private XYPair GetRoomSize(int numberOfEnemies, int numberOfInteractables, int numberOfConnections, bool haveMovementTest)
        {
            int totalRoomArea = 0;
            totalRoomArea += (int)(numberOfEnemies * AREA_PER_ENEMY);
            totalRoomArea += (int)(numberOfInteractables * AREA_PER_INTERACTABLE);
            totalRoomArea += (int)(numberOfConnections * AREA_PER_ROOM_LINK);
            if (haveMovementTest)
            {
                totalRoomArea += (int)AREA_PER_MOVEMENT_TEST;
            }
            totalRoomArea = System.Math.Min(totalRoomArea, MAX_AREA);
            float side = Mathf.Sqrt(totalRoomArea);
            int intSide = Mathf.CeilToInt(side);

            return new XYPair() { x = intSide, y = intSide };
        }

        private Vector2 GetRoomPosition()
        {
            XYPair mapSize = mapData.GetMapSize();
            XYPair roomPosInMap = MapGenerationUtils.IndexToCoordinates(RoomIndex, mapSize.x);
            Vector2 centerPosition = new Vector2() { x = roomPosInMap.x * MAX_AREA * 1.5f, y = roomPosInMap.y * MAX_AREA * 1.5f };
            return centerPosition;
        }

        private void SetLinksPositionInGrid()
        {
            for (int i = 0; i < roomConnections.Count; ++i)
            {
                roomConnections[i] = SetRoomLinkPositionInGrid(roomConnections[i]);
            }
        }

        private RoomLink SetRoomLinkPositionInGrid(RoomLink link)
        {
            // Where is the adjacent room? Top, left, right or bottom?
            XYPair roomCoordinates = MapGenerationUtils.IndexToCoordinates(RoomIndex, mapData.GetMapSize().x);
            XYPair linkedRoomCoordinates = MapGenerationUtils.IndexToCoordinates(link.linkedRoomIndex, mapData.GetMapSize().x);
            int relativePosition = link.linkPosition;
            if (roomCoordinates.x == linkedRoomCoordinates.x) // vertical
            {
                int thirdOfWidth = roomSize.x / 3;
                int xposition = thirdOfWidth * relativePosition + thirdOfWidth / 2;
                xposition = roomSize.x / 2 + relativePosition * 2;
                if (roomCoordinates.y > linkedRoomCoordinates.y) // bottom link
                {
                    link.gridPosition = new XYPair() { x = xposition, y = BOTTOM_LINK_OFFSET };
                } else // top link
                {
                    link.gridPosition = new XYPair() { x = xposition, y = roomSize.y - TOP_LINK_OFFSET };
                }
            } else // horizontal
            {
                int thirdOfHeihgt = roomSize.y / 3;
                int yposition = thirdOfHeihgt * relativePosition + thirdOfHeihgt / 2;
                yposition = roomSize.y / 2 + relativePosition * 2;
                if (roomCoordinates.x > linkedRoomCoordinates.x) // left link
                {
                    link.gridPosition = new XYPair() { x = LEFT_LINK_OFFSET, y = yposition };
                } else // right link
                {
                    link.gridPosition = new XYPair() { x = roomSize.x - RIGHT_LINK_OFFSET, y = yposition };
                }
            }
            return link;
        }

        public void SetNoiseData(float scale, Vector2 offset, float weight)
        {
            UnityEngine.Assertions.Assert.IsTrue(scale > 0f);
            UnityEngine.Assertions.Assert.IsTrue(offset.x >= 0f && offset.x <= 10f && offset.y >= 0f && offset.y <= 10f);
            noiseScale = scale;
            noiseOffset = offset;
            noiseWeight = weight;
        }

        public List<AStarNode> GetScenarioNodes()
        {
            List<AStarNode> nodes = new List<AStarNode>();
            for (int i = 0; i < roomSize.y; ++i)
            {
                for (int j = 0; j < roomSize.x; ++j)
                {
                    float xCoord = noiseOffset.x + (j*20) / roomSize.x * noiseScale * 20f;
                    float yCoord = noiseOffset.y + (i*20) / roomSize.y * noiseScale * 20f;
                    float perlin = Mathf.Clamp01(Mathf.PerlinNoise(xCoord, yCoord));
                    if (perlin < 0.5f)
                    {
                        perlin = 0f;
                    } else
                    {
                        perlin = 1f;
                    }
                    perlin *= noiseWeight;
                    nodes.Add(new AStarNode(j + i * roomSize.x, roomSize, 1f + perlin));
                }
            }
            return nodes;
        }

        private int PathsLength(List<AStarNode[]> paths)
        {
            int length = 0;
            foreach(var path in paths)
            {
                length += path.Length;
            }
            return length;
        }
        #endregion
    }
}
