using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LookingForOpponentAnimationTextDots : LookingForOpponentAnimationTextBase
{
    public override IEnumerator ShowText(TextMeshProUGUI _lookingForOpponentText, Action MoveToNextText = null)
    {
        _lookingForOpponentText.text = _newEnteringText;
        yield return new WaitForSeconds(_showingTime);
        if (MoveToNextText != null)
            MoveToNextText.Invoke();
    }
}
