using System.Collections;
using UnityEngine;

public abstract class VFXBase : MonoBehaviour
{
    IStayOnTarget _stayOnTarget;
    public IStayOnTarget StayOnTarget => _stayOnTarget;
    Coroutine coroutine;
    public virtual void Cancel()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
    }
    public void DestroyVFX() => Destroy(this);
    public abstract bool IsPlaying { get;}
    public virtual void StartVFX(IStayOnTarget stayOnTarget, Vector3 position)
    {
        _stayOnTarget = stayOnTarget;
        transform.position = position;
        coroutine = StartCoroutine(OnDelayCounter(stayOnTarget));
    }
    public virtual void StartVFX(IStayOnTarget stayOnTarget, Transform GetTransform)
    {
        _stayOnTarget = stayOnTarget;
        transform.SetParent(GetTransform);

  
            transform.SetPositionAndRotation(GetTransform.position, stayOnTarget.ToUseBodyRotation ? GetTransform.rotation : Quaternion.identity);
  

        coroutine = StartCoroutine(OnDelayCounter(stayOnTarget, () => { if (stayOnTarget.StayOnTarget == true) transform.SetParent(null); }));
    }
    protected IEnumerator OnDelayCounter(IStayOnTarget stayOnTarget, System.Action OnFinish = null)
    {
        if (stayOnTarget.DelayUntillDetach < 0)
        {
            float counter = 0;
            while (counter <= stayOnTarget.DelayUntillDetach)
            {
                yield return null;
                counter += Time.deltaTime;
            }

        }
        OnFinish?.Invoke();
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
    bool ToUseBodyRotation { get; }
    bool StayOnTarget { get; }
    float DelayUntillDetach { get; }
}