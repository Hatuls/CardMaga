
using DesignPattern;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Map
{
    public class NodeMap : MonoBehaviour, IObserver
    {
        [SerializeField]
        Animator _animator;
        int IsAttentableHash = Animator.StringToHash("IsAttenable");

        [ShowInInspector]
        public Node NodeData { get; private set; }

        [ShowInInspector]
        public NodePointAbstSO BluePrintNode { get; private set; }

        [ShowInInspector]
        public NodeType nodeType => NodeData == null ? NodeType.None : NodeData.NodeTypeEnum;
        [SerializeField] SpriteRenderer _backgroundImg;
        [SerializeField] SpriteRenderer _sr;

        [SerializeField]
        Vector3 _startSize;
        [SerializeField]
        float _scaleWhenAttendable = 1.1f;

        [SerializeField]
        float _bossScale = 1.5f;

        [Tooltip("0 - Normal\n1 - Gray Scale")]
        [Range(0, 1)]
        [SerializeField]
        float _lockGrayScale;

        [Tooltip("0 - Normal\n1 - Gray Scale")]
        [Range(0, 1)]
        [SerializeField]
        float _attainableGrayScale;

        [Tooltip("0 - Normal\n1 - Gray Scale")]
        [Range(0, 1)]
        [SerializeField]
        float _visitedGrayScale;


        [SerializeField] ObserverSO _observer;
        [SerializeField] BoxCollider2D _boxCollider;
        private void Start()
        {
            _startSize = transform.localScale;
            SetTrigger(true);
        }
        private void OnEnable()
        {
            //   _observer.Subscribe(this);
        }
        bool toRecieveInputs = true;
        private void OnDisable()
        {
            //  _observer.UnSubscribe(this);
        }
        private void SetTrigger(bool state)
        {
            _boxCollider.enabled = state;
        }
        bool _isHoldingDown;
        private void OnMouseDown()
        {
            if (!_isHoldingDown && !EventSystem.current.IsPointerOverGameObject()  && !IsPointerOverUIObject())
            {
                _isHoldingDown = true;
            }

        }
        private void OnMouseUp()
        {
            if (_isHoldingDown && !EventSystem.current.IsPointerOverGameObject() && !IsPointerOverUIObject())
            {
                MapPlayerTracker.Instance.SelectNode(this);
            }
            _isHoldingDown = false;
        }
        private bool IsPointerOverUIObject()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
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
            //  _sr.color = bluePrint.PointColor;
            _sr.sprite = bluePrint.Icon;
            _backgroundImg.sprite = bluePrint.BackGroundImage;
            //_backgroundImg.color =Color.cyan;

            SetState(NodeStates.Locked);




        }

        public void SetState(NodeStates state)
        {
            NodeData.SetState(state);
            switch (state)
            {
                case NodeStates.Locked:
                    transform.localScale = _startSize;
                    _animator.SetBool(IsAttentableHash, false);
                    SetGrayScale(_lockGrayScale);
                    break;
                case NodeStates.Visited:
                    transform.localScale = _startSize;
                    _animator.SetBool(IsAttentableHash, false);
                    SetGrayScale(_visitedGrayScale);
                    break;
                case NodeStates.Attainable:
                    SetGrayScale(_attainableGrayScale);
                    transform.localScale = _startSize * _scaleWhenAttendable;
                    _animator.SetBool(IsAttentableHash, true);
                    break;
                default:
                    break;
            }

            if (NodeData.NodeTypeEnum == NodeType.Boss_Enemy)
                transform.localScale = Vector3.one * _bossScale;
        }
        private void SetGrayScale(float scale)
        {
            _backgroundImg.material.SetFloat("_EffectAmount", scale);
            _sr.material.SetFloat("_EffectAmount", scale);
        }
        public void OnNotify(IObserver Myself)
        {
            if (Myself == null)
                toRecieveInputs = true;

            else
                toRecieveInputs = false;

            SetTrigger(toRecieveInputs);
        }
    }

    public enum NodeStates
    {
        Locked,
        Visited,
        Attainable,
    }
}