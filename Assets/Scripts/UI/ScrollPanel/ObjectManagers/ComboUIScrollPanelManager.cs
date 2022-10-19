using Battle;
using Battle.Combo;
using CardMaga.SequenceOperation;
using ReiTools.TokenMachine;
using UnityEngine;

public class ComboUIScrollPanelManager : BaseScrollPanelManager<ComboUI,Combo> , ISequenceOperation<IBattleManager>
{
    [SerializeField] private ComboUIPool _comboUIPool;

    private Combo[] _combos;
    public override BasePoolObject<ComboUI, Combo> ObjectPool
    {
        get => _comboUIPool;
    }

    #region Test

    [Header("Testing")] [SerializeField] private Combo[] _comboData;
    
    [ContextMenu("TestAddComboToPanel")]
    public void TestAddComboToPanel()
    {
        AddObjectToPanel(_comboData);
    }

    #endregion

    private void Awake()
    {
        BattleManager.Register(this,OrderType.After);
    }

    private void OnEnable()
    {
        AddObjectToPanel(_combos);
    }

    private void OnDisable()
    {
        RemoveAllObjectsFromPanel();
    }

    public void ExecuteTask(ITokenReciever tokenMachine, IBattleManager data)
    {
        _combos = data.PlayersManager.LeftCharacter.Combos;
    }

    public int Priority { get; }
}
