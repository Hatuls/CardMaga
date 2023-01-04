using CardMaga.ObjectPool;
using UnityEngine;
namespace CardMaga.VFX
{

    [CreateAssetMenu(fileName = "New VFX SO", menuName = "ScriptableObjects/VFX/Battle/New VFX SO")]
    public class BattleVisualEffectSO : BasePoolSO<BaseVisualEffect>
    {
        [SerializeField]
        private BaseBattleVisualEffectPosition _baseBattleVisualEffectPosition;

        public BaseBattleVisualEffectPosition PositionLogic => _baseBattleVisualEffectPosition;
    }
}