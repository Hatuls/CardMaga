using UnityEngine;
using Cards;
namespace Battles.UI.CardUIAttributes
{
    [System.Serializable]
    public class CardUILevelHandler
    {
        [SerializeField] CardUILevel[] _cardsLevels;

        public void SetLevels(Card card)
            => SetLevels(card.CardLevel, card.CardSO.Rarity);

        public void SetLevels(int level, RarityEnum rarity, bool toShowMissingLevels = false)
        {

            const byte CommonAndUnCommonMaxLevel = 3;
            const byte RareAndEpicMaxLevel = 5;
            const byte LegendaryMaxLevel = 7;

            byte amountOfLevelsToTurnOn = GetAmoungOfLevel(rarity, CommonAndUnCommonMaxLevel, RareAndEpicMaxLevel, LegendaryMaxLevel);
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

                if (currentActiveState == true)
                    currentLevel.SetState((i <= level) ? CardUILevelState.On : (toShowMissingLevels)? CardUILevelState.Missing: CardUILevelState.Off);
            }
        }
        private static byte GetAmoungOfLevel(RarityEnum rarity, byte CommonAndUnCommonMaxLevel, byte RareAndEpicMaxLevel, byte LegendaryMaxLevel)
        {
            byte amountOfLevelsToTurnOn;
            switch (rarity)
            {

                case RarityEnum.Common:
                case RarityEnum.Uncommon:
                    amountOfLevelsToTurnOn = CommonAndUnCommonMaxLevel;
                    break;
                case RarityEnum.Rare:
                case RarityEnum.Epic:
                    amountOfLevelsToTurnOn = RareAndEpicMaxLevel;
                    break;
                case RarityEnum.LegendREI:
                    amountOfLevelsToTurnOn = LegendaryMaxLevel;
                    break;
                case RarityEnum.None:
                default:
                    throw new System.Exception("Cannot activate level for card because its rarity level is not valid! " + rarity.ToString());
            }

            return amountOfLevelsToTurnOn;
        }
    }
}