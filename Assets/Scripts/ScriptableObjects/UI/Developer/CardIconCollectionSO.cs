﻿using UnityEngine;

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
                Debug.LogError("Body Part Is Not Set To return Sprite");
                return null;
            case Cards.BodyPartEnum.Head:
                return _bodyPartIcons[0];
            case Cards.BodyPartEnum.Elbow:
                return _bodyPartIcons[1];
            case Cards.BodyPartEnum.Hand:
                return _bodyPartIcons[2];
            case Cards.BodyPartEnum.Knee:
                return _bodyPartIcons[3];
            case Cards.BodyPartEnum.Leg:
                return _bodyPartIcons[4];

        }
    }
  
}
