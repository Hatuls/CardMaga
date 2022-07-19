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
    public Sprite GetSprite(CardMaga.Card.BodyPartEnum bodyPart)
    {
        switch (bodyPart)
        {
            default:
            case CardMaga.Card.BodyPartEnum.None:
            //    Debug.LogError("Body Part Is Not Set To return Sprite");
                return null;
            case CardMaga.Card.BodyPartEnum.Head:
                return _bodyPartIcons[0];
            case CardMaga.Card.BodyPartEnum.Elbow:
                return _bodyPartIcons[1];
            case CardMaga.Card.BodyPartEnum.Hand:
                return _bodyPartIcons[2];
            case CardMaga.Card.BodyPartEnum.Knee:
                return _bodyPartIcons[3];
            case CardMaga.Card.BodyPartEnum.Leg:
                return _bodyPartIcons[4];

        }
    }
  
}
