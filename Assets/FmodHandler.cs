using UnityEngine;


namespace Game.Fmod
{

    public class FmodHandler : MonoBehaviour
    {
        private FMOD.Studio.EventInstance instance;
       [FMODUnity.EventRef]
        public string fmodEvent = "event:/";
        [SerializeField]
        FmodParams[] _fmodParams;

        private void Start()
        {
            instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
            for (int i = 0; i < _fmodParams.Length; i++)
            {
                _fmodParams[i].Init(ref instance);
            }
            instance.start();
        }

        public void InvokeEventEmitter(float value)
        {
            instance.setParameterByID(_fmodParams[0].paramID, value);
        }
 
        public void InvokeEventEmitter(float[] floats)
        {
            for (int i = 0; i < floats.Length; i++)
            {
                instance.setParameterByID(_fmodParams[i].paramID, floats[i]) ;
            }
        }
    }
    [System.Serializable]
   public class FmodParams
    {

        public FMOD.Studio.PARAMETER_ID paramID;
        public string ParameterName = "None";
        public void Init(ref FMOD.Studio.EventInstance instance)
        {
            FMOD.Studio.EventDescription paramEventDescription;
            instance.getDescription(out paramEventDescription);
            FMOD.Studio.PARAMETER_DESCRIPTION paramNameDescription;
            paramEventDescription.getParameterDescriptionByName(ParameterName, out paramNameDescription);
            paramID = paramNameDescription.id;
        }
    }
}