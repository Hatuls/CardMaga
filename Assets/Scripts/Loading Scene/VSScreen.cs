using UnityEngine;
using TMPro;

public class VSScreen : MonoBehaviour
{
    [SerializeField]
    Animator _vsAnimator;
    [SerializeField]
    TextMeshProUGUI _playerNameText;
    [SerializeField]
    TextMeshProUGUI _opponentNameText;
    int _playVSHash = Animator.StringToHash("PlayVSAnim");
    private void Start()
    {
         StartVSScreen();
    }

    public void StartVSScreen()
    {
        //names
        if (Battles.BattleData.Player != null || Battles.BattleData.Opponent != null)
        {
            _playerNameText.text = Battles.BattleData.Player.CharacterData.Info.CharacterName;
            _opponentNameText.text = Battles.BattleData.Opponent.CharacterData.Info.CharacterName;
        }
        StartAnimation();
    }
    void StartAnimation()
    {
        _vsAnimator.SetTrigger(_playVSHash);
    }
}
