using Sirenix.OdinInspector;
using UnityEngine;
[CreateAssetMenu(fileName = "New Model SO", menuName = "ScriptableObjects/Art/Models/New Model SO")]
public class ModelSO : ScriptableObject
{

    [SerializeField,OnValueChanged("ReOrder")]
    private ModelSkin[] _characterSkins;
    public ModelSkin DefaultCharacter => GetCharacterSkin(0);
    public ModelSkin GetCharacterSkin(int id) => _characterSkins[id];

#if UNITY_EDITOR
        const string SKIN_VERSION = "Skin Version ";
        const string DEFAULT_SKIN = "Default Skin";
        const string DEFAULT_COLOR = "Default Color";
        const string COLOR_VERSION = "Tinted Version ";
    private void ReOrder()
    {
        if(_characterSkins!=null && _characterSkins.Length>0)
        for (int i = 0; i < _characterSkins.Length; i++)
        {
            _characterSkins[i].SkinDetails = (i == 0) ? DEFAULT_SKIN : SKIN_VERSION + i;
            for (int j = 0; j < _characterSkins[i].SkinAmount; j++)
                _characterSkins[i].GetSkin(j).SkinDetails = (j == 0) ? DEFAULT_COLOR : COLOR_VERSION + j;
            
        }
    }
#endif
}

[System.Serializable]
public class AvatarSkin
{
#if UNITY_EDITOR
    [ReadOnly]
    public string SkinDetails;
#endif
    [HorizontalGroup("View")]
    [PreviewField(75f)]
    [SerializeField]
    private Material _material;
    [HorizontalGroup("View")]
    [SerializeField]
    [PreviewField(75f)]
    private Sprite _portait;

    public Sprite Portrait => _portait;
    public Material Material => _material;
}
[System.Serializable]
public class ModelSkin
{
#if UNITY_EDITOR
    [Sirenix.OdinInspector.ReadOnly]
    public string SkinDetails;
#endif



    [Sirenix.OdinInspector.PreviewField(75f)]
    [SerializeField,Tooltip("The PREFAB of this model that will be instantiated in the scene")]
    private AvatarHandler _model;

    [SerializeField]
    private AvatarSkin[] _skinVersions;

    public AvatarHandler Model => _model;
    public AvatarSkin DefaultSkin => _skinVersions[0];

    public int SkinAmount => _skinVersions.Length;


    public AvatarSkin GetRandomSkin()
    {
        if (_skinVersions.Length == 1)
            return _skinVersions[0];
        else
            return GetSkin(UnityEngine.Random.Range(0, _skinVersions.Length));
    }

    public AvatarSkin GetRandomSkin(int exceptThisID)
    {
        int skinsAmount = _skinVersions.Length;
        if (skinsAmount == 1)
        {
            Debug.Log("Returning default skin because there is no other options");
            return DefaultSkin;
        }

        int randID = 0;
        do
        {
            randID = Random.Range(0, skinsAmount);
        } while (randID == exceptThisID);
        return GetSkin(randID);
    }
    public AvatarSkin GetSkin(int index)
        => _skinVersions[index];
}
[System.Serializable]
public class BattleVisualCharacter
{
    [SerializeField] private AvatarSkin _avatarSkin;
    [SerializeField] private AvatarHandler _avatarHandler;
    public void Init(ModelSkin characterSkin, AvatarSkin avatarSkin)
    {
        var data = characterSkin;
        _avatarSkin = avatarSkin;
        _avatarHandler = data.Model;
    }

    public AvatarHandler Model => _avatarHandler;
    public Sprite Portrait => _avatarSkin.Portrait;
    public Material Material => _avatarSkin.Material;
}