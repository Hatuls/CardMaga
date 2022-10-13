using System.Collections;
using System.Collections.Generic;
using CardMaga.Input;
using CardMaga.UI.Card;
using UnityEngine;

[CreateAssetMenu(fileName = "Input Group", menuName = "ScriptableObjects/Input/New Input Group")]
public class InputGroup : ScriptableObject
{
   [SerializeField] private List<TouchableItem<CardUI
   >> _inputs;
}
