using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace CardMaga.UI.PopUp
{
    [CreateAssetMenu(fileName = "New PopUpSO", menuName = "ScriptableObjects/PopUp/New PopUp SO")]
    public class PopUpSO : ScriptableObject
    {
        [SerializeField] private BasePopUp _popUpPrefab;
        public BasePopUp PopUpPrefab => _popUpPrefab;
    }
}
