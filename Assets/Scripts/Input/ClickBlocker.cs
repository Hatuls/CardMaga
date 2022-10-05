using System.Collections;
using UnityEngine;
using System;

[System.Serializable]
public class ClickBlocker 
{
    [SerializeField] CanvasGroup _canvasGroup;
    private ClickHelper _clickHelper;
    public void Init(ClickHelper clickHelper)
    {
        _clickHelper = clickHelper;
    }

    public void BlockInput()
    {
        _clickHelper.Clicker.ForceChangeState(false);
    }

    public void UnblockInput()
    {
        _clickHelper.Clicker.ForceChangeState(true);
    }

    public void BlockInputForSeconds(float seconds, Action onComplete= null)
    {
        _clickHelper.StartCoroutine(BlockForSeconds(seconds, onComplete));

    }

    private IEnumerator BlockForSeconds(float seconds, Action onComplete= null)
    {
        BlockInput();
        yield return new WaitForSeconds(seconds);
        if(onComplete!=null)
        onComplete.Invoke();

        UnblockInput();
    }

}
