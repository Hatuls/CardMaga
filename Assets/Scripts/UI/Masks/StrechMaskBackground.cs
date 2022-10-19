using CardMaga.Trackers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StrechMaskBackground : MonoBehaviour
{
    [SerializeField] RectTransform _background;
    [SerializeField] RectTransform _hole;
    [SerializeField] RectTransform _maskHolder;
    [SerializeField] TrackerID _trackerID;

    private TrackerHandler _trackerHandler;
    
    private void OnEnable()
    {
        _trackerHandler = TrackerHandler.Instance;

        if (_trackerID!=null)
        _maskHolder = _trackerHandler.GetTracker(_trackerID).RectTransform;
        SetParent(_maskHolder);
        ResetRectScale();
        transform.SetParent(_hole);
    }
    private void SetParent(RectTransform rectTransform)
    {
        transform.SetParent(rectTransform);
    }

    private void ResetRectScale()
    {
        _background.offsetMin = Vector2.zero;
        _background.offsetMax = Vector2.zero;
    }
}
