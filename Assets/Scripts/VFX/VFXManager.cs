using Keywords;
using Unity.Events;
using UnityEngine;
[RequireComponent(typeof(BoolListener))]
public class VFXManager : MonoSingleton<VFXManager>
{
    [SerializeField] KeywordEnumEvent _playerVFXEvent;
    [SerializeField]KeywordEnumEvent _enemyVFXEvent;

    [SerializeField]VFXController _playerVFX, _enemyVFX;

    public void PlayParticleEffect(bool isPlayer, KeywordTypeEnum keywordTypeEnum) 
    {
        (isPlayer ? _playerVFXEvent : _enemyVFXEvent)?.Raise(keywordTypeEnum);    
    }




    public void HitOtherCharacter(bool isPlayer)
    {
        (isPlayer ? _enemyVFXEvent: _playerVFXEvent)?.Raise(KeywordTypeEnum.Attack);
    }
  
    public override void Init()
    {
       
    }
}
