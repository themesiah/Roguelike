using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Laresistance.LevelGeneration
{
    public class MapGenerationVisualizer : EditorWindow
    {
        private static Vector2 ROOM_WINDOW_SIZE = Vector2.one * 100f;
        private static float COORDINATES_SEPARATION = 200f;
        private static Vector2 ROOM_POSITION_OFFSET = Vector2.up * 100f + Vector2.right * 50f;

        private MapData mapData;
        private List<Rect> roomsWindows;

        private XYPair mapSize = new XYPair() { x = 4, y = 4 };
        private int seed = -1;

        [MenuItem("Laresistance/Map Generation Visualizer")]
        public static void ShowEditor()
        {
            MapGenerationVisualizer editor = EditorWindow.GetWindow<MapGenerationVisualizer>();
            editor.Init();
        }

        private MapData CreateMap()
        {
            if (seed != -1)
            {
                Random.InitState(seed);
            }
            MapData mapData = new MapData(mapSize);
            mapData.GenerateMontecarloMinimalPath();
            mapData.GenerateMontecarloExtraPaths();
            mapData.GenerateInteractables();
            mapData.GenerateMovementTestRooms();
            mapData.GenerateRoomEnemies();
            mapData.GenerateRoomsGrids();
            return mapData;
        }

        public void Init()
        {
            mapData = CreateMap();
            roomsWindows = new List<Rect>();
            RoomData[] mapRooms = mapData.GetAllRooms();

            foreach(RoomData roomData in mapRooms)
            {
                Vector2 pos = GetPosFromRoom(roomData, mapSize);
                roomsWindows.Add(new Rect(pos, ROOM_WINDOW_SIZE));
            }
            mapData.ToJson("mapData.json");
        }

        private XYPair GetCoordinatesFromRoom(RoomData roomData, XYPair mapSize)
        {
            XYPair coord = MapGenerationUtils.IndexToCoordinates(roomData.RoomIndex, mapSize.x);
            coord.y = mapSize.y - coord.y - 1;
            return coord;
        }

        private Vector2 GetPosFromRoom(RoomData roomData, XYPair mapSize)
        {
            XYPair coord = GetCoordinatesFromRoom(roomData, mapSize);
            Vector2 pos = new Vector2(coord.x * COORDINATES_SEPARATION, coord.y * COORDINATES_SEPARATION) + ROOM_POSITION_OFFSET;
            return pos;
        }

        private void OnGUI()
        {
            mapSize.x = EditorGUILayout.IntField("Width", mapSize.x);
            mapSize.y = EditorGUILayout.IntField("Height", mapSize.y);
            seed = EditorGUILayout.IntField("Seed", seed);
            if (GUILayout.Button("Generate"))
            {
                Init();
            }
            var rooms = mapData.GetAllRooms();
            BeginWindows();
            for (int i = 0; i < roomsWindows.Count; ++i)
            {
                if (rooms[i].IsFirstRoom)
                {
                    GUI.color = Color.red;
                }
                else if (rooms[i].IsLastRoom)
                {
                    GUI.color = Color.red;
                }
                else if (rooms[i].IsMinimalPathRoom)
                {
                    GUI.color = Color.yellow;
                }
                else if (rooms[i].HaveMovementTest && false)
                {
                    GUI.color = Color.blue;
                } else if (rooms[i].IsPathEnd) {
                    GUI.color = Color.green;
                } else
                {
                    GUI.color = Color.white;
                }
                roomsWindows[i] = GUI.Window(i, roomsWindows[i], DrawNodeWindow, (i + 1).ToString());
                DrawLink(rooms[i], mapData.GetMapSize());
            }
            EndWindows();
        }

        private void DrawNodeWindow(int id)
        {
            //GUI.DragWindow();
        }

        private void DrawLink(RoomData roomData, XYPair mapSize)
        {
            Vector2 startPos = GetPosFromRoom(roomData, mapSize) + ROOM_WINDOW_SIZE/2f;
            var connections = roomData.GetLinks();
            for (int i = 0; i < connections.Length; ++i)
            {
                Vector2 connectedPos = GetPosFromRoom(connections[i].linkedRoom, mapSize) + ROOM_WINDOW_SIZE/2f;
                if (connections[i].minimalPath)
                {
                    Handles.color = Color.red;
                } else
                {
                    Handles.color = Color.white;
                }
                Handles.DrawLine(startPos, connectedPos);
            }
        }
    }
}