using UnityEngine;

[CreateAssetMenu(fileName = "New Sound Event SO", menuName = "Cfir/SoundEventSO")]
public class SoundEventSO : ScriptableObject
{
    [SerializeField]
    public string EventPathName;

    public bool IsStackable = true;
#if UNITY_EDITOR
    [Sirenix.OdinInspector.ShowInInspector] public string FullEventName => string.Concat(AudioManager.FmodEventString, EventPathName);
#endif
    
    public virtual void PlaySound() => AudioManager.Instance.PlaySoundEvent(this);
}
