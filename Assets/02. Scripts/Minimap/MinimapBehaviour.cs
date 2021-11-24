using Laresistance.Behaviours;
using Laresistance.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Minimap
{
    public class MinimapBehaviour : MonoBehaviour
    {
        public struct RoomToIndex
        {
            public RoomChangeBehaviour room;
            public int index;
        }

        [SerializeField]
        private RuntimeMinimapSingle minimapReference = default; 

        [SerializeField] [Tooltip("List of the 9 minirooms (circles) on the minimap, in order")]
        private Miniroom[] miniRooms = default;

        [SerializeField] [Tooltip("List of the 12 connections on the minimap, in order")]
        private Renderer[] connections = default;

        [SerializeField]
        private Transform playerIndicator = default;

        private Dictionary<RoomChangeBehaviour, int> roomToIndex;
        private int playerCurrentRoom = 0;
        private int pilgrimRoom = -1;

        private void OnEnable()
        {
            minimapReference?.Set(this);   
        }

        private void OnDisable()
        {
            minimapReference?.Set(null);
        }

        private void Awake()
        {
            roomToIndex = new Dictionary<RoomChangeBehaviour, int>();
        }

        public void AddRoom(RoomChangeBehaviour room, int index)
        {
            roomToIndex.Add(room, index);
        }

        public void SetPilgrim(int index)
        {
            pilgrimRoom = index;
            miniRooms[index].SetPilgrim();
        }

        public int RoomIndexesToConnectionIndex(int index1, int index2)
        {
            int minIndex = System.Math.Min(index1, index2);
            int maxIndex = System.Math.Max(index1, index2);

            if (maxIndex - minIndex == 1)
            {
                if (maxIndex <= 2)
                    return minIndex;
                if (maxIndex <= 5)
                    return minIndex + 2;
                else
                    return minIndex + 4;
            } else
            {
                if (minIndex <= 2)
                    return minIndex + 2;
                else
                    return minIndex + 4;
            }
        }

        public void ShowConnection(RoomChangeBehaviour room1, RoomChangeBehaviour room2)
        {
            int index1 = roomToIndex[room1];
            int index2 = roomToIndex[room2];
            ShowConnection(index1, index2);
        }

        public void ShowConnection(int index1, int index2)
        {
            int connectionIndex = RoomIndexesToConnectionIndex(index1, index2);
            connections[connectionIndex].enabled = true;
        }

        public void MovePlayer(RoomChangeBehaviour room)
        {
            if (roomToIndex.ContainsKey(room))
            {
                int index = roomToIndex[room];
                int lastRoom = playerCurrentRoom;
                ShowConnection(lastRoom, index);
                miniRooms[index].Show();
                playerIndicator.SetParent(miniRooms[index].transform);
                playerIndicator.localPosition = Vector3.zero;
                playerCurrentRoom = index;
            }
        }
    }
}