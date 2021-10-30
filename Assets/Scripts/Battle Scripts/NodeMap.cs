

using UI.Map;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Map
{
    public class NodeMap : MonoBehaviour
    {
        [ShowInInspector]
         public Node NodeData { get; private set; }

        [ShowInInspector]
        public NodePointAbstSO BluePrintNode { get; private set; }

        [ShowInInspector]
        public NodeType nodeType => NodeData == null ? NodeType.None : NodeData.NodeTypeEnum;
        [SerializeField] SpriteRenderer _backgroundImg;
        [SerializeField] SpriteRenderer _sr;

        float initialScale;
        private const float HoverScaleFactor = 1.2f;
        private float mouseDownTime;
        private const float MaxClickDuration = 0.5f;

        private void OnMouseDown()
        {
            Debug.Log("OnMouseDown");
        }

        public void PointSelected()
        {
            if (NodeData.IsOpen)
            {
                Debug.Log($"Point Was Selected:\nFloor: {NodeData.GetPoint.FloorLevel}\nEvent is: {NodeData.NodeTypeEnum}");
   
            }
        }
        public void PointLockState(bool state)
        {
            _backgroundImg.color = (state) ? Color.green : Color.red;
        }


        public void SetUp(Node data, NodePointAbstSO bluePrint)
        {
            NodeData = data;
            BluePrintNode = bluePrint;
          //  _sr.sprite = bluePrint.Icon;
            _backgroundImg.color = data.IsOpen ? Color.black : Color.white;

            if (data.NodeTypeEnum == NodeType.Boss_Enemy) 
                transform.localScale *= 1.5f;

            SetState(NodeStates.Locked);
        }

        public void SetState(NodeStates state)
        {
            NodeData.SetState(state);
            switch (state)
            {
                case NodeStates.Locked:
                 
                    _sr.color = MapView.Instance.lockedColor;
                    break;
                case NodeStates.Visited:
                    _sr.color = MapView.Instance.visitedColor;
                    break;
                case NodeStates.Attainable:
                    _sr.color = Color.green;
                    break;
                default:
                    break;
            }
        }
    }

    public class Node
    {
        public Node(NodeType type, Point p )
        {
            NodeTypeEnum = type;
            GetPoint = p;
            IsOpen = p.FloorLevel == 0;
            _connectedTo = new List<Node>();
            _connectedFrom = new List<Node>();
        }
  
        [SerializeField] bool _isOpen;
        [SerializeField] NodeStates _nodeState;
        [SerializeField] NodeType _nodeType;
        [SerializeField] List<Node> _connectedTo;
        [SerializeField] List<Node> _connectedFrom;
        Point _point;
        public Vector2 position;

        public List<Node> ConnectTo => _connectedTo;
        public List<Node> ConnectFrom => _connectedFrom;
        public Point GetPoint { get => _point; private set => _point = value; }
        public bool IsOpen { get => _isOpen;private set => _isOpen = value; }
        public NodeStates NodeState => _nodeState;
        public NodeType NodeTypeEnum { get => _nodeType;private set => _nodeType = value; }

        public void SetState(NodeStates state) { _nodeState = state; IsOpen = state == NodeStates.Attainable; }
    }

    public class Point
    {
        public Point(int FloorLevel, int Index)
        {
            this.Index = Index;
            this.FloorLevel = FloorLevel;
        }
        public int FloorLevel;
        public int Index;
    }

    public enum NodeStates
    {
       Locked,
       Visited,
       Attainable,
    }
}