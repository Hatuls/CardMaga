
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Map
{
    [System.Serializable]
    public class Node
    {
        public Node(NodeType type, Point p )
        {
            NodeTypeEnum = type;
            point = p;
            IsOpen = p.y == 0;
        }
  
        [SerializeField] bool _isOpen;
        [SerializeField] NodeStates _nodeState;
        [SerializeField] NodeType _nodeType;
        [SerializeField] Point _point;
        [SerializeField] public Vector2 position;

        public Point point { get => _point; set => _point = value; }
        public readonly List<Point> incoming = new List<Point>();
        public readonly List<Point> outgoing = new List<Point>();
        public void AddIncoming(Point p)
        {
            if (incoming.Any(element => element.Equals(p)))
                return;

            incoming.Add(p);
        }

        public void AddOutgoing(Point p)
        {
            if (outgoing.Any(element => element.Equals(p)))
                return;

            outgoing.Add(p);
        }

        public void RemoveIncoming(Point p)
        {
            incoming.RemoveAll(element => element.Equals(p));
        }

        public void RemoveOutgoing(Point p)
        {
            outgoing.RemoveAll(element => element.Equals(p));
        }

        public bool HasNoConnections()
        {
            return incoming.Count == 0 && outgoing.Count == 0;
        }

        public bool IsOpen { get => _isOpen;private set => _isOpen = value; }
        public NodeStates NodeState => _nodeState;
        public NodeType NodeTypeEnum { get => _nodeType;private set => _nodeType = value; }

        public void SetState(NodeStates state) { _nodeState = state; IsOpen = state == NodeStates.Attainable; }
    }
}