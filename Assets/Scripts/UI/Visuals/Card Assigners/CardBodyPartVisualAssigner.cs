using System;
using UnityEngine;
using UnityEngine.UI;

namespace CardMaga.UI.Visuals
{
    [Serializable]
    public class CardBodyPartVisualAssigner : BaseVisualAssigner
    {
        [SerializeField] BodyPartCardVisualSO _bodyPartCardVisualSO;

        [Tooltip("Attack - 0, Defense - 1, Utility - 2")]
        [SerializeField] GameObject[] _bodyPartActivationObject;

        [SerializeField] Image[] _bGImages;
        [SerializeField] Image[] _innerBGImages;
        [SerializeField] Image[] _bodyPartsImages;

        public override void Init()
        {
            //in class
            if (_bodyPartActivationObject.Length == 0)
                throw new Exception("CardBodyPartVisualAssigner has no activation object");

            if (_bGImages.Length == 0)
                throw new Exception("CardBodyPartVisualAssigner has no BGImages");

            if (_innerBGImages.Length == 0)
                throw new Exception("CardBodyPartVisualAssigner has no InnerBGImages");

            if (_bodyPartsImages.Length == 0)
                throw new Exception("CardBodyPartVisualAssigner has no BodyPartsImages");

            //in card SO
            if (_bodyPartCardVisualSO.BodyPartsBG.Length == 0)
                throw new Exception("CardBodyPartCardVisualSO has no BG Sprites");

            if (_bodyPartCardVisualSO.BodyPartsInnerBG.Length == 0)
                throw new Exception("CardBodyPartCardVisualSO has no InnerBG Sprites");

            //in base SO
            if (_bodyPartCardVisualSO.BaseSO.InnerBGColor.Length == 0)
                throw new Exception("CardBodyPartVisualAssigner has no BG Color");

            if (_bodyPartCardVisualSO.BaseSO.MainColor.Length == 0)
                throw new Exception("CardBodyPartVisualAssigner has no Main Color");

            if (_bodyPartCardVisualSO.BaseSO.BodyParts.Length == 0)
                throw new Exception("CardBodyPartVisualAssigner has no BodyParts");
        }
        public void SetBodyPart(int cardTypeNum, int bodyPartNum)
        {
            var cardType = cardTypeNum - 1;
            var bodyPart = bodyPartNum - 1;
            //Set Card Type object On
            SetActiveObject(cardType);

            //Set Card BG Sprites
            var sprite = GetSpriteToAssign(cardType, cardType, _bodyPartCardVisualSO.BodyPartsBG);
            AssignSprite(_bGImages[cardType], sprite);

            //Set Card Inner BG sprites and color
            sprite = GetSpriteToAssign(cardType, cardType, _bodyPartCardVisualSO.BodyPartsInnerBG);
            AssignSprite(_innerBGImages[cardType], sprite);
            var color = GetColorToAssign(cardType, _bodyPartCardVisualSO.BaseSO.InnerBGColor);
            AssignColor(_innerBGImages[cardType], color);

            //Set body part and color
            sprite = GetSpriteToAssign(cardType, bodyPart, _bodyPartCardVisualSO.BaseSO.BodyParts);
            AssignSprite(_bodyPartsImages[cardType], sprite);
            color = GetColorToAssign(cardType, bodyPart, _bodyPartCardVisualSO.BaseSO.MainColor);
            AssignColor(_bodyPartsImages[cardType], color);
        }
        private void SetActiveObject(int cardType)
        {

            if (_bodyPartActivationObject[cardType].activeSelf)
            {
                //gameobject is already active
                return;
            }
            else
            {
                for (int i = 0; i < _bodyPartActivationObject.Length; i++)
                {
                    if (i == cardType)
                    {
                        _bodyPartActivationObject[i].SetActive(true);
                    }
                    else
                    {
                        _bodyPartActivationObject[i].SetActive(false);
                    }
                }
            }
        }
    }
}
