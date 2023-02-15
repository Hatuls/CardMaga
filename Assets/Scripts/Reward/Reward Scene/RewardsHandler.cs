﻿using CardMaga.Core;
using CardMaga.Rewards.Bundles;
using CardMaga.UI;
using ReiTools.TokenMachine;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CardMaga.Rewards
{

    public class RewardsHandler : MonoBehaviour
    {
        [SerializeField, EventsGroup]
        private UnityEvent OnRewardsScreenExit;
        [SerializeField]
        private SceneLoader _sceneLoader;
        [SerializeField]
        private BaseRewardsVisualHandler[] _baseRewardScreens;

        private OperationEnumerable _operationEnumerable;
        //private SequenceHandler _sequenceHandler;
        void Start()
        {
   
            IReadOnlyList<BaseRewardFactorySO> x = RewardManager.Instance.RewardsData.Rewards;
            AssignRewards(x);
            RegisterSequence();
            // _sequenceHandler.StartAll(ExitScene);
            TokenMachine rewardsTokenMachine = new TokenMachine(ExitScene);
            _operationEnumerable.Init(rewardsTokenMachine);
            _operationEnumerable.StartOperation();
        }


        private void RegisterSequence()
        {
            //   _sequenceHandler = new SequenceHandler();
            BaseRewardsVisualHandler baseRewardScreen;
            var operationBehaviours = new List<IOperationBehaviour>();
            for (int i = 0; i < _baseRewardScreens.Length; i++)
            {
                baseRewardScreen = _baseRewardScreens[i];

                if (baseRewardScreen.HasRewards)
                {
                    operationBehaviours.Add(baseRewardScreen);
                }
            }
            _operationEnumerable = new OperationEnumerable(operationBehaviours, 0);

        }

        private void AssignRewards(IReadOnlyList<BaseRewardFactorySO> rewards)
        {
            //Generating all rewards
            int length = rewards.Count;
            IRewardable[] rewardable = new IRewardable[length];
            for (int i = 0; i < length; i++)
                rewardable[i] = rewards[i].GenerateReward();

            GetAllRewards(rewardable);


            // Sort it and insert it to each screen
            void GetAllRewards(IRewardable[] rewardables)
            {
                for (int i = 0; i < rewardables.Length; i++)
                {
                    IRewardable current = rewardables[i];
                    RewardType rewardType = current.RewardType;

                    if (rewardType.Contain(RewardType.Gift) || rewardType.Contain(RewardType.Bundle))
                        GetAllRewards((current as GiftReward).Rewardables);
                    else
                        AddToScreen(current); // Add Also the SO 
                }
            }
        }
        //Adding the rewardable object to the right reward-type's screen
        private void AddToScreen(IRewardable reward)
        {
            for (int i = 0; i < _baseRewardScreens.Length; i++)
                if (_baseRewardScreens[i].RewardType == reward.RewardType)
                    _baseRewardScreens[i].AddRewards(reward);
        }





        private void ExitScene()
        {
            _operationEnumerable.OnCompleted -= ExitScene;
            RewardManager.Instance.RewardsData.ClearRewards();
            Account.AccountManager.Instance.SendAccountData();
            OnRewardsScreenExit?.Invoke();
            _sceneLoader.UnloadManualy();
        }

#if UNITY_EDITOR
        [Sirenix.OdinInspector.Button]
        public void ReOrderList(
            ) => _baseRewardScreens.Sort((x, y) => x.CompareTo(y));

        [Sirenix.OdinInspector.Button]
        public void PopulateList()
        {
            _baseRewardScreens = FindObjectsOfType<BaseRewardsVisualHandler>();
            ReOrderList();
        }
#endif
    }


    public abstract class BaseRewardsVisualHandler : BaseUIElement, IComparable<BaseRewardsVisualHandler>, IOperationBehaviour //ISequenceOperation,
    {
        private IDisposable _token;
        [SerializeField]
        private int _priority;
        public RewardType RewardType;
        protected List<IRewardable> _rewards = new List<IRewardable>();


        [SerializeField, EventsGroup]
        private UnityEvent OnShowEvent, OnHideEvent;

        public event Action OnCompleted;

        public bool HasRewards => (_rewards.Count > 0);


        public int Order =>_priority;

        protected virtual void AddRewards()
        {
            foreach (var item in _rewards)
                item.AddToDevicesData();
        }
        protected abstract void CalculateRewards();
        public void AddRewards(IRewardable rewards)
        {
            _rewards.Add(rewards);
        }

        public override void Show()
        {
            base.Show();
            OnShowEvent?.Invoke();
            CalculateRewards();
        }

        public override void Hide()
        {
            OnHideEvent?.Invoke();
            base.Hide();
            AddRewards();
            Completed();
        }

        public int CompareTo(BaseRewardsVisualHandler obj)
        {
            if (_priority < obj._priority)
                return -1;
            else if (_priority > obj._priority)
                return 1;
            return 0;
        }

        //public void ExecuteTask(ITokenReceiver tokenMachine)
        //{
        //    _token = tokenMachine.GetToken();

        //}

        private void OnDestroy()
        {
            _rewards.Clear();
            _rewards = null;
        }

        public void Init(ITokenReceiver tokenReciever)
        => _token = tokenReciever.GetToken();



        public void StartOperation()
        {
            Show();
        }

        public void Completed()
        {
            OnCompleted?.Invoke();
            _token.Dispose();
        }
    }
}