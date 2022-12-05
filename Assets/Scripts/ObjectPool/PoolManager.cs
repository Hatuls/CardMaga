using CardMaga.MetaUI;
using CardMaga.Tools.Pools;
using CardMaga.UI.Card;
using CardMaga.UI.Combos;
using UnityEngine;

namespace CardMaga.ObjectPool
{
    public class PoolManager : MonoSingleton<PoolManager>
    {
        private IPoolMBObject<BattleCardUI> _battleCardUIPool;
        private IPoolMBObject<BattleComboUI> _battleComboUIPool;
        private IPoolMBObject<MetaCardUI> _metaCardUIPool;
        private IPoolMBObject<MetaComboUI> _metaComboUIPool;
        private IPoolMBObject<MetaCollectionUICard> _metaCollectionCardUIPool;
        private IPoolMBObject<MetaCollectionUICombo> _metaCollectionComboUIPool;

        public override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this);
        }

        public IPoolMBObject<T> GetPool<T>(T obj) where T : MonoBehaviour, IPoolableMB<T>, new()
        {
            switch (obj)
            {
                case BattleCardUI battleCardUI:
                    return _battleCardUIPool as IPoolMBObject<T>;
                case BattleComboUI battleComboUI:
                    return _battleComboUIPool as IPoolMBObject<T>;
                case MetaCardUI metaCardUI:
                    return _metaCardUIPool as IPoolMBObject<T>;
                case MetaComboUI metaComboUI:
                    return _metaComboUIPool as IPoolMBObject<T>;
                case MetaCollectionUICard metaCollectionCardUI:
                    return _metaCollectionCardUIPool as IPoolMBObject<T>;
                case MetaCollectionUICombo metaCollectionComboUI:
                    return _metaCollectionComboUIPool as IPoolMBObject<T>;
                default:
                    Debug.LogError("No Type pool found");
                    return null;
            }
        }
    }
}