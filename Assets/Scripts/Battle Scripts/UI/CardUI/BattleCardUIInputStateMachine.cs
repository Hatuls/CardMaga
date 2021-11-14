using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace Battles.UI.CardUIAttributes
{
    [CreateAssetMenu(fileName = "Behaviour", menuName = "ScriptableObject/Inputs/BattleCardInputs")]
    public class BattleCardUIInputStateMachine : CardStateMachine
    {
        public override void InitState(CardUI cardUI)
        {    
            CardReference = cardUI;
            _rect = CardReference.Inputs.Rect;
            const int states = 5;
            _statesDictionary = new Dictionary<CardUIInput, CardUIAbstractState>(states)
            {
                { CardUIInput.None, null },
                {CardUIInput.Locked, null },
                {CardUIInput.Hand, new HandState(_rect, this) },
                {CardUIInput.Hold, new HoldState(CardReference,this) },
                {CardUIInput.Zoomed, new ZoomState(_rect,this)}
            };

            _currentState = _statesDictionary[CardUIInput.None];
        }

        private void OnDrag(CardUI card,PointerEventData data)
        {

        }


    }












}