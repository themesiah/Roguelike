using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Laresistance.LevelGeneration
{
    public class RoomGenerationVisualizer : EditorWindow
    {
        

        private static Vector2 NODE_WINDOW_SIZE = Vector2.one * 10f;
        private static float COORDINATES_SEPARATION = 20f;
        private static Vector2 ROOM_POSITION_OFFSET = Vector2.up * 150f + Vector2.right * 50f;
        private static Vector2 NOISE_SCALE_MIN_MAX = RoomData.NOISE_SCALE_MIN_MAX;
        private static Vector2 NOISE_WEIGHT_MIN_MAX = RoomData.NOISE_WEIGHT_MIN_MAX;
        private static Vector2 NOISE_OFFSET_MIN_MAX = RoomData.NOISE_OFFSET_MIN_MAX;

        private MapData mapData = null;
        private RoomData roomData;
        private List<Rect> nodesWindows;

        private XYPair mapSize = new XYPair() { x = 4, y = 4 };
        private float noiseScale = 1f;
        private float noiseWeight = 1f;
        private Vector2 noiseOffset = Vector2.one;

        private Color[] pix;
        private Texture2D noiseTex;

        [MenuItem("Laresistance/Room Generation Visualizer")]
        public static void ShowEditor()
        {
            RoomGenerationVisualizer editor = EditorWindow.GetWindow<RoomGenerationVisualizer>();
            editor.Init();
        }

        private void CreateMap()
        {
            mapData = new MapData(mapSize);
            mapData.GenerateMontecarloMinimalPath();
            mapData.GenerateMontecarloExtraPaths();
            mapData.GenerateInteractables();
            mapData.GenerateMovementTestRooms();
            mapData.GenerateRoomEnemies();
            mapData.GenerateRoomsGrids();
        }

        private RoomData CreateRoom()
        {
            if (mapData == null)
            {
                CreateMap();
            }
            roomData = mapData.FirstRoom.GetLinks()[0].linkedRoom;
            roomData.SetNoiseData(noiseScale, noiseOffset, noiseWeight);
            roomData.GenerateRoom();
            return roomData;
        }

        public void Init()
        {
            roomData = CreateRoom();
            nodesWindows = new List<Rect>();
            AStarNode[] roomNodes = roomData.Grid.Nodes;

            foreach(AStarNode nodeData in roomNodes)
            {
                Vector2 pos = GetPosFromRoom(nodeData, roomData.RoomSize);
                nodesWindows.Add(new Rect(pos, NODE_WINDOW_SIZE));
            }
        }

        private XYPair GetCoordinatesFromRoom(AStarNode nodeData, XYPair mapSize)
        {
            XYPair coord = MapGenerationUtils.IndexToCoordinates(nodeData.Index, mapSize.x);
            coord.y = mapSize.y - coord.y - 1;
            return coord;
        }

        private Vector2 GetPosFromRoom(AStarNode nodeData, XYPair mapSize)
        {
            XYPair coord = GetCoordinatesFromRoom(nodeData, mapSize);
            Vector2 pos = new Vector2(coord.x * COORDINATES_SEPARATION, coord.y * COORDINATES_SEPARATION) + ROOM_POSITION_OFFSET;
            return pos;
        }

        private void OnGUI()
        {
            bool dirty = false;

            if (GUILayout.Button("New Map"))
            {
                CreateMap();
                dirty = true;
            }

            if (GUILayout.Button("New Room") || dirty == true)
            {
                Init();
            }
            var nodes = roomData.Grid.Nodes;
            var paths = roomData.RoomPaths;

            BeginWindows();
            for (int i = 0; i < nodesWindows.Count; ++i)
            {
                GUI.color = Color.white;
                foreach(var link in roomData.GetLinks())
                {
                    int gridIndexPosition = MapGenerationUtils.CoordinatesToIndex(link.gridPosition, roomData.RoomSize.x);
                    if (gridIndexPosition == i)
                    {
                        GUI.color = Color.red;
                    }
                }
                nodesWindows[i] = GUI.Window(i, nodesWindows[i], DrawNodeWindow, (i + 1).ToString());
            }

            for (int j = 0; j < paths.Count; ++j)
            {
                var path = paths[j];
                for (int i = 0; i < path.Length; ++i)
                {
                    if (i < path.Length - 1)
                    {
                        DrawLink(path[i], path[i + 1], roomData.RoomSize);
                    }
                }
            }
            EndWindows();
        }

        private void DrawNodeWindow(int id)
        {
            //GUI.DragWindow();
        }

        private void DrawLink(AStarNode nodeData, AStarNode connectedNodeData, XYPair mapSize)
        {
            Vector2 startPos = GetPosFromRoom(nodeData, mapSize) + NODE_WINDOW_SIZE/2f;
            Vector2 endPos = GetPosFromRoom(connectedNodeData, mapSize) + NODE_WINDOW_SIZE/2f;
            Handles.DrawLine(startPos, endPos);
        }
    }
}