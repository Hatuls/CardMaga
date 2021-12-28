using UnityEngine;

[CreateAssetMenu(fileName = "New Sound Event SO", menuName = "Cfir/SoundEventSO With Parameter")]
public class SoundEventWithParamsSO : SoundEventSO
{
    public string ParameterName;
    public bool IgnoreSeekReed;
    public  void PlaySound(float value)
    {
        AudioManager.Instance.PlaySoundEvent(this, value);

    }
}
