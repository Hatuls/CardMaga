using Battle.Characters;
using CardMaga.BattleConfigSO;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tutorial Config SO", menuName = "ScriptableObjects/Battle/Tutorial/New Tutorial Config SO")]
public class TutorialConfigSO : ScriptableObject
{
    [Header("Character configuration:")]
    [SerializeField,Tooltip("Determines whether to use this information for characters or information from the server")] 
    private bool _isOverrideCharacter;
    [SerializeField,Tooltip("Left player information")]
    private Character _leftCharacter;
    [SerializeField,Tooltip("right player information")] 
    private Character _rightCharacter;
    [Header("Battle configuration:")] [SerializeField]
    private BattleConfigSO _battleConfig;
    [Header("Tutorial:")]
    [SerializeField,Tooltip("Tutorial configuration:")] private BattleTutorial _battleTutorial;//done


    public bool IsOverrideCharacter => _isOverrideCharacter;
    public Character LeftCharacter => _leftCharacter;
    public Character RightCharacter => _rightCharacter;
    public BattleConfigSO BattleConfig => _battleConfig;
    public BattleTutorial BattleTutorial => _battleTutorial;
}
