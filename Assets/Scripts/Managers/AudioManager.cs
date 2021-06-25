using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;

public class AudioManager : MonoSingleton<AudioManager>
{
    [SerializeField] GameObject _audioSourcePrefab;
    Dictionary<SoundsNameEnum, AudioConfigurationSO> AudioDictionary;


    [SerializeField]
    List<AudioSourceHandler> _audioSourceContainer;

    public override void Init()
    {
        if (AudioDictionary == null || (AudioDictionary!= null && AudioDictionary.Count == 0))
         StartCoroutine(LoadSound());
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
            yield return null;
        }

        Debug.Log("Audio Loaded Complete");
    }
   

    AudioSourceHandler EnableAudioSource()
    {
        if (_audioSourceContainer.Count > 0)
        {
            for (int i = 0; i < _audioSourceContainer.Count; i++)
            {
                if (!_audioSourceContainer[i].gameObject.activeSelf)
                {
                    return _audioSourceContainer[i];
                }
            }
        }
        GameObject go = Instantiate(_audioSourcePrefab,transform);
        AudioSourceHandler ash = go.GetComponent<AudioSourceHandler>();
        _audioSourceContainer.Add(ash);
        return ash;
    }
    bool CheckIfPlaying(AudioClip clip)
    {
        for (int i = 0; i < _audioSourceContainer.Count; i++)
        {
            if (_audioSourceContainer[i].gameObject.activeSelf && _audioSourceContainer[i].GetAudioSource.clip == clip)
            {
                return true;
            }
        }
        return false;
    }

    public void PlayerAudioSource(SoundsNameEnum nameOfSound)
    {
        if (AudioDictionary != null && AudioDictionary.Count > 0)
        {
            if (AudioDictionary.ContainsKey(nameOfSound))
                PlayAudioSource(AudioDictionary[nameOfSound]);
            else
                throw new Exception("Song Was Not Found In Resource Folder!");
        }
    }

   

    private void PlayAudioSource(AudioConfigurationSO audio)
    {
        if (audio.IsStackable || !CheckIfPlaying(audio.Clip))
        {
            EnableAudioSource().Init(audio);
        }
    }
    public void ResetAudioCollection()
    {
        if (_audioSourceContainer != null && _audioSourceContainer.Count > 0)
        {
            for (int i = 0; i < _audioSourceContainer.Count; i++)
            {
                if(_audioSourceContainer[i] != null)
                {
                    _audioSourceContainer[i].ResetAudioHandler();
                }
            }
        }
    }
}
