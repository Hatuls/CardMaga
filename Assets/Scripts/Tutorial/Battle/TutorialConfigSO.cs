using Battle.Characters;
using CardMaga.BattleConfigSO;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tutorial Config SO", menuName = "ScriptableObjects/Battle Config/Tutorial/New Tutorial Config SO")]
public class TutorialConfigSO : ScriptableObject
{
    [Header("Character configuration:")]
    [SerializeField,Tooltip("Left player information")]
    private BattleCharacter _leftCharacter;
    [SerializeField,Tooltip("right player information")] 
    private BattleCharacter _rightCharacter;
    [Header("Battle configuration:")] [SerializeField]
    private BattleConfigSO _battleConfig;
    [Header("Tutorial CoreID")]
    [SerializeField]
    private int _tutorialID;


    public BattleCharacter LeftCharacter => _leftCharacter;
    public BattleCharacter RightCharacter => _rightCharacter;
    public BattleConfigSO BattleConfig => _battleConfig;
    public int TutorialID => _tutorialID;
}
