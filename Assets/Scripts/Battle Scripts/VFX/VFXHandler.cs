using System.Collections;
using UnityEngine;



public class VFXBase : MonoBehaviour
{
    IStayOnTarget _stayOnTarget;
    public IStayOnTarget StayOnTarget => _stayOnTarget;
    Coroutine coroutine;
    public void Cancel()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
        Destroy(this);
    }

    public void StartVFX(IStayOnTarget stayOnTarget, Transform GetTransform)
    {
        _stayOnTarget = stayOnTarget;
        transform.SetParent(GetTransform);
        transform.SetPositionAndRotation(GetTransform.position, GetTransform.rotation);
        coroutine = StartCoroutine(Detach(stayOnTarget));
    }
    private IEnumerator Detach(IStayOnTarget stayOnTarget)
    {
        if (stayOnTarget.DelayUntillDetach < 0 || stayOnTarget.StayOnTarget == true)
            yield break;
        float counter = 0;
        while (counter <= stayOnTarget.DelayUntillDetach)
        {
            yield return null;
            counter += Time.deltaTime;
        }
        transform.SetParent(null);
       
    }
}
public interface IVFXPackage
{
    IStayOnTarget StayOnTarget { get; }
    void Cancel();
    void StartVFX();
}
public interface IStayOnTarget
{
    bool StayOnTarget { get; }
    float DelayUntillDetach { get; }
}