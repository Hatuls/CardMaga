﻿using CardMaga.SequenceOperation;
using ReiTools.TokenMachine;
using System;
using UnityEngine;

namespace CardMaga.Battle.UI
{
    [Serializable]
    public class BuffIconManager : ISequenceOperation<IBattleUIManager>
    {
        [SerializeField]
        private BuffIconsHandler _rightBuffIconHandler;
        [SerializeField]
        private BuffIconsHandler _leftBuffIconHandler;

        public int Priority => 0;

        public void ExecuteTask(ITokenReciever tokenMachine, IBattleUIManager data)
        {
            var leftCharacter = data.VisualCharactersManager.GetVisualCharacter(true);
            var rightCharacter = data.VisualCharactersManager.GetVisualCharacter(false);

            var visualStat = leftCharacter.VisualStats;
            _leftBuffIconHandler.Init(visualStat);
            visualStat = rightCharacter.VisualStats;
            _leftBuffIconHandler.Init(visualStat);
            // if need to be initalize
        }

        public BuffIconsHandler GetBuffIconsHandler(bool isLeft) => isLeft ? _leftBuffIconHandler : _rightBuffIconHandler;


#if UNITY_EDITOR
        public void AssignBuffsFields()
        {
            var allBuffs = MonoBehaviour.FindObjectsOfType<BuffIconsHandler>();

            for (int i = 0; i < allBuffs.Length; i++)
            {
                if (allBuffs[i].gameObject.name.Contains("Player"))
                    _leftBuffIconHandler = allBuffs[i];
                else if (allBuffs[i].gameObject.name.Contains("Enemy"))
                    _rightBuffIconHandler = allBuffs[i];
                else
                    throw new Exception("More than 2 BuffIconHandler found in scene!\nCheck this object -> " + allBuffs[i].gameObject.name);
            }
        }
#endif
    }
}