using System;
using UnityEngine;
namespace UI
{
    public class EventNotifier : BaseEventNotifier
    {
        [Range(0, byte.MaxValue)]
        [SerializeField]
        private float _loopTime;

        public float Counter { get; private set; }

        public override bool ConditionsMet()
        {
            bool result = Counter >= _loopTime;

            if (result)
                Counter = 0;

            return result;
        }
    }
}