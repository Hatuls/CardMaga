
using Collections;
using Map;
using UnityEngine;
namespace Meta.Map
{

    public class LegendHandler : MonoBehaviour
    {
        [SerializeField] GameObject _legendPanel;
        //[SerializeField] Animator _legendAnimator;
        [SerializeField] GameObject _legendInfoContainer;
        [SerializeField] LegendRow[] _legendsRows;
        [SerializeField] EventPointCollectionSO _eventPointCollection;
        int _entranceAnimatioHash = Animator.StringToHash("Entrance");
        int _exitAnimationHash = Animator.StringToHash("Exit");

        private void Start()
        {
            _legendInfoContainer.SetActive(false);

            for (int i = 0; i < _legendsRows.Length; i++)
            {
                _legendsRows[i].Init(_eventPointCollection.EventPoints[i]);
            }
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
            //_legendAnimator.Play(_entranceAnimatioHash);
        }
        private void CloseLegend()
        {
            SetLegendIfoContainerState(false);
            //_legendAnimator.Play(_exitAnimationHash);
        }
        public void SetLegendIfoContainerState(bool state)
            => _legendInfoContainer.SetActive(state);
    }

}