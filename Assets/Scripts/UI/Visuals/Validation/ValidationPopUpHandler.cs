using CardMaga.ValidatorSystem;
using TMPro;
using UnityEngine;
namespace CardMaga.UI.PopUp
{
    public class ValidationPopUpHandler : MonoBehaviour
    {
        private const string Title = "Error: ";


        [SerializeField]
        private TextMeshProUGUI _errorTitle;
        [SerializeField]
        private TextMeshProUGUI _errorContext;



        public void AssignVisuals(IValidFailedInfo valid)
        {
            _errorTitle.text = string.Concat(Title, valid.ID);
            _errorContext.text = string.Concat(valid.Message);
        }
    }
}