
using UnityEngine;
namespace Meta.Map
{

    public class LegendHandler : MonoBehaviour
    {
        [SerializeField] GameObject _legendPanel;
        [SerializeField] Animator _legendAnimator;
        [SerializeField] GameObject _legendInfoContainer;

        int _entranceAnimatioHash = Animator.StringToHash("Entrance");
        int _exitAnimationHash = Animator.StringToHash("Exit");

        private void Start()
        {
            _legendInfoContainer.SetActive(false);
        }


        public void OnLegendButtonPress()
        {
            if (_legendInfoContainer.activeSelf)
                CloseLegend();
            else
                OpenLegend();
        }
        private void OpenLegend()
        {
            SetLegendIfoContainerState(true);
            _legendAnimator.Play(_entranceAnimatioHash);
        }
        private void CloseLegend()
        {
            _legendAnimator.Play(_exitAnimationHash);
        }
        public void SetLegendIfoContainerState(bool state)
            => _legendInfoContainer.SetActive(state);
    }

}