
using CardMaga.Battle.UI;
using CardMaga.Battle.Visual;
using CardMaga.Commands;
using CardMaga.Keywords;
using CardMaga.SequenceOperation;
using CardMaga.Tools.Pools;
using ReiTools.TokenMachine;
using System;
using System.Collections;
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
        private VFXPool _vfxPool;
        private VisualCharactersManager _visualCharactersManager;
        private VFXController _leftVFXController;
        private VFXController _rightVFXController;
        public int Priority => 0;

        public IEnumerable<BattleVisualEffectSO> VFXs
        {
            get
            {
                yield return _hittingVFXSO;
                yield return _defenseVFXSO;
                yield return _getHitVFXSO;
            }
        }
        private void Awake()
        {
            const int STARTING_SIZE = 2;
            _vfxPool = new VFXPool(this.transform);

            foreach (var so in VFXs)
                _vfxPool.PopulatePool(so, STARTING_SIZE);
        }
        public void ExecuteTask(ITokenReciever tokenMachine, IBattleUIManager data)
        {
            // logic
            _keywordManager = data.BattleDataManager.KeywordManager;
            _visualCharactersManager = data.VisualCharactersManager;
            _leftVFXController = _visualCharactersManager.GetVisualCharacter(true).VfxController;
            _rightVFXController = _visualCharactersManager.GetVisualCharacter(false).VfxController;

            RegisterEffects(_leftVFXController);
            RegisterEffects(_rightVFXController);

            void RegisterEffects(VFXController vfxController)
            {
                vfxController.OnDefenseVFX += PlayDefenseVFX;
                vfxController.OnHittingVFX += PlayHitVFX;
                vfxController.OnGetHitVFX += PlayGettingHitVFX;
  
            }
        }

        private void PlayKeywordVFX(KeywordType keywordType, IVisualPlayer player)
        {
            PlayVFX (_keywordManager.GetLogic(keywordType).KeywordSO.GetVFX(), player);
        }
        private void PlayGettingHitVFX(IVisualPlayer visualPlayer)
            => PlayVFX(_getHitVFXSO, visualPlayer);
        private void PlayHitVFX(IVisualPlayer visualPlayer)
            => PlayVFX(_hittingVFXSO, visualPlayer);
        private void PlayDefenseVFX(IVisualPlayer visualPlayer)
            => PlayVFX(_defenseVFXSO, visualPlayer);

        private void PlayVFX(BattleVisualEffectSO vfxSO, IVisualPlayer visualPlayer)
        {
            var effect = _vfxPool.Pull(vfxSO);
            _hittingVFXSO.PositionLogic.SetPosition(effect.transform, visualPlayer);
            effect.Play();
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
                vfxController.OnVisualStatChanged -= PlayKeywordVFX;
            }
        }
    }


    public class VisualEffectCommand : ISequenceCommand, IPoolable<VisualEffectCommand>
    {
        public event Action OnFinishExecute;
        public event Action<VisualEffectCommand> OnDisposed;

        private CommandType _commandType;

        public bool IsPlayer;
        public KeywordType KeywordType;
        public CommandType CommandType => _commandType;

        public void Dispose()
        {
            OnDisposed?.Invoke(this);
        }

        public void Execute()
        {
            throw new NotImplementedException();
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }
    }
}