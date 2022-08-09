
using Battle.Data;
using ReiTools.TokenMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VSScreenUI : MonoBehaviour
{

    private IDisposable _token;
    [SerializeField]
    private GameObject _background;
    [SerializeField]
    private GameObject _context;

    [SerializeField]
    private OpponentUI _bottom, _upper;

    [SerializeField]
    private float _screenDuration;
    private void Start()
    {
        ActivateVisuals(false);
    }
    public void AssignValues(ITokenReciever tokenReciever)
    {
            BattleData battleData = BattleData.Instance;
            _upper.AssignCharacterVisuals(battleData.Opponent);
            _bottom.AssignCharacterVisuals(battleData.Player);
            ActivateVisuals(true);
        StartCoroutine(ScreenDuration());
        _token = tokenReciever.GetToken();
        
    }

    private void ActivateVisuals(bool toActivate)
    {
        _context.SetActive(toActivate);
        _background.SetActive(toActivate);
    }


   private IEnumerator ScreenDuration()
    {
        yield return new WaitForSeconds(_screenDuration);
        _token.Dispose();
    }
}

[Serializable]
public class OpponentUI
{
    [SerializeField]
    private Image _portrait;
    [SerializeField]
    private TextMeshProUGUI _name;
  //  [SerializeField]

    //AddComboThingy


    public void AssignCharacterVisuals(Battle.Characters.Character player)
    {
        _name.text = player.DisplayName;
    }
}
