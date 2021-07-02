using UnityEngine;

[CreateAssetMenu(fileName ="Audio Clip", menuName = "ScriptableObjects/Audio/Create Audio Clip")]
public class AudioConfigurationSO : ScriptableObject
{
    [SerializeField] SoundsNameEnum _soundNameEnum;
    [SerializeField] AudioClip _clip;
    [Range(0,1)]
    [SerializeField] float _volume = 0.5f;
    [Range(-3,3)]
    [SerializeField] float _pitch = 1;
    [SerializeField] bool _loop;
    [SerializeField] bool _isStackable;
    public float Pitch => _pitch;
    public bool Loop => _loop;
    public AudioClip Clip => _clip;
    public float Volume => _volume;
    public bool IsStackable => _isStackable;
    public SoundsNameEnum SoundsNameEnum => _soundNameEnum;
}
public enum SoundsNameEnum
{
    MainMenuBackground = 0,
    CombatBackground = 1,
    CrowdAmbientSound = 2,
    CrowdCheering = 3,
    DrawCard = 4,
    DisacrdCard = 5,
    CharacterGettingHit1 = 6,
    CharacterGettingHit2 = 7,
    EnemyGettingHit1 = 8,
    EnemyGettingHit2 = 9,
    Attacking1 = 10,
    Attacking2 = 11,
    EnemyAttack1 = 12,
    EnemyAttack2 = 13,
    GainArmor = 14,
    Bleeding = 15,
    GainStrength = 16,
    Healing = 17,
    Victory = 18,
    Defeat = 19,
    SelectCard = 20,
    PlaceCard = 21,
    TapCard = 22,
    ButtonTapped = 23,
    EndTurn = 24,
    VS = 25,
    WomanBleeding = 26

}