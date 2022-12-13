using CardMaga.Trackers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerTutoriaPanelLoader : MonoBehaviour
{
    private RectTransform _trackerRect;

    public void LoadTrackerOnTutoriaPanel(TrackerID trackerID)
    {
        _trackerRect = TrackerHandler.GetTracker(trackerID).RectTransform;
        TutorialClickHelper.Instance.LoadObject(true, true, null, _trackerRect);
    }
}
