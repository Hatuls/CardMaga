using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardMaga.UI.PopUp
{
    public class TransitionData
    {
        private Vector2 _destination;
        private TransitionPackSO _transitionPackSO;
        public TransitionPackSO TransitionPackSO => _transitionPackSO;
        public Vector2 Destination => _destination;

        public TransitionData(TransitionPackSO transitionPackSO, Vector2 destination)
        {
            _transitionPackSO = transitionPackSO;
            _destination = destination;
        }
    }
}

