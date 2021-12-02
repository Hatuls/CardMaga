using UnityEngine;

public class AnimationBodyPartSoundsHandler : MonoBehaviour
{
    [SerializeField]
    SoundEventSO OnPunchHitSound;
    [SerializeField]
    SoundEventSO OnKickHitSound;
    [SerializeField]
    SoundEventSO OnHeadHitSound;
    [SerializeField]
    SoundEventSO OnElbowSound;


    public void PlayElbowHitSound()
        => OnElbowSound.PlaySound();
    public void PlayHeadHitSound()
        => OnHeadHitSound.PlaySound();
    public void PlayKickSound()
        => OnKickHitSound.PlaySound();
    public void PlayPunchSound()
        => OnPunchHitSound.PlaySound();


}
