using CardMaga.Keywords;
using CardMaga.UI.Buff;
using UnityEngine;
using System;

namespace CardMaga.UI.Visuals
{
    [CreateAssetMenu(fileName = "Buff Visual SO", menuName = "ScriptableObjects/UI/Visuals/Buff Visual SO")]
    public class BuffVisualSO : BaseVisualSO
    {
        public BuffTypeEnum BuffType;
        public Sprite BuffIcon;
        public KeywordType KeywordType;
        public bool ToShowText;
        public bool IsShardText;
        [Tooltip("Only when is shard text is on")]
        public int MaxShards;
        public override void CheckValidation()
        {
            if (BuffIcon == null)
                throw new Exception("BuffVisualSO has no Buff Icon");
        }
    }
}