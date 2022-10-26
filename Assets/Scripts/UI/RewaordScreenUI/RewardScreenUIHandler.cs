using UnityEngine;
using UnityEngine.Playables;

public class RewardScreenUIHandler : MonoBehaviour
{
    [Header("Playable Director")]
    [SerializeField] private PlayableDirector _baseCoin;
    [SerializeField] private PlayableDirector _additionalBaseCoin;
    [SerializeField] private PlayableDirector _firstWinCoin;
    [SerializeField] private PlayableDirector _additionalFirstWinBaseCoin;

    private void OnEnable()
    {
        GiveBaseReward(false);
    }

    public void GiveBaseReward(bool isFirstWin)
    {
        _baseCoin.Play();

        if (isFirstWin)
            _firstWinCoin.Play();
    }
    
    public void GiveAdditionalReward(bool isFirstWin)
    {
        _additionalBaseCoin.Play();

        if (isFirstWin)
            _additionalFirstWinBaseCoin.Play();
    }
}
