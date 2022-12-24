using TMPro;
using UnityEngine;
using System;
using System.Collections;

public abstract class LookingForOpponentAnimationTextBase : MonoBehaviour
{
    [SerializeField] protected string _newEnteringText;
    [SerializeField] protected float _showingTime;
    public abstract IEnumerator ShowText(TextMeshProUGUI _lookingForOpponentText, Action MoveToNextText = null);
}
