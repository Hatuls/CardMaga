using Battle.Turns;
using UnityEngine;

[CreateAssetMenu(fileName = "RightCharacterStart", menuName = "ScriptableObjects/Battle Config/Character Starter/Right Character Start")]
public class RightCharacterStart : CharacterSelecter
{
    protected override bool IsRandom { get => false; }
    protected override GameTurnType GameTurnType { get => GameTurnType.RightPlayerTurn; }
}
