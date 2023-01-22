using Account;
using Battle.Data;
using UnityEngine;
using CardMaga.MetaData.AccoutData;
using CardMaga.SequenceOperation;
using CardMaga.ValidatorSystem;
using MetaData;
using ReiTools.TokenMachine;
using UnityEngine.Events;

public class TempPlayButton : MonoBehaviour, ISequenceOperation<MetaDataManager>
{
    [SerializeField]
    private GameObject _battleDataPrefab;
    [SerializeField] private UnityEvent OnStartBattle;
    private MetaAccountData _metaAccount;
    public int Priority => 3;
    
    public void ExecuteTask(ITokenReciever tokenMachine, MetaDataManager data)
    {
        _metaAccount = data.MetaAccountData;
    }

    
    private void Awake()
    {
        MetaDataManager.Register(this);
    }

    private void Start()
    {
        if (BattleData.Instance != null)
            Destroy(BattleData.Instance.gameObject);
    }
    
    public void Play()
    {
        MetaCharacterData mainCharacterData = _metaAccount.CharacterDatas.MainCharacterData;
        
        if (!Validator.Valid(mainCharacterData,out var failedInfo,ValidationTag.MetaCharacterDataSystem))
        {
            //failed
            return;       
        }
        
        OnStartBattle?.Invoke();
        BattleData battleData = Instantiate(_battleDataPrefab).GetComponent<BattleData>();
        battleData.AssignUserCharacter(AccountManager.Instance.DisplayName,mainCharacterData.CharacterData);
    }
}
