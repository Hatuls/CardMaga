using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    static List<FmodData> _fmodLibrary = new List<FmodData>();
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = MonoBehaviour.Instantiate(Resources.Load("Prefabs/Audio/AudioManager")) as GameObject;
                _instance = go.GetComponent<AudioManager>();
            }
            return _instance;
        }
    }

    [SerializeField]
    private string _sfxPrefName;
    [SerializeField]
    private StudioGlobalParameterTrigger _sfxGlobalParameterTrigger;

    [SerializeField]
    private string _musicPrefName;
    [SerializeField]
    private StudioGlobalParameterTrigger _musicGlobalParameterTrigger;

    [SerializeField]
    SoundEventSO _backgroundMusic;
    FmodData backgroundFmod;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            FmodInit();
            DontDestroyOnLoad(this.gameObject);
        }
        else if (_instance != this)
            Destroy(this.gameObject);



    }


    private void OnDisable()
    {

        StopAllSounds();

    }
    private void FmodInit()
    {

        StartPreviousSettings();
        PlayBackGround();
        _fmodLibrary.Clear();
    }

    private void StartPreviousSettings()
    {
        LoadValues(_sfxGlobalParameterTrigger, _sfxPrefName);
        LoadValues(_musicGlobalParameterTrigger, _musicPrefName);

        void LoadValues(StudioGlobalParameterTrigger studioGlobalParameterTrigger, string name)
        {
            float value = 0;
            if (PlayerPrefs.HasKey(name))
                value = PlayerPrefs.GetFloat(name);

            AssignValue(studioGlobalParameterTrigger, value);
        }
        void AssignValue(StudioGlobalParameterTrigger studioGlobalParameterTrigger, float val)
        {
            studioGlobalParameterTrigger.value = val;
            studioGlobalParameterTrigger.TriggerParameters();
        }
    }

    private void PlayBackGround()
    {
        string path = string.Concat(FmodEventString, _backgroundMusic.EventPathName);
        EventDescription eventDescription;
        RuntimeManager.StudioSystem.getEvent(path, out eventDescription);
        if (eventDescription.isValid())
        {
            EventInstance _eventInstance = RuntimeManager.CreateInstance(path);
            backgroundFmod = new FmodData(_backgroundMusic, _eventInstance, eventDescription);

            backgroundFmod.PlaySound();
        }
        else
        {
            Debug.LogError("Background Event Sound was not valid!\n" + path);
        }
    }

    public const string FmodEventString = "event:/";

    public void PlaySoundEvent(SoundEventWithParamsSO soundEvent, float param)
    {
        for (int i = 0; i < _fmodLibrary.Count; i++)
        {
            if (soundEvent == _fmodLibrary[i].SoundEvent)
            {
                if (!soundEvent.IsStackable)
                    _fmodLibrary[i].SetParameter(param);
                else
                    _fmodLibrary[i].PlaySound(param);
                return;
            }
        }

        string path = string.Concat(FmodEventString, soundEvent.EventPathName);
        EventDescription eventDescription;
        RuntimeManager.StudioSystem.getEvent(path, out eventDescription);
        if (eventDescription.isValid())
        {
            EventInstance _eventInstance = RuntimeManager.CreateInstance(path);
            var fmod = new FmodData(soundEvent, _eventInstance, eventDescription);
            _fmodLibrary.Add(fmod);
            fmod.PlaySound(param);
        }
        else
            Debug.LogError($"Not A Valid Event Name!!!!\nInputString:{soundEvent.EventPathName}");
    }
    public void PlaySoundEvent(SoundEventSO soundEvent)
    {
        for (int i = 0; i < _fmodLibrary.Count; i++)
        {
            if (soundEvent == _fmodLibrary[i].SoundEvent)
            {
                _fmodLibrary[i].PlaySound();
                return;
            }
        }

        string path = string.Concat(FmodEventString, soundEvent.EventPathName);
        EventDescription eventDescription;
        RuntimeManager.StudioSystem.getEvent(path, out eventDescription);
        if (eventDescription.isValid())
        {
            EventInstance _eventInstance = RuntimeManager.CreateInstance(path);
            var fmod = new FmodData(soundEvent, _eventInstance, eventDescription);
            _fmodLibrary.Add(fmod);
            fmod.PlaySound();
        }
        else
            Debug.LogWarning($"Not A Valid Sound Event Name!\nInputString: {soundEvent.EventPathName}");

    }
    public void StopAllSounds()
    {
        if (_fmodLibrary == null)
        {
            throw new System.Exception("Audio Manager fmodLibrary is null");
        }
        if (_fmodLibrary.Count > 0)
        {
            foreach (var fmodData in _fmodLibrary)
            {
                fmodData.StopSound(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                fmodData.Release();
            }

            _fmodLibrary.Clear();
        }
    }
    private void OnDestroy()
    {
        StopAllSounds();
    }
    //private void SceneParameter(SceneHandler.ScenesEnum scene)
    //{
    //    switch (scene)
    //    {
    //        case SceneHandler.ScenesEnum.NetworkScene:
    //        case SceneHandler.ScenesEnum.LoadingScene:
    //        case SceneHandler.ScenesEnum.MainMenuScene:
    //        case SceneHandler.ScenesEnum.MapScene:
    //            RuntimeManager.StudioSystem.setParameterByName("Scene Parameter", 0);
    //            break;
    //        case SceneHandler.ScenesEnum.GameBattleScene:
    //            BattleMusicParameter();
    //            break;
    //        default:
    //            break;
    //    }
    //}


    // Need To be Re-Done
    public void BattleMusicParameter()
    {
        RuntimeManager.StudioSystem.setParameterByName("Scene Parameter", 2);
        //switch (Account.AccountManager.Instance.BattleData.Opponent.CharacterData.CharacterSO.CharacterType)
        //{
        //    case Battles.CharacterTypeEnum.Elite_Enemy:
        //        RuntimeManager.StudioSystem.setParameterByName("Scene Parameter", 2);
        //        break;
        //    case Battles.CharacterTypeEnum.Boss_Enemy:
        //        RuntimeManager.StudioSystem.setParameterByName("Scene Parameter", 3);
        //        break;

        //    default:
        //    case Battles.CharacterTypeEnum.Tutorial:
        //    case Battles.CharacterTypeEnum.Basic_Enemy:
        //        RuntimeManager.StudioSystem.setParameterByName("Scene Parameter", 1);
        //        break;
        //}
    }
    public class FmodData
    {
        public EventInstance _eventInstance;
        private EventDescription _eventDescription;
        private PARAMETER_ID _parameterID;
        float _duration;
        public SoundEventSO SoundEvent;

        public EventDescription EventDescription { get => _eventDescription; set => _eventDescription = value; }

        public FmodData(SoundEventSO eventSound, EventInstance eventInstance, EventDescription eventDescription)
        {
            this._eventInstance = eventInstance;
            this._eventDescription = eventDescription;
            this.SoundEvent = eventSound;
            if (eventSound is SoundEventWithParamsSO s)
            {
                PARAMETER_DESCRIPTION _paramDescription;
                _eventDescription.getParameterDescriptionByName(s.ParameterName, out _paramDescription);
                _parameterID = _paramDescription.id;
            }
        }
        public void PlaySound()
        {
            _eventDescription.getLength(out int d);
            _duration = d;
            _eventInstance.start();
        }
        public void PlaySound(float val)
        {
            _eventDescription.getLength(out int d);
            _duration = d;
            SetParameter(val);
            _eventInstance.start();
        }
        public void SetParameter(float param)
        {
            _eventInstance.setParameterByID(_parameterID, param, ((SoundEventWithParamsSO)SoundEvent).IgnoreSeekReed);
        }
        public void StopSound(FMOD.Studio.STOP_MODE mode)
            => _eventInstance.stop(mode);

        public void Release() => _eventInstance.release();
        ~FmodData()
            => Release();
    }
}


