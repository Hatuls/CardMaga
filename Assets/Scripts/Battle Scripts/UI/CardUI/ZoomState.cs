using UnityEngine;

namespace Battles.UI.CardUIAttributes
{
    internal class ZoomState : CardUIAbstractState
    {
       
        const float StationaryOffset = 60f;
        Vector2 location;
        CardUI reference;
        public ZoomState(RectTransform rectm, CardStateMachine cardStateMachine) : base(rectm, cardStateMachine)
        {

            reference = _cardStateMachine.CardReference;
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
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                        reference.CardTranslations?.MoveCard(false, UIManager.MiddleScreenPosition, reference.Settings.GetCardScaleDelay);
                  
                    if (Vector2.Distance(CardStateMachine.TouchPos, touchPos.position) > StationaryOffset)
                    {
                        location = touchPos.position;
                        _cardStateMachine.MoveToState(CardStateMachine.CardUIInput.Hold);
                    }

                    
                    break;



                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                default:
                    if (HoldState.IsAboveTheTouchLine(touchPos.position))
                    {
                        Debug.LogWarning("<a>Above The Line!</a>");
                        CardUIHandler.Instance.CardUITouchedReleased(CardUIHandler.Instance.TryExecuteCardUI(_cardStateMachine.CardReference), _cardStateMachine.CardReference);
                    }


         
                    OnStateExit();
                    CardUIHandler.Instance.CardUITouchedReleased(false,_cardStateMachine.CardReference);

                    break;
            }
        }
        public override void OnStateEnter()
        {
            GameBattleDescriptionUI.Instance.CloseCardUIInfo();
            CardUIHandler.Instance.ToZoomCardUI();
            reference.CardTranslations?.MoveCard(false, UIManager.MiddleScreenPosition, reference.Settings.GetCardScaleDelay);
        }
        public override void OnStateExit()
        {
        
            CardUIHandler.Instance.ToUnZoomCardUI(location);
            reference.CardTranslations?.MoveCard(false,InputManager.PlayerTouch.Value.position, reference.Settings.GetCardScaleDelay);
       
        }
    }
}