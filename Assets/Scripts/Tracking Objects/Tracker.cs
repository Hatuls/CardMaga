using UnityEngine;
namespace CardMaga.Trackers
{
    public class Tracker : MonoBehaviour
    {
        [SerializeField]
        private TrackerID _trackID;
        [SerializeField]
        private RectTransform _rectTransform;
        public virtual RectTransform RectTransform => _rectTransform;

        public TrackerID TrackerID => _trackID;

        private void Start()
        {
            TrackerHandler.Instance.AddTracker(this);
        }

    }

    //public class HandUIFirstCardTracker: Tracker
    //{
    //    [SerializeField]private UI.HandUI _handUI;
    //    public override RectTransform RectTransform => base.RectTransform;

    //    private RectTransform FirstCard()
    //    {
    //        //return _handUI.
    //    }
    //}
}