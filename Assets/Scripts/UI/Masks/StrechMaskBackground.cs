using CardMaga.Trackers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StrechMaskBackground : MonoBehaviour
{
    [SerializeField] private RectTransform _background;
    [SerializeField] private RectTransform _hole;
    [SerializeField] private RectTransform _maskHolder;
    [SerializeField] private TrackerID _trackerID;
    [SerializeField] private bool _strechOnEnable;
    [SerializeField] private bool _loadMaskOnTutorial;
    private TutorialClickHelper _tutorialClickHelper;
    private TrackerHandler _trackerHandler;
    
    private void OnEnable()
    {
        if (_strechOnEnable)
            StrechMask();
    }

    public void StrechMask()
    {
        _trackerHandler = TrackerHandler.Instance;
        if (_trackerID != null)
        {
            _maskHolder = _trackerHandler.GetTracker(_trackerID).RectTransform;
            //RectTransform _maskParent = _maskHolder.transform.parent;
            
            //_maskHolder = _maskParent.getre
        }
        //if (_loadMaskOnTutorial)
        //{
        //    _tutorialClickHelper = TutorialClickHelper.Instance;
        //    _tutorialClickHelper.LoadObject(true, true, null, _maskHolder);
        //}
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
