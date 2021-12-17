using System;
using System.Threading.Tasks;
using UnityEngine;
namespace Account.GeneralData
{
    [Serializable]
    public class AccountSettingsData : ILoadFirstTime
    {
        [SerializeField]
        bool _vfxVolume;
        [SerializeField]
        bool _masterVolume;

        [SerializeField]
        bool _camShake;
        [SerializeField]
        SoundEventSO _backgroundMusic;
        public bool CamShake { get => _camShake; set => _camShake = value; }
        public bool MasterVolume { get => _masterVolume; set {
                _masterVolume = value;
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Music Volume", _masterVolume ? 0 : 1);
            }
        }
        public bool SFXEffect { get => _vfxVolume; set { 
                _vfxVolume = value;
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName("SFX Volume", _vfxVolume ? 0 : 1);
            } 
        }


     

 
        public async Task NewLoad()
        {
            SFXEffect = true;
            MasterVolume = true;
            CamShake = true;
        }
    }
}
