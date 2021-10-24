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


        [SerializeField] Image _backgroundImg;
        [SerializeField] Image _img;
        [SerializeField] bool _isOpen;
        [SerializeField] EventPointType _eventPointType;

        [SerializeField] MapEventPoint[] _connectTO;
        public MapEventPoint[] ConnectTo => _connectTO;
        public byte FloorLevel { get => _floorLevel; set => _floorLevel = value; }
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
}