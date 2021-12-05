

using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
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
                 Debug.LogError($"Not A Valid Event Name!!!!\nInputString:{eventName}");
            
        }
    }
    public void StopAllSounds()
    {
        if(_fmodLibrary == null)
        {
            throw new System.Exception("Audio Manager fmodLibrary is null");
        }
        if (_fmodLibrary.Count > 0)
        {
            foreach (var fmodData in _fmodLibrary)
            {
                fmodData.Value.StopSound(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

            }
            _fmodLibrary.Clear();
        }
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

        public void PlaySound()
        {
            _eventInstance.getDescription(out EventDescription desc);
            desc.getLength(out int d);
            _duration = d;
            _eventInstance.start();
        }
        public void StopSound(FMOD.Studio.STOP_MODE mode)
            => _eventInstance.stop(mode);

        ~FmodData()
            => _eventInstance.release();
    }
}


