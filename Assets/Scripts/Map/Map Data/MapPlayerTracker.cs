﻿using System;
using System.Linq;
using UnityEngine;

namespace Map
{
    public class MapPlayerTracker : MonoBehaviour
    {
        public bool lockAfterSelecting = false;
       // public float enterNodeDelay = 1f;
        public MapManager mapManager;
        public MapView view;

        public static MapPlayerTracker Instance;

        public bool Locked { get; set; }

        private void Awake()
        {
            Instance = this;
        }

        public void SelectNode(NodeMap mapNode)
        {
            if (Locked) return;

            // Debug.Log("Selected node: " + mapNode.Node.point);

            if (mapManager.CurrentMap.path.Count == 0)
            {
                // player has not selected the node yet, he can select any of the nodes with y = 0
                if (mapNode.NodeData.point.y == 0)
                    SendPlayerToNode(mapNode);
                else
                    PlayWarningThatNodeCannotBeAccessed();
            }
            else
            {
                var currentPoint = mapManager.CurrentMap.path[mapManager.CurrentMap.path.Count - 1];
                var currentNode = mapManager.CurrentMap.GetNode(currentPoint);

                if (currentNode != null && currentNode.outgoing.Any(point => point.Equals(mapNode.NodeData.point)))
                    SendPlayerToNode(mapNode);
                else
                    PlayWarningThatNodeCannotBeAccessed();
            }
        }

        private void SendPlayerToNode(NodeMap mapNode)
        {
            Locked = lockAfterSelecting;
            mapManager.CurrentMap.path.Add(mapNode.NodeData.point);
            mapManager.SaveMap();
  
            //    view.SetLineColors();
            EnterNode(mapNode);
        }

        private void Start()
        {
            view.SetAttainableNodes();
        }

        private static void EnterNode(NodeMap mapNode)
        {
            // we have access to blueprint name here as well
            Debug.Log("Entering node of type: " + mapNode.NodeData.NodeTypeEnum);
            // load appropriate scene with context based on nodeType:
            // or show appropriate GUI over the map: 
            // if you choose to show GUI in some of these cases, do not forget to set "Locked" in MapPlayerTracker back to false


            Factory.GameFactory.Instance.EventPointFactoryHandler.GetEventPoint(mapNode.NodeData.NodeTypeEnum).ActivatePoint();

            //switch (mapNode.NodeData.NodeTypeEnum)
            //{
           
            //    case NodeType.Basic_Enemy:
            //        break;
            //    case NodeType.Elite_Enemy:
            //        break;
            //    case NodeType.Chest:
            //        break;
            //    case NodeType.QuestionMark:
            //        break;
            //    case NodeType.Rest_Area:
            //        break;
            //    case NodeType.Dojo:
            //        break;
            //    case NodeType.Boss_Enemy:
            //        break;
            //    default:
            //        throw new ArgumentOutOfRangeException();
            //}

        }

        private void PlayWarningThatNodeCannotBeAccessed()
        {
            Debug.Log("Selected node cannot be accessed");
        }
    }

}