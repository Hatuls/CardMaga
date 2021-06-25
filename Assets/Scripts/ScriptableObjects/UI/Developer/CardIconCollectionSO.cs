using UnityEngine;

[CreateAssetMenu(fileName = "CardIconCollectionOS", menuName = "ScriptableObjects/UI/CardIconCollection")]
public class CardIconCollectionSO : ScriptableObject
{
    #region Fields
    [SerializeField]
    [Tooltip("0 UpperPart\n 1 MidPart\n 2 LowPart")]
    Sprite[] _targetedBodyPartIcons;
    [SerializeField]
    [Tooltip("0 Head\n 1 Elbow\n 2 Fist\n 3 Knee\n 4 Feet\n 5 Duck\n 6 Jump")]
    Sprite[] _bodyPartIcons;
    #endregion
    public Sprite GetSprite(Cards.BodyPartEnum bodyPart)
    {
        switch (bodyPart)
        {
            default:
            case Cards.BodyPartEnum.None:
                return null;
            case Cards.BodyPartEnum.Head:
                return _bodyPartIcons[0];
            case Cards.BodyPartEnum.Elbow:
                return _bodyPartIcons[1];
            case Cards.BodyPartEnum.Fist:
                return _bodyPartIcons[2];
            case Cards.BodyPartEnum.Knee:
                return _bodyPartIcons[3];
            case Cards.BodyPartEnum.Feet:
                return _bodyPartIcons[4];
            case Cards.BodyPartEnum.Duck:
                return _bodyPartIcons[5];
            case Cards.BodyPartEnum.Jump:
                return _bodyPartIcons[6];
        }
    }
    public Sprite GetSprite(Cards.TargetedPartEnum targetedBodyPart)
    {
        switch (targetedBodyPart)
        {
            default:
            case Cards.TargetedPartEnum.None:
                return null;
            case Cards.TargetedPartEnum.UpperPart:
                return _targetedBodyPartIcons[0];
            case Cards.TargetedPartEnum.MidPart:
                return _targetedBodyPartIcons[1];
            case Cards.TargetedPartEnum.LowerPart:
                return _targetedBodyPartIcons[2];
        }
    }
}
