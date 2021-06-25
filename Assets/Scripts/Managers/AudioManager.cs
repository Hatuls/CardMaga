using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoSingleton<AudioManager>
{
    [SerializeField] GameObject _audioSourcePrefab;
    public override void Init()
    {
    }
    [SerializeField]
    List<AudioSourceHandler> _audioSourceContainer;
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
    public void PlayAudioSource(AudioConfigurationSO audio)
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
