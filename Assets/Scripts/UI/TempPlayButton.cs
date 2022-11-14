using Battle.Data;
using UnityEngine;
using Account;

public class TempPlayButton : MonoBehaviour
{
    [SerializeField]
    private GameObject _battleDataPrefab;
    private AccountManager _account;
    
    private void Awake()
    {
        _account = AccountManager.Instance;
    }

    private void Start()
    {
        if (BattleData.Instance != null)
            Destroy(BattleData.Instance.gameObject);
    }
    
    public void Play()
    {
        BattleData battleData = Instantiate(_battleDataPrefab).GetComponent<BattleData>();
        Account.GeneralData.Character mainCharacter = _account.Data.CharactersData.GetMainCharacter();
        battleData.AssignUserCharacter(_account.Data.DisplayName,mainCharacter);
    }
}
