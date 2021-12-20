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

    public void PlayHitSound(float param)
    {
        if (CurrentCharacter.SoundOnAttack == null)
            Debug.LogError($"Character: {CurrentCharacter.CharacterName} doenst have GetHitSounds!");
        else
            CurrentCharacter.GetHitSounds.PlaySound(param); 
    }
    public void PlayVoiceSound(float param)
    {
        if (CurrentCharacter.SoundOnAttack == null)
            Debug.LogError($"Character: {CurrentCharacter.CharacterName} doenst have SoundOnAttack!");
        else

            CurrentCharacter.SoundOnAttack.PlaySound(param); 
    }
    public void PlayComboSound()
    {
        if (CurrentCharacter.SoundOnAttack == null)
            Debug.LogError($"Character: {CurrentCharacter.CharacterName} doenst have ComboSounds!");
        else
            CurrentCharacter.ComboSounds.PlaySound();
    }
    public void PlayVictorySound()
    {
        if (CurrentCharacter.SoundOnAttack == null)
            Debug.LogError($"Character: {CurrentCharacter.CharacterName} doenst have VictorySound!");
        else
            CurrentCharacter.VictorySound.PlaySound(); 
    }

    public void PlayKOSound()
    {
        if (CurrentCharacter.SoundOnAttack == null)
            Debug.LogError($"Character: {CurrentCharacter.CharacterName} doenst have DeathSounds!");
        else
            CurrentCharacter.DeathSounds.PlaySound();
    }
    public void PlayTauntSound()
    {
        if (CurrentCharacter.SoundOnAttack == null)
            Debug.LogError($"Character: {CurrentCharacter.CharacterName} doenst have TauntSounds!");
        else
            CurrentCharacter.TauntSounds.PlaySound();
    }

    
}
