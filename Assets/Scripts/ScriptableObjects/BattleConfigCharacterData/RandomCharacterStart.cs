using Battle.Turns;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomCharacterStart", menuName = "ScriptableObjects/Battle Config/Character Starter/Random Character Start")]
public class RandomCharacterStart : CharacterSelecter
{
    protected override bool IsRandom { get => true; }
    protected override GameTurnType GameTurnType { get; }
}
