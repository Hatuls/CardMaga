﻿using CardMaga.UI.Text;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CardMaga.UI.Visuals
{
    public class AccountBarVisualHandler : BaseAccountBarVisualHandler
    {
        [SerializeField] AccountBarVisualAssignerHandler _accountBarVisualAssignerHandler;
        [SerializeField] AccountBarTextAssignerHandler _accountBarTextAssignerHandler;

        [SerializeField] ResourceVisualHandler _chipsVisualHandler;
        [SerializeField] ResourceVisualHandler _goldVisualHandler;
        [SerializeField] ResourceVisualHandler _diamondsVisualHandler;

        public override BaseVisualAssignerHandler<AccountBarVisualData> AccountBarVisualAssignerHandler => _accountBarVisualAssignerHandler;
        public override BaseTextAssignerHandler<AccountBarVisualData> AccountBarTextAssignerHandler => _accountBarTextAssignerHandler;
        public override void CheckValidation()
        {
            base.CheckValidation();
            _chipsVisualHandler.CheckValidation();
            _goldVisualHandler.CheckValidation();
            _diamondsVisualHandler.CheckValidation();
        }
        public override void Init(AccountBarVisualData resources)
        {
            base.Init(resources);
            _chipsVisualHandler.Init(resources.ChipsData);
            _goldVisualHandler.Init(resources.GoldData);
            _diamondsVisualHandler.Init(resources.DiamondsData);
        }
        public override void Dispose()
        {
            base.Dispose();
            _chipsVisualHandler.Dispose();
            _goldVisualHandler.Dispose();
            _diamondsVisualHandler.Dispose();
        }
#if UNITY_EDITOR
        [Header("Test")]
        public AccountBarVisualData AccountBarVisualDataTest;
        [Button]
        public void TestAccountBar()
        {
            CheckValidation();
            Init(AccountBarVisualDataTest);
        }
#endif

    }
}