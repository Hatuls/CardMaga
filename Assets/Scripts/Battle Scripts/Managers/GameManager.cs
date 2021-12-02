using Art;
using Battles;
using Battles.UI;
using Managers;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    ISingleton[] _singletons;
    //[SerializeField]
    //int _maxFPS = 30;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        //  Application.targetFrameRate = _maxFPS;
        ThreadsHandler.ThreadHandler.TickThread();

        if (Input.GetKeyDown(KeyCode.R))
        {
            Init();
        }
    }
    public override void Init()
    {

        const byte amount = 12;
        _singletons = new ISingleton[amount]
        {
            VFXManager.Instance,
            CardExecutionManager.Instance,
            BattleUiManager.Instance,
            CardManager.Instance,
            CameraController.Instance,
            PlayerManager.Instance,
            EnemyManager.Instance,
            Combo.ComboManager.Instance,
            Keywords.KeywordManager.Instance,
            Battles.Deck.DeckManager.Instance,
            CardUIManager.Instance,
            BattleManager.Instance,
        };

        StartCoroutine(InitScripts());
    }
    System.Collections.IEnumerator InitScripts()
    {
        int frameSeperator = 3;
        for (int i = 0; i < _singletons.Length; i++)
        {
            if (i % frameSeperator == 0)
                yield return null;
            _singletons[i]?.Init();
        }
    }
}
