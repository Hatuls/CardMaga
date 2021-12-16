using Battles;
using UnityEngine;

public class AnimationBodyPartSoundsHandler : MonoBehaviour
{
    [SerializeField]
    SoundEventSO OnBlockDamage;

    CharacterSO _currentCharacter;
 
    public CharacterSO CurrentCharacter { get => _currentCharacter; set => _currentCharacter = value; }

    public void PlayElbowHitSound()
        => CurrentCharacter.PunchSounds.Sounds.PlaySound();
    public void PlayHeadHitSound()
        => CurrentCharacter.PunchSounds.Sounds.PlaySound();
    public void PlayKickSound()
        => CurrentCharacter.KickSounds.Sounds.PlaySound();
    public void PlayPunchSound()
        => CurrentCharacter.PunchSounds.Sounds.PlaySound();

    public void PlayComboSound() => CurrentCharacter.ComboSound.Sounds.PlaySound();
    public void PlayVictorySound() => CurrentCharacter.VictorySound.Sounds.PlaySound();
    public void PlayHitSound() => CurrentCharacter.HitSounds.Sounds.PlaySound();
    public void PlayKOSound() => CurrentCharacter.KOSounds.Sounds.PlaySound();
    public void PlayTauntSound() => CurrentCharacter.TauntSounds.Sounds.PlaySound();

    public void PlayBlockDamage() => OnBlockDamage.PlaySound();
}
