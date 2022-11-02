using Battle.Turns;
using UnityEngine;

[CreateAssetMenu(fileName = "LeftCharacterStart", menuName = "ScriptableObjects/Battle Config/Character Starter/Left Character Start")]
public class LeftCharacterStart : CharacterSelecter
{
    protected override bool IsRandom { get => false; }
    protected override GameTurnType GameTurnType { get=> GameTurnType.LeftPlayerTurn; }
}
