using System;
using UnityEngine;

namespace CardMaga.UI.Visuals
{
    [CreateAssetMenu(fileName = "Account Portrait Collection SO", menuName = "ScriptableObjects/UI/Visuals/Account Portrait Collection SO")]
    public class AccountPortraitCollectionVisualSO : BaseVisualSO
    {
        public AccountPortraitVisualSO[] AccountPortraitVisualSos;

        public override void CheckValidation()
        {
            if (AccountPortraitVisualSos == null || AccountPortraitVisualSos.Length == 0)
                throw new Exception("AccountPortraitCollectionVisualSO has no portrait sos in it");
        }

        public AccountPortraitVisualSO GetPortraitSO(int id)
        {
            foreach (var portraitSO in AccountPortraitVisualSos)
            {
                if (portraitSO.AccountPortraitID == id)
                    return portraitSO;
            }
            return null;
        }
    }
}