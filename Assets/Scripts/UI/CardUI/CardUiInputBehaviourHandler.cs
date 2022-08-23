using CardMaga.Input;
using CardMaga.UI.Card;
using UnityEngine;

public class CardUiInputBehaviourHandler : InputBehaviourHandler<CardUI>
{
    [SerializeField] private CardUIInputBehaviourSO[] _inputBehaviourSos;
    
    public override InputBehaviour<CardUI>[] InputBehaviours { get => _inputBehaviourSos; }
}
