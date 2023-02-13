using CardMaga.UI;
using System;
using System.Text;
using TMPro;
using UnityEngine;

namespace CardMaga.Meta.Upgrade
{
    public class UpgradeConfirmationScreen : MonoBehaviour
    {
        private const string CONTEXT_TEXT = "Are you sure you want to spend\n\n";
        private const string AND_TEXT = " and   ";
        public event Action OnConfirm;
        public event Action OnCancel;


        [SerializeField]
        private UpgradeUIManager _upgradeUIManager;

        [SerializeField]
        private GameObject _gameObject;
        [SerializeField]
        private TextMeshProUGUI _textMeshProGUI;

        private StringBuilder _stringBuilder = new StringBuilder();
        private void Awake()
        {
            Close();
            _upgradeUIManager.OnCostAssigned += SetText;
        }
        private void OnDestroy()
        {
            _upgradeUIManager.OnCostAssigned -= SetText;
            _stringBuilder.Clear();
            _stringBuilder = null;
        }
        private void SetText(int chipAmount, int goldAmount)
        => _textMeshProGUI.text = string.Concat(CONTEXT_TEXT, AssignText(chipAmount, 1), AND_TEXT, AssignText(goldAmount, 0));


        private string AssignText(int amount, int spriteIndex)
        {
            _stringBuilder.Clear();
            string stringText = amount.ToString().ToBold().AddImageInFrontOfText(spriteIndex);
            _stringBuilder.Append(stringText);

            return _stringBuilder.ToString();
        }


        public void Open() => _gameObject.SetActive(true);
        public void Close() => _gameObject.SetActive(false);
    }
}