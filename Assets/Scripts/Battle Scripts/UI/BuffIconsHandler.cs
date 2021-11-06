using Battles.UI;
using Keywords;
using UnityEngine;
using UnityEngine.Events;
public class RemoveUIIcon: UnityEvent<bool ,KeywordTypeEnum>{}
public class BuffIconsHandler : MonoBehaviour
{
    #region Fields
    [SerializeField]
    BuffIconCollectionSO _buffCollection;

    [SerializeField] BuffIcon armourIcon;
    [SerializeField]
    BuffIcon[] _buffSlots;
    [SerializeField] Art.ArtSO _artSO;

    [SerializeField] BuffIcon _enemyOpponentActionUI;

    [SerializeField] bool isPlayer;
    #endregion
    private void Start()
    {
        Init();
    }
    private void Init()//check for bugs
    {
        if(_buffSlots != null && _buffSlots.Length>0)
        {
            for (int i = 0; i < _buffSlots.Length; i++)
            {
                if(_buffSlots[i] != null && _buffSlots[i].gameObject.activeSelf)
                {
                    _buffSlots[i].ResetEnumType();
                }
            }
        }
        if (isPlayer)
            BattleUiManager.Instance.PlayerBuffHandler = this;
     else
            BattleUiManager.Instance.EnemyBuffHandler = this;
    }
    BuffIcon GetFreeSlot()
    {
        if (_buffSlots != null && _buffSlots.Length > 0)
        {
            for (int i = 0; i < _buffSlots.Length; i++)
            {
                if (_buffSlots[i] != null && !_buffSlots[i].gameObject.activeSelf)
                {
                    return _buffSlots[i];
                }
            }
        }
        Debug.LogError("Error in GetBuffIcon");
        return null;
    }
    public void SetBuffIcon(KeywordTypeEnum icon, int amount)
    {
  
        if(CheckForDuplicates(icon))
        {
            GetDuplicate(icon).SetAmount(amount);
            //set text
            return;
        }
        var buffSlot = GetFreeSlot();
        buffSlot.GetSetName = icon;
        buffSlot.InitIconData(_buffCollection.GetIconData(icon),amount , icon);
    }

    internal void UpdateArmour(int amount)
    {
        armourIcon.SetAmount(amount);
    }

    public void RemoveBuffIcon(KeywordTypeEnum icon)
    {
        if (_buffSlots != null && _buffSlots.Length > 0)
        {
            for (int i = 0; i < _buffSlots.Length; i++)
            {
                if (_buffSlots[i].GetSetName == icon)
                {
                    _buffSlots[i].ResetEnumType();
                    OrderArray();
                    return;
                }
            }
        }
    }
    private bool CheckForDuplicates(KeywordTypeEnum icon)
    {
        if (_buffSlots != null && _buffSlots.Length > 0)
        {
            for (int i = 0; i < _buffSlots.Length; i++)
            {
                if (_buffSlots[i].GetSetName == icon)
                {
                    return true;
                }
            }
        }
        return false;
    }
    void OrderArray()
    {
        for (int i = 0; i < _buffSlots.Length-1; i++)
        {
            if (_buffSlots[i].gameObject.activeSelf==false)
            {
                for (int j = i + 1; j < _buffSlots.Length; j++)
                {
                    if(_buffSlots[j].gameObject.activeSelf)
                    {

                       // _buffSlots[j].transform.SetParent(null);

                        _buffSlots[j].transform.SetAsFirstSibling();
                      //  _buffSlots[i].InitIconData(_buffCollection.GetIconData(_buffSlots[j].GetSetName.GetValueOrDefault()));
                      //  _buffSlots[j].ResetEnumType();
                        break;
                    }
                    if(j == _buffSlots.Length-1)
                    {
                        return;
                    }
                }
            }
        }
    }


    public void SetOpponentActionUI(Cards.Card enemyAction)
    {
        _enemyOpponentActionUI?.InitIconData(enemyAction);
    }

    BuffIcon GetDuplicate(KeywordTypeEnum icon)
    {
        if (_buffSlots != null && _buffSlots.Length > 0)
        {
            for (int i = 0; i < _buffSlots.Length; i++)
            {
                if (_buffSlots[i].GetSetName == icon)
                {
                    return _buffSlots[i];
                }
            }
        }
        Debug.LogError("Error in GetDuplicate");
        return null;
    }
    //accses to all icons and colors
    //Array of Icons
    //void(Enum)add
    //void(Enum)remove
}
