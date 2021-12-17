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

    public void PlayVoiceSound(float param) => CurrentCharacter.SoundOnAttack.PlaySound(param);
    public void PlayHitSound(float param) => CurrentCharacter.GetHitSounds.PlaySound(param);
    public void PlayComboSound() => CurrentCharacter.ComboSounds.PlaySound();
    public void PlayVictorySound() => CurrentCharacter.VictorySound.PlaySound();
    public void PlayKOSound() => CurrentCharacter.DeathSounds.PlaySound();
    public void PlayTauntSound() => CurrentCharacter.TauntSounds.PlaySound();

    
}
