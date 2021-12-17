using Battles;
using UnityEngine;

public class AnimationBodyPartSoundsHandler : MonoBehaviour
{
    [SerializeField]
    SoundEventSO OnBlockDamage; // shield damage
    [SerializeField]
    SoundEventSO SoundOnPunch; // punch damage
    [SerializeField]
    SoundEventSO SoundOnKick; // kick damage

    CharacterSO _currentCharacter;
 
    public CharacterSO CurrentCharacter { get => _currentCharacter; set => _currentCharacter = value; }

    public void PlayKickSound()
        => SoundOnKick.PlaySound();
    public void PlayPunchSound()
        => SoundOnPunch.PlaySound();
    public void PlayBlockDamage()
        => OnBlockDamage.PlaySound();

    public void PlayVoiceSound(float param)=> CurrentCharacter.
    public void PlayComboSound() => CurrentCharacter.ComboSound.Sounds.PlaySound();
    public void PlayVictorySound() => CurrentCharacter.VictorySound.Sounds.PlaySound();
    public void PlayHitSound() => CurrentCharacter.GetHitSounds.Sounds.PlaySound();
    public void PlayKOSound() => CurrentCharacter.DeathSounds.Sounds.PlaySound();
    public void PlayTauntSound() => CurrentCharacter.TauntSounds.Sounds.PlaySound();

    
}
