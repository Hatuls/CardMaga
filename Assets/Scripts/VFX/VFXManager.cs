
using CardMaga.Battle.UI;
using CardMaga.SequenceOperation;
using CardMaga.Tools.Pools;
using ReiTools.TokenMachine;
using System.Collections.Generic;
using UnityEngine;
namespace CardMaga.VFX
{


    public class VFXManager : MonoBehaviour, ISequenceOperation<IBattleUIManager>
    {
        [SerializeField]
        private VisualEffectSO[] _vfxSO;
      
        private VFXPool _vfxPool;


        public int Priority =>0;

        private void Awake()
        {
            const int STARTING_SIZE = 2;
            _vfxPool = new VFXPool(this.transform);
            for (int i = 0; i < _vfxSO.Length; i++)
                _vfxPool.PopulatePool(_vfxSO[i], STARTING_SIZE);
            
        }
        public void ExecuteTask(ITokenReciever tokenMachine, IBattleUIManager data)
        {
            // logic
            data.VisualCharactersManager.GetVisualCharacter(true).VfxController.Init(_vfxPool);
            data.VisualCharactersManager.GetVisualCharacter(false).VfxController.Init(_vfxPool);
        }

        private void OnDestroy()
        {
            _vfxPool.Dispose();
        }
    }



}