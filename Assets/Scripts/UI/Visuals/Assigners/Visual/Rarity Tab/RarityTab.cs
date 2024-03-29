﻿using CardMaga.UI.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CardMaga.UI.Visuals
{

  
    public class RarityTab : MonoBehaviour,IInitializable<RarityTextData>
    {
        [SerializeField]
        private RarityTabVisualAssigner _visalAssigner;
        [SerializeField]
        private RarityTabTextAssigner _textAssigner;

        public IEnumerable<BaseVisualHandler<RarityTextData>> Assigners
        {
            get
            {
                yield return _visalAssigner;
                yield return _textAssigner;
            }
        }

        public void CheckValidation()
        {
            foreach (var item in Assigners)
                item.CheckValidation();
        }

        public void Dispose()
        {
            foreach (var item in Assigners)
                item.Dispose();
        }

        public void Init(RarityTextData comboData)
        {
            foreach (var item in Assigners)
                item.Init(comboData);
        }
    }

    [System.Serializable]
    public class RarityTabVisualAssigner : BaseVisualAssigner<RarityTextData>
    {
        [SerializeField]
        private RarityCardVisualSO _rarityCardVisualSO;
        [SerializeField]
        private Image _rarityImage;

        public override void CheckValidation()
        {
            if (_rarityImage == null)
                throw new System.Exception($"Image is not assigned:\nField: _rarityImage");
            else if (_rarityCardVisualSO == null)
                throw new System.Exception($"Rarity Visual SO is not assigned:\nField: _rarityCardVisualSO");

        }

        public override void Dispose()
        {

        }

        public override void Init(RarityTextData comboData)
        {
            Sprite rarityPicture = _rarityCardVisualSO.GetRarity(comboData.RarityType);
            _rarityImage.AssignSprite(rarityPicture);
        }
    }
}