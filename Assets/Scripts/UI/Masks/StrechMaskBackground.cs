using CardMaga.Trackers;
using UnityEngine;

public class StrechMaskBackground : MonoBehaviour
{
    [SerializeField] private RectTransform _background;
    [SerializeField] private RectTransform _hole;
    [SerializeField] private RectTransform _maskHolder;
    [SerializeField] private TrackerID _trackerID;
    [SerializeField] private bool _strechOnEnable;
    private TutorialClickHelper _tutorialClickHelper;
    private TrackerHandler _trackerHandler;

    private void OnEnable()
    {
        if (_strechOnEnable)
            StrechMask();
    }

    public void StrechMask()
    {
        GetInstances();
        GetMaskHolder();
        LoadObject();
        SetParent(_maskHolder);
        ResetRectScale();
        transform.SetParent(_hole);
    }

    public void OnlyLoadMask()
    {
        GetInstances();
        GetMaskHolder();
        LoadObject();
    }

    private void GetMaskHolder()
    {
        if (_trackerID != null)
            _maskHolder = _trackerHandler.GetTracker(_trackerID).RectTransform;

        else
            Debug.Log("There is no tracker here");
    }

    private void GetInstances()
    {
        _tutorialClickHelper = TutorialClickHelper.Instance;
        _trackerHandler = TrackerHandler.Instance;
    }

    private void LoadObject()
    {
            _tutorialClickHelper.LoadObject(true, true, null, _maskHolder);
    }

    public void ReturnObject()
    {
            _tutorialClickHelper.ReturnObjects();

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
