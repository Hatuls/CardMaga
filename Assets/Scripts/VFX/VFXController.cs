﻿using Keywords;
using System.Collections.Generic;
using UnityEngine;
using Unity.Events;

[RequireComponent(typeof(KeywordEnumListener))]
public class VFXController : MonoBehaviour
{
    [SerializeField] ParticleSystem _bleedPS;
    [SerializeField] ParticleSystem _healPS;
    [SerializeField] ParticleSystem _shieldPS;
    [SerializeField] ParticleSystem _StrengthPS;
    [SerializeField] ParticleSystem _attackPS;


    [SerializeField] bool _isPlayer;
    [SerializeField] BoolEvent _hitCharacterEvent;
    [SerializeField] BoolKeywordEnumEvent _activateKewordEvent;
    [SerializeField] SoundsEvent _soundsEvent;

    Dictionary<KeywordTypeEnum, ParticleSystem> _particleSystemDict;
    private void Start()
    {
        _particleSystemDict = new Dictionary<KeywordTypeEnum, ParticleSystem>()
        {
            { KeywordTypeEnum.Attack, _attackPS },
            {KeywordTypeEnum.Bleed, _bleedPS },
            {KeywordTypeEnum.Defense, _shieldPS },
            {KeywordTypeEnum.Strength, _StrengthPS },
            {KeywordTypeEnum.Heal, _healPS },
        };


        foreach (var item in _particleSystemDict)
        {
            item.Value.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
           
        }
    }

    public void ApplyHit()
    {



        _hitCharacterEvent?.Raise(_isPlayer);
        switch (Random.Range(0, 2))
        {
            case 0:
                _soundsEvent?.Raise(_isPlayer ? SoundsNameEnum.Attacking1 : SoundsNameEnum.EnemyAttack1);
                break;
            case 1:
                _soundsEvent?.Raise(_isPlayer ? SoundsNameEnum.Attacking2 : SoundsNameEnum.EnemyAttack2);
                break;
        }
    }
 
    public void PlayParticleEffect(KeywordTypeEnum keyword)
    {
        _activateKewordEvent?.Raise(_isPlayer, keyword);
    }
   public void PlayParticle(KeywordTypeEnum keywordType)
    {
        if (_particleSystemDict.TryGetValue(keywordType, out ParticleSystem ps))
        {
            if (ps.isPlaying)
               ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            
            ps.Play(true);
            _soundsEvent?.Raise(KeywordToSound(keywordType));
        }
    }
    private SoundsNameEnum KeywordToSound(KeywordTypeEnum keywordTypeEnum)
    {
        SoundsNameEnum soundsNameEnum;
        soundsNameEnum = SoundsNameEnum.Attacking1;
        switch (keywordTypeEnum)
        {

            case KeywordTypeEnum.Attack: // this is recieving dmg 

                switch ((int)Random.Range(0, 2))
                {
                    default:
                    case 0:
                        soundsNameEnum = (_isPlayer) ? SoundsNameEnum.CharacterGettingHit1 : SoundsNameEnum.EnemyGettingHit1;
                        break;
                    case 1:
                        soundsNameEnum = (_isPlayer) ? SoundsNameEnum.CharacterGettingHit2 : SoundsNameEnum.EnemyGettingHit2;
                        break;
                }
        
                break;
            case KeywordTypeEnum.Defense:
                soundsNameEnum = SoundsNameEnum.GainArmor;
                break;
            case KeywordTypeEnum.Heal:
                soundsNameEnum = SoundsNameEnum.Healing;
                break;
            case KeywordTypeEnum.Strength:
                soundsNameEnum = SoundsNameEnum.GainStrength;
                break;
            case KeywordTypeEnum.Bleed:
                soundsNameEnum = SoundsNameEnum.Bleeding;
                break;
            case KeywordTypeEnum.MaxHealth:
                soundsNameEnum = SoundsNameEnum.Healing;
                break;
            default:
                soundsNameEnum =  SoundsNameEnum.Attacking1;
                break;
        }
        return soundsNameEnum;
    }
}