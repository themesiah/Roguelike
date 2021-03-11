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

        private MapData mapData = null;
        private RoomData roomData;
        private List<Rect> nodesWindows;
        private int seed = -1;
        private int room = 0;

        private XYPair mapSize = new XYPair() { x = 4, y = 4 };

        [MenuItem("Laresistance/Room Generation Visualizer")]
        public static void ShowEditor()
        {
            RoomGenerationVisualizer editor = EditorWindow.GetWindow<RoomGenerationVisualizer>();
            editor.Init();
        }

        private void CreateMap()
        {
            if (seed != -1)
            {
                Random.InitState(seed);
            }
            mapData = new MapData(mapSize);
            mapData.GenerateMontecarloMinimalPath();
            mapData.GenerateMontecarloExtraPaths();
            mapData.GenerateInteractables();
            mapData.GenerateMovementTestRooms();
            mapData.GenerateRoomEnemies();
            mapData.GenerateRoomsGrids();
            roomData = GetRoomData();
        }

        private RoomData GetRoomData()
        {
            if (room == -2)
            {
                return mapData.FirstRoom.GetLinks()[0].linkedRoom;
            } else
            {
                return mapData.GetAllRooms()[room];
            }
        }

        private void NextRoom()
        {
            room++;
            if (room >= mapData.GetAllRooms().Length)
            {
                room = 0;
            } else if (room == -1)
            {
                room = 0;
            }
            roomData = GetRoomData();
        }

        private void PreviousRoom()
        {
            room--;
            if (room <= -1)
            {
                room = mapData.GetAllRooms().Length - 1;
            }
            roomData = GetRoomData();
        }

        private void CreateRoom()
        {
            if (mapData == null)
            {
                CreateMap();
                roomData = GetRoomData();
            }
            else
            {
                //if (seed != -1)
                //{
                //    Random.InitState(seed);
                //}
                //roomData = GetRoomData();
                //roomData.GenerateRoom();
            }
        }

        public void Init()
        {
            CreateMap();
            InitRoom();
        }

        private void InitRoom()
        {
            nodesWindows = new List<Rect>();
            AStarNode[] roomNodes = roomData.Grid.Nodes;

            foreach (AStarNode nodeData in roomNodes)
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
            //bool dirty = false;

            seed = EditorGUILayout.IntField("Seed", seed);

            if (GUILayout.Button("New Map"))
            {
                Init();
                //dirty = true;
            }

            //if (GUILayout.Button("New Room") || dirty == true)
            //{
            //    Init();
            //}

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Previous Room"))
            {
                PreviousRoom();
                InitRoom();
            }
            GUILayout.Label("ROOM " + (room + 1).ToString());
            if (GUILayout.Button("Next Room"))
            {
                NextRoom();
                InitRoom();
            }
            GUILayout.EndHorizontal();

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
                        GUI.color = Color.green;
                    }
                }
                foreach(var interactable in roomData.GetInteractables())
                {
                    int gridIndexPosition = MapGenerationUtils.CoordinatesToIndex(interactable.gridPosition, roomData.RoomSize.x);
                    if (gridIndexPosition == i)
                    {
                        GUI.color = Color.blue;
                    }
                }
                foreach(var enemy in roomData.GetRoomEnemies())
                {
                    int gridIndexPosition = MapGenerationUtils.CoordinatesToIndex(enemy.gridPosition, roomData.RoomSize.x);
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
            Vector2 endPos = GetPosFromRoom(connectedNodeData, mapSize) + NODE_WINDOW_SIZE/ 2f;
            Handles.DrawLine(startPos, endPos);
        }
    }
}