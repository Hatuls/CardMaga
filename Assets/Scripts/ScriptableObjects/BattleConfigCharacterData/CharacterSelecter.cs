using Battle.Turns;
using UnityEngine;

public abstract class CharacterSelecter : ScriptableObject
{
    protected abstract bool IsRandom { get;}
    protected abstract GameTurnType GameTurnType { get;}
    
    public GameTurnType GetTurnType()
    {
        if (IsRandom)
        {
            switch (Random.Range(0,2))
            {
                case 0:
                    return GameTurnType.LeftPlayerTurn;
                case 1:
                    return GameTurnType.RightPlayerTurn;
            }
        }
        else
        {
            return GameTurnType;
        }

        return GameTurnType.LeftPlayerTurn;
    }
}
