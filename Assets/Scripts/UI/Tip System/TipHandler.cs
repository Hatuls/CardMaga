using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TipHandler : MonoBehaviour
{
    [SerializeField] private TipsScriptableObject _tips;
    [SerializeField] private TMP_Text _text;

    private int _tipCount;
    private List<int> _shownTipIndexs;

    [Range(0, 10)] [SerializeField] private float _delayBetweenTips;

    private WaitForSeconds _waitForSeconds;
    
    private void Start()
    {
        _waitForSeconds = new WaitForSeconds(_delayBetweenTips);
        _tipCount = _tips.GetLength;
        
        _shownTipIndexs = new List<int>(_tipCount);
        
        StartCoroutine(CycleTips());
    }

    private IEnumerator CycleTips()
    {
        while (true)
        {
            _text.text = GetTip();
            yield return _waitForSeconds;    
        }
        
    }

    private string GetTip()
    {
        int tipIndex = -1;

        if (_shownTipIndexs.Count == _tipCount)
            _shownTipIndexs.Clear();
        
        
        while (tipIndex == -1 || _shownTipIndexs.Contains(tipIndex))
        {
            tipIndex = Random.Range(0, _tipCount);
        }
        
        _shownTipIndexs.Add(tipIndex);

        return _tips.GetTip(tipIndex);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
