using Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Meta.Map
{
    public class MapEventPoint : MonoBehaviour
    {
        [SerializeField] byte _floorLevel;
        [SerializeField] Image _img;
        [SerializeField] bool _isOpen;
        [SerializeField] EventPointType _eventPointType;

        [SerializeField] MapEventPoint[] _connectTO;
        public void PointSelected()
        {
            if (_isOpen)
                Debug.Log($"Point Was Selected:\nFloor: {_floorLevel}\nEvent is: {_eventPointType}");
        }
        public void PointLockState(bool state) => _isOpen = state;
        public void Init(byte _floorLevel, EventPointAbstSO type)
        {
            _isOpen = (_floorLevel == 0);
            _eventPointType = type.PointType;
            _img.sprite = type.Icon;
        }
    }
}