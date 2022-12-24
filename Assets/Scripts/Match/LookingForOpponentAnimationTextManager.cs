using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LookingForOpponentAnimationTextManager : MonoBehaviour
{
    [SerializeField] List<LookingForOpponentAnimationTextBase> _textList;
    [SerializeField] TextMeshProUGUI _lookingForOpponentText;
    private int _currentTextIndex=-1;

    private void OnEnable()
    {
        ChangeAnimation();
    }

    public void ChangeAnimation()
    {
        _currentTextIndex++;
        StartCoroutine(_textList[_currentTextIndex% _textList.Count].ShowText(_lookingForOpponentText, ChangeAnimation));
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}