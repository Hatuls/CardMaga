using CardMaga.Trackers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrechMaskBackground : MonoBehaviour
{
    [SerializeField] RectTransform _holder;
    [SerializeField] RectTransform _background;
    [SerializeField] RectTransform _hole;
    [SerializeField] RectTransform _maskHolder;
    [SerializeField] TrackerHandler _trackerHandler;
    [SerializeField] TrackerID _trackerID;

    private void OnEnable()
    {
        if(_trackerID!=null)
        _holder = _trackerHandler.GetTracker(_trackerID).RectTransform;
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
