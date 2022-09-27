
using System.Collections.Generic;
using UnityEngine;
namespace CardMaga.Trackers
{

    public class TrackerHandler : MonoBehaviour
    {
        private static TrackerHandler _instance;
        public static TrackerHandler Instance => _instance;
        private List<Tracker> _trackers = new List<Tracker>();

        public Tracker GetTracker(TrackerID trackerID)
        {
            for (int i = 0; i < _trackers.Count; i++)
            {
                if (_trackers[i].TrackerID == trackerID)
                {
                    return _trackers[i];
                }
            }
            throw new System.Exception("TrackerHandler: Tracker Was not found");
        }

        public void AddTracker(Tracker tracker)
            => _trackers.Add(tracker);
        private void Awake()
        {
            _trackers.Clear();
            _instance = this;
        }
        private void OnDestroy()
        {
            _instance = null;
        }
    }
}