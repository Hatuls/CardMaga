using Map;
using System.Collections;
using System.Collections.Generic;
using UI.Map;
using UnityEngine;
using UnityEngine.UI;

namespace Meta.Map
{
    public class MapEventPoint : MonoBehaviour
    {
        [SerializeField] byte _floorLevel;
        [SerializeField] bool _isOpen;
        [SerializeField] EventPointType _eventPointType;

        [SerializeField] MapEventPoint[] _connectFrom;
        public MapEventPoint[] ConnectTo => _connectFrom;
        public byte FloorLevel { get => _floorLevel; set => _floorLevel = value; }
        public bool IsOpen { get => _isOpen; private set => _isOpen = value; }
        public EventPointType EventPointType { get => _eventPointType; private set => _eventPointType = value; }
        [SerializeField] Image _backgroundImg;
        [SerializeField] Image _img;
        public void PointSelected()
        {
            if (_isOpen)
            {
                Debug.Log($"Point Was Selected:\nFloor: {_floorLevel}\nEvent is: {_eventPointType}");
                MapUIManager.Instance.MapPointSelected(this);
            }
        }
        public void PointLockState(bool state)
        {
            _backgroundImg.color = (state) ? Color.green : Color.red;
               _isOpen = state; 
        }
        public void Init(byte _floorLevel, EventPointAbstSO type)
        {
            _isOpen = (_floorLevel == 0);
            _eventPointType = type.PointType;
            _img.sprite = type.Icon;
        }
        public void Init(EventPointAbstSO type)
        {

            PointLockState(_floorLevel == 0);
            _eventPointType = type.PointType;
            _img.color = type.PointColor;


        }

        

    }

    public class EventPoint
    {
        public EventPoint(byte floorLevel,EventPointType type,params EventPoint[] points )
        {
            EventPointType = type;
            _floorLevel = floorLevel;
            IsOpen = _floorLevel == 0;
             _connectFrom = points;
        }
        [SerializeField] byte _floorLevel;
        [SerializeField] bool _isOpen;
        [SerializeField] EventPointType _eventPointType;

        [SerializeField] EventPoint[] _connectFrom;
        public EventPoint[] ConnectTo => _connectFrom;
        public byte FloorLevel { get => _floorLevel; set => _floorLevel = value; }
        public bool IsOpen { get => _isOpen;private set => _isOpen = value; }
        public EventPointType EventPointType { get => _eventPointType;private set => _eventPointType = value; }
    }
}