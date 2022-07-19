using UnityEngine;
using CardMaga.Card;

namespace Battle.UI.CardUIAttributes
{
    [System.Serializable]
    public class CardUILevelHandler
    {
        [SerializeField] CardUILevel[] _cardsLevels;

        public void SetLevels(CardData card)
            => SetLevels(card.CardLevel, card.CardSO.Rarity);

        public void SetLevels(int level, RarityEnum rarity, bool toShowMissingLevels = false)
        {

           

            byte amountOfLevelsToTurnOn = GetAmoungOfLevel(rarity);
            byte totalLevelLength = (byte)_cardsLevels.Length;

            for (byte i = 0; i < totalLevelLength; i++)
            {

                var currentLevel = _cardsLevels[i];
                bool currentActiveState = currentLevel.gameObject.activeSelf;

                if (i < amountOfLevelsToTurnOn)
                {
                    if (currentActiveState == false)
                        currentLevel.SetActiveState(true);
                }
                else
                {
                    if (currentActiveState == true)
                        currentLevel.SetActiveState(false);
                }
        
                if (currentLevel.gameObject.activeSelf == true)
                    currentLevel.SetState((i <= level) ? CardUILevelState.On : (toShowMissingLevels)? CardUILevelState.Missing: CardUILevelState.Off);
            }
        }
        private static byte GetAmoungOfLevel(RarityEnum rarity)
        {
            const byte CommonMaxLevel = 2;
            const byte UnCommonMaxLevel = 3;
            const byte RareMaxLevel = 4;
            const byte EpicMaxLevel = 5;
            const byte LegendaryMaxLevel = 6;
         
            switch (rarity)
            {

                case RarityEnum.Common:
                    return CommonMaxLevel;

                case RarityEnum.Uncommon:
                    return UnCommonMaxLevel;
                
                case RarityEnum.Rare:
                    return RareMaxLevel;

                case RarityEnum.Epic:
                    return EpicMaxLevel;
                    
                case RarityEnum.LegendREI:
                    return LegendaryMaxLevel;
                    
                case RarityEnum.None:
                default:
                    throw new System.Exception("Cannot activate level for card because its rarity level is not valid! " + rarity.ToString());
            }

          ;
        }
    }
}