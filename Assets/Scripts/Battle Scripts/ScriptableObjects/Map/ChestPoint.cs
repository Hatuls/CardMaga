﻿
using DesignPattern;
using Rewards.Battles;
using Sirenix.OdinInspector;
using System;
using UnityEngine;


namespace Map
{
    [CreateAssetMenu (fileName = "Chest", menuName = "ScriptableObjects/Map/Points/Chest")]
    public class ChestPoint : NodePointAbstSO , IObserver
    {
        public override NodeType PointType =>  NodeType.Chest;
        [SerializeField] ObserverSO _observerSO;

        [Button]
        public override void ActivatePoint()
        {
            _observerSO.Notify(this);
            var rewardBundle = Factory.GameFactory.Instance.RewardFactoryHandler.GetBattleRewards( Battles.CharacterTypeEnum.Elite_Enemy,Rewards.ActsEnum.ActOne);
            if (rewardBundle == null)
                throw new Exception("Reward Bundle is null!");

            BattleUIRewardHandler.Instance.OpenChestScreen(rewardBundle);
            MapPlayerTracker.Instance.view.SetAttainableNodes();
            MapView.Instance.ShowMap(MapManager.Instance.CurrentMap);
        }

        public void OnNotify(IObserver Myself)
        {
      
        }
    }
}