using CardMaga.Battle.Players;
using CardMaga.Keywords;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
namespace CardMaga.VFX
{

    [CreateAssetMenu(fileName = "New VFX SO", menuName = "ScriptableObjects/VFX/Battle/New VFX SO")]
    public class BattleVisualEffectSO : VisualEffectSO
    {
        [SerializeField]
        private BaseBattleVisualEffectPosition _baseBattleVisualEffectPosition;

        public BaseBattleVisualEffectPosition PositionLogic=> _baseBattleVisualEffectPosition; 
    }
    [CreateAssetMenu(fileName = "New VFX SO", menuName = "ScriptableObjects/VFX/New generic VFX SO")]
    public class VisualEffectSO : TagSO
    {
        [PreviewField(100f)]
        [SerializeField]
        private BaseVisualEffect _vfxPrefab;

        public BaseVisualEffect VFXPrefab => _vfxPrefab;
      
    }



}