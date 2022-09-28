using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void BlockInputForSeconds(float seconds)
    {
        _clickHelper.StartCoroutine(BlockForSeconds(seconds));
    }

    private IEnumerator BlockForSeconds(float seconds)
    {
        BlockInput();
        yield return new WaitForSeconds(seconds);
        UnblockInput();
    }

}
