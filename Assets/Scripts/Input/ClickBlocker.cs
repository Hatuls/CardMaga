using System.Collections;
using UnityEngine;
using System;

[System.Serializable]
public class ClickBlocker 
{
    private ClickHelper _clickHelper;
    private TutorialClickHelper _tutorialClickHelper;
    public void InitClickHelper(ClickHelper clickHelper)
    {
        _clickHelper = clickHelper;
    }

    public void InitTutorialClickHelper(TutorialClickHelper tutorialClickHelper)
    {
        _tutorialClickHelper = tutorialClickHelper;
    }

    public void BlockInput()
    {
        Debug.Log("Input blocked");
        _clickHelper.ZoomInClicker.ForceChangeState(false);
    }

    public void UnblockInput()
    {
        Debug.Log("Input unlocked");
        _clickHelper.ZoomInClicker.ForceChangeState(true);
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
