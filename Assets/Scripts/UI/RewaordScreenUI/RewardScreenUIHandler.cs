using System;
using CardMaga.CinematicSystem;
using UnityEngine;

public class RewardScreenUIHandler : MonoBehaviour
{
    public event Action OnRewardEnded;
    
    [SerializeField] private CinematicManager _baseCinematicManager;
    [SerializeField] private CinematicManager _AdditionalCinematicManager;
    [SerializeField] private ClickHelper _clickHelper;

    private void Start()
    {
        _baseCinematicManager.StartCinematicSequence();
    }

    public void OpenClickHelper()
    {
        _clickHelper.Open(EndReward);
    }

    private void EndReward()
    {
        OnRewardEnded?.Invoke();
    }

    public void CheckFirstWin(CinematicManager cinematicManager)
    {
        if (true)
            cinematicManager.ResumeCinematicSequence();
    }
}
