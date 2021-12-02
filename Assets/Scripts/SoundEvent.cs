using UnityEngine;

[CreateAssetMenu (fileName = "New Sound Event SO" , menuName = "Cfir/SoundEventSO")]
public class SoundEvent : ScriptableObject
{
    [SerializeField]
    public string EventPathName;

#if UNITY_EDITOR
    [Sirenix.OdinInspector.ShowInInspector] public string FullEventName => string.Concat(AudioManager.FmodEventString, EventPathName);
#endif
}
