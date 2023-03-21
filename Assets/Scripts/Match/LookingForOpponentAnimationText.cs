using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class LookingForOpponentAnimationText : MonoBehaviour
{
    private const string DOT = ".";
    [SerializeField]
    private string LFO = "Looking For Opponent";
    [SerializeField] TextMeshProUGUI _lookingForOpponentText;
    [SerializeField,Range(0,1f)] float _delayBetweenDots;
    [SerializeField]
    private string _worthyOpponentText;
    private bool _isActive;

    private void OnEnable()
    {
        StartCoroutine(ChangeAnimation());
    }

    public IEnumerator ChangeAnimation()
    {
        WaitForSeconds delay = new WaitForSeconds(_delayBetweenDots);
        _isActive = true;

        const int dotStages = 4;

        string[] dots = CreateDotSequence(dotStages);

        int totalAmount = dots.Length;
        int counter = -1;
        do
        {
            counter++;
            string result = string.Concat(LFO, dots[counter % totalAmount]);
            _lookingForOpponentText.text = result;
            yield return delay;
        } while (_isActive);


        string[] CreateDotSequence(int amount)
        {
            string[] sequence = new string[amount];
            sequence[0] = "";

            for (int i = 1; i < amount; i++)
                sequence[i] = string.Concat(sequence[i - 1], DOT);

            return sequence;
        }
    }


    public void FoundOpponent()
    {
        StopAllCoroutines();
        _lookingForOpponentText.text =_worthyOpponentText;
    }
    private void OnDisable()
    {
        _isActive = false;
        StopAllCoroutines();
    }
}