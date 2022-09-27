﻿using UnityEngine;
using System;
using UnityEngine.UI;
using CardMaga.Card;

namespace CardMaga.UI.Visuals
{
    [System.Serializable]
    public class CardLevelVisualAssigner : BaseVisualAssigner<CardData>
    {
        [Header("SO")]
        [SerializeField] LevelCardVisualSO[] _levelsSO;

        [Header("BG")]
        [SerializeField] GameObject _levelsBGObject;
        [SerializeField] Image _levelsBG;
        [SerializeField] Image _levelsBGOutline;

        [Header("Levels")]
        [SerializeField] GameObject[] _visualLevels;
        [SerializeField] Image[] _common;
        [SerializeField] Image[] _uncommon;
        [SerializeField] Image[] _rare;
        [SerializeField] Image[] _epic;
        [SerializeField] Image[] _legendary;

        public override void CheckValidation()
        {
            //Check Levels SO
            foreach (var level in _levelsSO)
            {
                //LevelBGCardVisualSO
                if (level.LevelBGCardVisualSO.LevelsBG == null)
                {
                    throw new Exception($"LevelBGCardVisualSO {level.ToString()} has no Levels BG");
                }
                if (level.LevelBGCardVisualSO.LevelsBGOutline == null)
                {
                    throw new Exception($"LevelBGCardVisualSO {level.ToString()} has no Levels BG Outline");
                }
                //LevelCardVisulSO
                if (level.OuterLevel.Length == 0)
                    throw new Exception($"LevelCardVisualSO {level.ToString()} has no Outer Level");

                if (level.InnerLevel.Length == 0)
                    throw new Exception($"LevelCardVisualSO {level.ToString()} has no inner Level");

                if (level.EmptyOuterLevel.Length == 0)
                    throw new Exception($"LevelCardVisualSO {level.ToString()} has no Empty Outer Level");

                if (level.EmptyInnerLevel.Length == 0)
                    throw new Exception($"LevelCardVisualSO {level.ToString()} has no Empty Inner Level");
            }

            //Check BG Objects
            if (_levelsBGObject == null)
                throw new Exception("LevelsBGObject is Null");
            if (_levelsBG == null)
                throw new Exception("LevelsBG is Null");
            if (_levelsBGOutline == null)
                throw new Exception("LevelsBGOutline is Null");

            //Check Levels Objects
            if (_visualLevels.Length == 0)
                throw new Exception("VisualLevelsObject is Null");
            if (_common.Length == 0)
                throw new Exception("Common has no Parts");
            if (_uncommon.Length == 0)
                throw new Exception("Uncommon has no Parts");
            if (_rare.Length == 0)
                throw new Exception("Rare has no Parts");
            if (_epic.Length == 0)
                throw new Exception("Epic has no Parts");
            if (_legendary.Length == 0)
                throw new Exception("Legendary has no Parts");
        }
        public override void Init(CardData cardData)
        {
            //(int)card.CardSO.Rarity, cardLevel, 1
            int cardRarity = (int)cardData.CardSO.Rarity - 1;
            var cardLevel = cardData.CardLevel;
            //Hard Coded Value
            var levelType = 0;

            //setActive relevent Objects
            SetActiveObject(cardRarity);

            //SetBGObjects
            _levelsBG.AssignSprite(_levelsSO[cardLevel].LevelBGCardVisualSO.LevelsBG);
            _levelsBGOutline.AssignSprite(_levelsSO[cardLevel].LevelBGCardVisualSO.LevelsBGOutline);

            //SetLevelsSprite
            LevelsSwitch(cardRarity, cardLevel, levelType);
        }
        private void SetActiveObject(int cardRarity)
        {
            if (!_levelsBGObject.activeSelf)
            {
                _levelsBGObject.SetActive(true);
            }
            if (_visualLevels[cardRarity].activeSelf)
            {
                //Object is already Active
                return;
            }
            else
            {
                for (int i = 0; i < _visualLevels.Length; i++)
                {
                    if (i == cardRarity)
                    {
                        _visualLevels[i].SetActive(true);
                    }
                    else
                    {
                        _visualLevels[i].SetActive(false);
                    }
                }
            }
        }
        private void LevelsSwitch(int cardRarity, int cardLevel, int levelType)
        {
            switch (cardRarity)
            {
                //common
                case 0:
                    SetVisualLevel(cardLevel, _common, _levelsSO[cardRarity], levelType);
                    break;
                //uncommon
                case 1:
                    SetVisualLevel(cardLevel, _uncommon, _levelsSO[cardRarity], levelType);
                    break;
                //rare
                case 2:
                    SetVisualLevel(cardLevel, _rare, _levelsSO[cardRarity], levelType);
                    break;
                //epic
                case 3:
                    SetVisualLevel(cardLevel, _epic, _levelsSO[cardRarity], levelType);
                    break;
                //legendary
                case 4:
                    SetVisualLevel(cardLevel, _legendary, _levelsSO[cardRarity], levelType);
                    break;
                default:
                    throw new Exception("CardLevelVisualAssigner did not find the correct rarity");
            }
        }
        private void SetVisualLevel(int cardLevel, Image[] level, LevelCardVisualSO levelSO, int levelType)
        {
            var maxLevel = level.Length - 1;
            for (int i = 0; i < level.Length; i++)
            {
                //first
                if (i == 0)
                {
                    //first Level is always Active
                    level[i].AssignSprite(levelSO.OuterLevel[levelType]);
                    level[i].AssignColor(levelSO.FullColor);
                }

                //last
                else if (i == maxLevel)
                {
                    if (cardLevel >= i)
                    {
                        level[i].AssignSprite(levelSO.OuterLevel[levelType]);
                        level[i].AssignColor(levelSO.FullColor);
                    }
                    else
                    {
                        level[i].AssignSprite(levelSO.EmptyOuterLevel[levelType]);
                        level[i].AssignColor(levelSO.EmptyColor);
                    }
                }
                else
                {
                    //middle
                    if (cardLevel >= i)
                    {
                        level[i].AssignSprite(levelSO.InnerLevel[levelType]);
                        level[i].AssignColor(levelSO.FullColor);
                    }
                    else
                    {
                        level[i].AssignSprite(levelSO.EmptyInnerLevel[levelType]);
                        level[i].AssignColor(levelSO.EmptyColor);
                    }
                }
            }
        }
        public override void Dispose()
        {
        }
    }
}