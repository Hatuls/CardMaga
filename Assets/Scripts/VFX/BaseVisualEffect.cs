using CardMaga.Battle.Players;
using CardMaga.Tools.Pools;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardMaga.VFX
{

    public class BaseVisualEffect : MonoBehaviour, IPoolableMB<BaseVisualEffect>, ITaggable
    {
        public event Action<BaseVisualEffect> OnDisposed;
        public event Action OnInitializable;
        public event Action OnPlay;
        public event Action OnStop;
        [SerializeField]
        private BattleVisualEffectSO[] _vFXSO;


        public virtual bool IsActive { get; }
        public BattleVisualEffectSO[] VFXSO { get => _vFXSO; }

        public IReadOnlyList<TagSO> Tags => VFXSO;

        public virtual void Play() { OnPlay?.Invoke(); }
        public virtual void Stop() { OnStop?.Invoke(); }
        public virtual void Init() { OnInitializable?.Invoke(); }

        public void Dispose()
        {
            OnDisposed?.Invoke(this);
        }

        private void FixedUpdate()
        {
            if (gameObject.activeInHierarchy && !IsActive)
                Dispose();
        }
    }
}