using UnityEngine;

[CreateAssetMenu (fileName = "New Sound Event SO" , menuName = "Cfir/SoundEventSO")]
public class SoundEventSO : ScriptableObject
{
    [SerializeField]
    public string EventPathName;

#if UNITY_EDITOR
    [Sirenix.OdinInspector.ShowInInspector] public string FullEventName => string.Concat(AudioManager.FmodEventString, EventPathName);
#endif
    public void PlaySound() => AudioManager.Instance.PlaySoundEvent(EventPathName);
}
