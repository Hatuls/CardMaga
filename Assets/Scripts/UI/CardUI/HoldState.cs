using UnityEngine;

namespace Battles.UI.CardUIAttributes
{
    internal class HoldState : CardUIAbstractState
    {
        CardTranslations _translation;
        static float _cardUIFollowUP;
        float alpha;

        private const float StationaryOffset = 30f;
        float scaleTimeOffset = 0.35f;
        float currentTime = 0;
        public HoldState(CardUI card, CardStateMachine cardStateMachine) : base(card.GFX.GetRectTransform, cardStateMachine)
        {
            alpha = card.Settings.AlphaWhenHold;
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
                    //    Debug.Log($"Moving! \ntouch position :{touchPos.position}\ncard position: {_cardStateMachine.Rect.position}\n distance between: {Vector2.Distance(touchPos.position, _cardStateMachine.Rect.position)}");
             


                    currentTime += Time.deltaTime;
                    if (Vector2.Distance(CardStateMachine.TouchPos, touchPos.position) < StationaryOffset
                        && currentTime > scaleTimeOffset)
                    {
                        Debug.Log($"Time Exceeds! {currentTime} / {scaleTimeOffset}");
                        _cardStateMachine.MoveToState(CardStateMachine.CardUIInput.Zoomed);
                        return;
                    }


                    Debug.Log("CardUI - State Hand - Stationary Touch ");


       float speed = _cardUIFollowUP;
                    //   Debug.LogWarning($"SPEED: {speed}");
                    _translation.MoveCard(false, touchPos.position, speed);
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    // check if its close to starting position 
                    bool succed = false;
                    if (IsAboveTheTouchLine(touchPos.position))
                    {
                        Debug.LogWarning("<a>Above The Line!</a>");
                        succed = CardUIHandler.Instance.TryExecuteCardUI(_cardStateMachine.CardReference);
                    }
                    Debug.Log("!!!!");
                    CardUIHandler.Instance.CardUITouchedReleased(succed, _cardStateMachine.CardReference);
                    break;
                default:
                    break;
            }
        }

        public override void OnStateEnter()
        {
            currentTime = 0;
            _cardStateMachine.CardReference.Inputs.GetCanvasGroup.alpha = alpha;
            //     OnTick(InputManager.PlayerTouch.Value);
            InputManager.Instance.AssignObjectFromTouch(_cardStateMachine.CurrentState);
         //   _cardStateMachine.CardReference.CardTranslations.MoveCard(true, InputManager.PlayerTouch.Value.position, _cardStateMachine.CardReference.Settings.GetCardFollowDelay);
        }
        public override void OnStateExit()
        {
            currentTime = 0;
            _cardStateMachine.CardReference.Inputs.GetCanvasGroup.alpha = 1f;
        }
        public static bool IsAboveTheTouchLine(in Vector2 touchPos) => (touchPos.y >= CardUIManager.Instance.GetInputHandLine);
    }
}