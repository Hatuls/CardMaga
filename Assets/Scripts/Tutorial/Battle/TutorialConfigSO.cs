using Battle.Characters;
using CardMaga.BattleConfigSO;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tutorial Config SO", menuName = "ScriptableObjects/Battle Config/Tutorial/New Tutorial Config SO")]
public class TutorialConfigSO : ScriptableObject
{
    [Header("Character configuration:")]
    [SerializeField,Tooltip("Left player information")]
    private Character _leftCharacter;
    [SerializeField,Tooltip("right player information")] 
    private Character _rightCharacter;
    [Header("Battle configuration:")] [SerializeField]
    private BattleConfigSO _battleConfig;
    
    
    public Character LeftCharacter => _leftCharacter;
    public Character RightCharacter => _rightCharacter;
    public BattleConfigSO BattleConfig => _battleConfig;
  
}
