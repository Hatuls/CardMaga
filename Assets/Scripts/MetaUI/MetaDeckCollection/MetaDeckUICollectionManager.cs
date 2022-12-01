using System;
using CardMaga.MetaData.AccoutData;
using CardMaga.UI;
using UnityEngine;

public class MetaDeckUICollectionManager : MonoBehaviour , IInitializable<MetaCharactersHandler>
{
    [SerializeField] private MetaCharecterUICollection[] _charectersUI;
    
    private MetaCharecterUICollection _mainCharecterUI;
    
    public void Init(MetaCharactersHandler data)
    {
        for (int i = 0; i < data.CharacterDatas.Length; i++)
        {
            _charectersUI[i].Init(data.CharacterDatas[i]);
        }
    }

    private void SetMainCharacterUI(MetaCharacterData metaCharacterData)
    {
        
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public void CheckValidation()
    {
        throw new NotImplementedException();
    }
}
