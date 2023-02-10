
using Battle.Turns;
using CardMaga.Battle;
using CardMaga.Battle.UI;
using CardMaga.Battle.Visual;
using CardMaga.Commands;
using CardMaga.Keywords;
using CardMaga.ObjectPool;
using CardMaga.SequenceOperation;
using CardMaga.Tools.Pools;
using ReiTools.TokenMachine;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace CardMaga.VFX
{


    public class VFXManager : MonoBehaviour, ISequenceOperation<IBattleUIManager>
    {
        [SerializeField]
        private BattleVisualEffectSO _hittingVFXSO;
        [SerializeField]
        private BattleVisualEffectSO _defenseVFXSO;
        [SerializeField]
        private BattleVisualEffectSO _getHitVFXSO;

        private KeywordManager _keywordManager;
        private PoolHandler<BattleVisualEffectSO,BaseVisualEffect> _vfxPool;
        private VisualCharactersManager _visualCharactersManager;
        private VFXController _leftVFXController;
        private VFXController _rightVFXController;
        public int Priority => 0;

        private IPoolObject<VFXData> _vfxDataPool;
        private Queue<VFXData> _vFXQueue;
        private ITokenReceiver _vfxTokenMachine;
        private IDisposable _turnToken;
        public IEnumerable<BattleVisualEffectSO> VFXs
        {
            get
            {
                yield return _hittingVFXSO;
                yield return _defenseVFXSO;
                yield return _getHitVFXSO;
            }
        }

        #region MonoBehaviour
        private void Awake()
        {
            const int STARTING_SIZE = 2;
            _vFXQueue = new Queue<VFXData>(STARTING_SIZE);
            _vfxPool = new PoolHandler<BattleVisualEffectSO, BaseVisualEffect>(this.transform);
            _vfxDataPool = new ObjectPool<VFXData>(STARTING_SIZE);
            _vfxTokenMachine = new TokenMachine(MoveNext);
            foreach (var so in VFXs)
                _vfxPool.PopulatePool(so, STARTING_SIZE);

        }
        private void OnDestroy()
        {
            _vfxPool.Dispose();
            UnRegisterEffects(_visualCharactersManager.GetVisualCharacter(true).VfxController);
            UnRegisterEffects(_visualCharactersManager.GetVisualCharacter(false).VfxController);

            void UnRegisterEffects(VFXController vfxController)
            {
                vfxController.OnDefenseVFX -= PlayDefenseVFX;
                vfxController.OnHittingVFX -= PlayHitVFX;
                vfxController.OnGetHitVFX -= PlayGettingHitVFX;
            }
        }
        #endregion

        public void ExecuteTask(ITokenReceiver tokenMachine, IBattleUIManager data)
        {
            // logic
            Battle.IBattleManager battleManager = data.BattleDataManager;
            battleManager.OnBattleManagerDestroyed += BeforeDataDestroyed;

            _keywordManager = battleManager.KeywordManager;
            _visualCharactersManager = data.VisualCharactersManager;
            _leftVFXController = _visualCharactersManager.GetVisualCharacter(true).VfxController;
            _rightVFXController = _visualCharactersManager.GetVisualCharacter(false).VfxController;

            RegisterEffects(_leftVFXController);
            RegisterEffects(_rightVFXController);

            var logics = _keywordManager.KeywordLogicDictionary;
            foreach (var logic in logics)
                logic.Value.OnApplyingKeywordVisualEffect += ApplyKeywordVFX;

            

            void RegisterEffects(VFXController vfxController)
            {
                vfxController.OnDefenseVFX += PlayDefenseVFX;
                vfxController.OnHittingVFX += PlayHitVFX;
                vfxController.OnGetHitVFX += PlayGettingHitVFX;
            }

        }

        private void BeforeDataDestroyed(IBattleManager obj)
        {
            obj.OnBattleManagerDestroyed -= BeforeDataDestroyed;
            var logics = _keywordManager.KeywordLogicDictionary;
            foreach (var logic in logics)
                logic.Value.OnApplyingKeywordVisualEffect -= ApplyKeywordVFX;
        }

        private void ApplyKeywordVFX(bool currentPlayer, KeywordType keywordType)
        {
            BattleVisualEffectSO vfx = _keywordManager.GetLogic(keywordType).KeywordSO.GetVFX();
            if (vfx != null && vfx.PullPrefab != null)
                AddToQueue(vfx, _visualCharactersManager.GetVisualCharacter(currentPlayer));
        }
        public void AddToQueue(BattleVisualEffectSO vfx, IVisualPlayer player)
        {
            if (!ContainVFX())
            {
                var data = _vfxDataPool.Pull();
                data.Init(vfx, player);
                _vFXQueue.Enqueue(data);

                if (_vFXQueue.Count == 1)
                    MoveNext();
            }

            bool ContainVFX()
            {
                foreach (VFXData vfxData in _vFXQueue)
                {
                    if (vfxData.VfxSO == vfx)
                        return true;
                }
                return false;
            }
        }


        private void MoveNext()
        {
            if (_vFXQueue.Count == 0)
            {
                _turnToken?.Dispose();
                return;
            }
            VFXData data = _vFXQueue.Dequeue();
            PlayVFX(data.VfxSO, data.VisualPlayer, _vfxTokenMachine);
        }

        private void PlayGettingHitVFX(IVisualPlayer visualPlayer)
            => PlayVFX(_getHitVFXSO, visualPlayer);
        private void PlayHitVFX(IVisualPlayer visualPlayer)
            => PlayVFX(_hittingVFXSO, visualPlayer);
        private void PlayDefenseVFX(IVisualPlayer visualPlayer)
            => PlayVFX(_defenseVFXSO, visualPlayer);

        private void PlayVFX(BattleVisualEffectSO vfxSO, IVisualPlayer visualPlayer)
        {
            BaseVisualEffect effect = InitVFX(vfxSO, visualPlayer);
            effect.Play();
        }

        private BaseVisualEffect InitVFX(BattleVisualEffectSO vfxSO, IVisualPlayer visualPlayer)
        {
            var effect = _vfxPool.Pull(vfxSO);
            vfxSO.PositionLogic.SetPosition(effect.transform, visualPlayer);

            effect.gameObject.SetActive(true);
            return effect;
        }

        private void PlayVFX(BattleVisualEffectSO vfxSO, IVisualPlayer visualPlayer, ITokenReceiver tokenReciever)
        {
            BaseVisualEffect effect = InitVFX(vfxSO, visualPlayer);
            effect.Play(tokenReciever);
        }
    }

    public class VFXData : IPoolable<VFXData>
    {
        public event Action<VFXData> OnDisposed;

        BattleVisualEffectSO _vfxSO;
        IVisualPlayer _visualPlayer;

        public BattleVisualEffectSO VfxSO { get => _vfxSO; private set => _vfxSO = value; }
        public IVisualPlayer VisualPlayer { get => _visualPlayer; private set => _visualPlayer = value; }

        public void Init(BattleVisualEffectSO vfxSO, IVisualPlayer visualPlayer)
        {
            VfxSO = vfxSO;
            VisualPlayer = visualPlayer;
        }
        public void Dispose()
        {
            OnDisposed.Invoke(this);
        }
    }
}