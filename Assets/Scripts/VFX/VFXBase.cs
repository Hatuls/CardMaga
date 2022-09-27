using Temp;
using UnityEngine;

public abstract class VFXBase : MonoBehaviour
{
    IStayOnTarget _stayOnTarget;
    public IStayOnTarget StayOnTarget => _stayOnTarget;
    protected Coroutine coroutine;
    [SerializeField] SequenceHandler sequanceHandler;

    public virtual void Cancel()
    {
        sequanceHandler.StopSequence();
        gameObject.SetActive(false);
    }
    public void DestroyVFX() => Destroy(this);
    public abstract bool IsPlaying { get; }
    public virtual void StartVFX(IStayOnTarget stayOnTarget, Vector3 position)
    {
        _stayOnTarget = stayOnTarget;
        transform.position = position;
        sequanceHandler.StartSequance();
    }
    public virtual void StartVFX(IStayOnTarget stayOnTarget, Transform GetTransform, System.Action OnFinishDuration = null)
    {
        gameObject.SetActive(true);
        _stayOnTarget = stayOnTarget;
        transform.SetParent(GetTransform);
        transform.SetPositionAndRotation(GetTransform.position, stayOnTarget.ToUseBodyRotation ? GetTransform.rotation : Quaternion.identity);
        sequanceHandler.StartSequance();
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
    bool IsFromAnimation { get; }
}