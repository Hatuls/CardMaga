using System.Collections.Generic;
namespace CardMaga.Trackers
{

    public static class TrackerHandler
    {
        private static List<Tracker> _trackers = new List<Tracker>();

        public static Tracker GetTracker(TrackerID trackerID)
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
        public static bool CheckTrackerRegistered(TrackerID trackerID)
        {
            for (int i = 0; i < _trackers.Count; i++)
            {
                if (_trackers[i].TrackerID == trackerID)
                {
                    return true;
                }
            }
            return false;
        }
        internal static void RemoveTracker(Tracker tracker)
        => _trackers.Remove(tracker);


        public static void AddTracker(Tracker tracker)
            => _trackers.Add(tracker);

    }
}