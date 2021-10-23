using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class AudioSourceAbstract : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;
    byte _finishCount;
    AudioConfigurationSO _audioSO;
    public AudioSource GetAudioSource => _audioSource;
    public bool GetIsCurrentlyPlaying => _audioSource.isPlaying;
    private void Start()
    {
        if (_audioSource == null)
            _audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (gameObject.activeSelf)
        {
            CheckAudioSource();
        }
    }
    public void Init(AudioConfigurationSO audioSO)
    {
        _audioSO = audioSO;
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
        if (!GetIsCurrentlyPlaying)
        {
            _finishCount++;
            if (_finishCount > 1)
            {
                OnEndPlayingSound();
            }
        }
        else
        {
            _finishCount = 0;
        }
    }
    public virtual void OnEndPlayingSound()
    {
        _audioSO = null;
        _audioSource.clip = null;
        gameObject.SetActive(false);
    }

}
