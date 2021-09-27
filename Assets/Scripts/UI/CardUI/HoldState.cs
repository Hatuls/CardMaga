using UnityEngine;

namespace Battles.UI.CardUIAttributes
{
    internal class HoldState : CardUIAbstractState
    {
        CardTranslations _translation;
        static float _cardUIFollowUP;

        public HoldState(CardUI card, CardStateMachine cardStateMachine) : base(card.GFX.GetRectTransform, cardStateMachine)
        {

            _translation = cardStateMachine.CardReference.CardTranslations;
            _cardUIFollowUP = card.Settings.GetCardFollowDelay;
        }

        public override CardStateMachine.CardUIInput State => CardStateMachine.CardUIInput.Hold;

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
                    Debug.Log("Moving");
                    _translation.MoveCard(false, touchPos.position, _cardUIFollowUP);
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    // check if its close to starting position 
                    if (IsAboveTheTouchLine(touchPos.position))
            {
                        Debug.LogWarning("<a>Above The Line!</a>");
                        CardUIHandler.Instance.TryExecuteCardUI(_cardStateMachine.CardReference);
                    }
                   
                    CardUIHandler.Instance.CardUITouchedReleased(_cardStateMachine.CardReference);
                    break;
                default:
                    break;
            }
        }

        public override void OnStateEnter()
        {
            _translation.MoveCard(false, InputManager.PlayerTouch.Value.position, _cardUIFollowUP);
        }
        private bool IsAboveTheTouchLine(in Vector2 touchPos) => (touchPos.y >= CardUIManager.Instance.GetInputHandLine);
    }
}