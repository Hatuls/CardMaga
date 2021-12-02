

using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour //MonoSingleton<AudioManager>
{
    static Dictionary<string, FmodData> _fmodLibrary = new Dictionary<string, FmodData>();
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {

            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            FmodInit();

        }
        else if (_instance != this)
            Destroy(this.gameObject);
        else
            DontDestroyOnLoad(this.gameObject);
    }

    private void FmodInit()
    {
        _fmodLibrary.Clear();
    }

    public const string FmodEventString = "event:/";


    public void PlaySoundEvent(string eventName)
    {
        if (_fmodLibrary.TryGetValue(eventName, out FmodData f))
        {
            f.PlaySound();
        }
        else
        {
            string path = string.Concat(FmodEventString, eventName);
            EventDescription eventDescription;
            RuntimeManager.StudioSystem.getEvent(path, out eventDescription);
            if (eventDescription.isValid())
                _fmodLibrary.Add(eventName, new FmodData(path));
            else
                   throw new System.Exception($"Not A Valid Event Name!!!!\nInputString:{eventName}");
            
        }
    }
    private void FixedUpdate()
    {
        foreach (var data in _fmodLibrary)
        {
            if (data.Value.CheckIfFinished(Time.fixedDeltaTime))
                _fmodLibrary.Remove(data.Key);
        }
    }
    public void StopAllSounds()
    {
        foreach (var fmodData in _fmodLibrary)
        {
            fmodData.Value.StopSound(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

            _fmodLibrary.Remove(fmodData.Key);
        }
        _fmodLibrary.Clear();
    }

    // Fmod with Params
    //FMOD.Studio.EventDescription paramEventDescription;
    //instance.getDescription(out paramEventDescription);
    //        FMOD.Studio.PARAMETER_DESCRIPTION paramNameDescription;
    //paramEventDescription.getParameterDescriptionByName(ParameterName, out paramNameDescription);
    //        paramID = paramNameDescription.id;

    public class FmodData
    {
        private EventInstance _eventInstance;
        float _duration;
        public FmodData(string eventPath)
        {
            _eventInstance = RuntimeManager.CreateInstance(eventPath);
            PlaySound();

        }
        public bool CheckIfFinished(float deltaTime)
        {
            _duration -= deltaTime;
            return _duration <= 0;
        }
        public void PlaySound()
        {
            _eventInstance.getDescription(out EventDescription desc);
            desc.getLength(out int d);
            _duration = d;
            _eventInstance.start();
        }
        public bool IsValid() => _eventInstance.isValid();

        public void StopSound(FMOD.Studio.STOP_MODE mode)
            => _eventInstance.stop(mode);

        ~FmodData()
            => _eventInstance.release();
    }
}


public class SoundEventSO :ScriptableObject
{
    [SerializeField] string eventName = "string.Empty";

#if UNITY_EDITOR
    [Sirenix.OdinInspector.ShowInInspector]
    private string ShowWholeEventName => string.Concat("event:/", eventName);
#endif
    public void PlaySound() => AudioManager.Instance.PlaySoundEvent(eventName);
}