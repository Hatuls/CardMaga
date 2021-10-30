
using UnityEngine;
using Sirenix.OdinInspector;

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
                Debug.Log($"Point Was Selected:\nFloor: {NodeData.point.y}\nEvent is: {NodeData.NodeTypeEnum}");
   
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

    public enum NodeStates
    {
       Locked,
       Visited,
       Attainable,
    }
}