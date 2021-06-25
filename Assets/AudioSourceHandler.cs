using UnityEngine;

public class AudioSourceHandler : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;
    byte _finishCount;
    public AudioSource GetAudioSource => _audioSource;
    private void Update()
    {
        if (gameObject.activeSelf)
        {
            CheckAudioSource();
        }
    }
    public void Init(AudioConfigurationSO audioSO)
    {
        gameObject.SetActive(true);
        _finishCount = 0;
        _audioSource.clip = audioSO.Clip;
        _audioSource.pitch = audioSO.Pitch;
        _audioSource.volume = audioSO.Volume;
        _audioSource.loop = audioSO.Loop;
        _audioSource.Play();
    }
    void CheckAudioSource()
    {
        if (!_audioSource.isPlaying)
        {
            _finishCount++;
            if (_finishCount > 1)
            {
                ResetAudioHandler();
            }
        }
        else
        {
            _finishCount = 0;
        }
    }
    public void ResetAudioHandler()
    {
        _audioSource.clip = null;
        gameObject.SetActive(false);
    }

}
