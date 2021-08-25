﻿using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;

public class AudioManager : MonoSingleton<AudioManager>
{
    [SerializeField] GameObject _audioSourcePrefab;
    Dictionary<SoundsNameEnum, AudioConfigurationSO> AudioDictionary;
    [SerializeField] AudioSourceQueuePlayer _audioQueuePlayer;

    [SerializeField]
    List<AudioSourceAbstract> _audioSourceStackable;
    Queue<AudioConfigurationSO> _notStackableAudioQueue;
    public override void Init()
    {
        if (AudioDictionary == null || (AudioDictionary!= null && AudioDictionary.Count == 0))
         StartCoroutine(LoadSound());


        _notStackableAudioQueue = new Queue<AudioConfigurationSO>();
    }
    IEnumerator LoadSound()
    {
        AudioConfigurationSO[] audio = Resources.LoadAll<AudioConfigurationSO>("Audio");

        if (audio == null || audio.Length == 0)
            yield break;

        yield return null;

        AudioDictionary = new Dictionary<SoundsNameEnum, AudioConfigurationSO>();
        for (int i = 0; i < audio.Length; i++)
        {
            AudioDictionary.Add(audio[i].SoundsNameEnum, audio[i]);
            if (i % 3 == 0)
                yield return null;
        }

        Debug.Log("Audio Loaded Complete");
    }
   

    AudioSourceAbstract EnableAudioSource()
    {
        if (_audioSourceStackable.Count > 0)
        {
            for (int i = 0; i < _audioSourceStackable.Count; i++)
            {
                if (!_audioSourceStackable[i].gameObject.activeSelf)
                {
                    return _audioSourceStackable[i];
                }
            }
        }
        GameObject go = Instantiate(_audioSourcePrefab,transform);
        AudioSourceAbstract ash = go.GetComponent<AudioSourceAbstract>();
        _audioSourceStackable.Add(ash);
        return ash;
    }
    bool CheckIfPlaying(AudioClip clip)
    {
        for (int i = 0; i < _audioSourceStackable.Count; i++)
        {
            if (_audioSourceStackable[i].gameObject.activeSelf && _audioSourceStackable[i].GetAudioSource.clip == clip)
            {
                return true;
            }
        }
        return false;
    }

    public void PlayerAudioSource(SoundsNameEnum nameOfSound)
    {


        if (nameOfSound != SoundsNameEnum.None && AudioDictionary != null && AudioDictionary.Count > 0)
        {
            if (AudioDictionary.ContainsKey(nameOfSound))
                PlayAudioSource(AudioDictionary[nameOfSound]);
            else
                throw new Exception("Song Was Not Found In Resource Folder! - " + nameOfSound.ToString());
        }
    }
    // queue to add sounds 
   // play them if they are stackable

    private void PlayAudioSource(AudioConfigurationSO audio)
    {
        if (audio.IsStackable)
        EnableAudioSource().Init(audio);
        else
        AddToPlayQueue(audio);
    }
    public void ResetAudioCollection()
    {
        if (_audioSourceStackable != null && _audioSourceStackable.Count > 0)
        {
            for (int i = 0; i < _audioSourceStackable.Count; i++)
            {
                if(_audioSourceStackable[i] != null)
                {
                    _audioSourceStackable[i].OnEndPlayingSound();
                }
            }
        }
    }

    private void AddToPlayQueue (AudioConfigurationSO audio)
    {
        _notStackableAudioQueue.Enqueue(audio);

        if (_notStackableAudioQueue.Count == 1 && !_audioQueuePlayer.GetIsCurrentlyPlaying)
            PlayNext();

    }

    public void PlayNext()
    {
        if (_notStackableAudioQueue.Count > 0 && !_audioQueuePlayer.GetIsCurrentlyPlaying)
        {
            _audioQueuePlayer.Init(_notStackableAudioQueue.Dequeue());
        }
    }
}
