using UnityEngine;

namespace Battles.UI.CardUIAttributes
{
    internal class ZoomState : CardUIAbstractState
    {
        const float StationaryOffset = 60f;
        Vector2 location;
        public ZoomState(RectTransform rectm, CardStateMachine cardStateMachine) : base(rectm, cardStateMachine)
        {
        }

        public override CardStateMachine.CardUIInput State =>  CardStateMachine.CardUIInput.Zoomed;

        public override void OnMouse()
        {
            throw new System.NotImplementedException();
        }

        public override void OnTick(in Touch touchPos)
        {
            switch (touchPos.phase)
            {

                case TouchPhase.Began:
                    break;

                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    if (Vector2.Distance(CardStateMachine.TouchPos, touchPos.position) > StationaryOffset)
                    {
                        location = touchPos.position;
                        _cardStateMachine.MoveToState(CardStateMachine.CardUIInput.Hold);
                    }
                    break;



                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                default:
                    OnStateExit();
                    CardUIHandler.Instance.CardUITouchedReleased(false,_cardStateMachine.CardReference);

                    break;
            }
        }
        public override void OnStateEnter()
        {
            CardUIHandler.Instance.ToZoomCardUI();
        }
        public override void OnStateExit()
        {
            CardUIHandler.Instance.ToUnZoomCardUI(location);
        }
    }
}