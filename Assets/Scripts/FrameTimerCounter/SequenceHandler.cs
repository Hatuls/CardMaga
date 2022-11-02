using System.Collections;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using System.Linq;
#endif

namespace Temp
{
    public class SequenceHandler : MonoBehaviour
    {
        [SerializeReference]
        ISequenceHandler[] _sequanceList;
    
        Coroutine _coroutine;
        public void StopSequence()
        {
            if(_coroutine!= null)
              StopCoroutine(_coroutine);
        }
        public void StartSequance()
        {
            if(gameObject.activeSelf)
             _coroutine = StartCoroutine(StartSequanceEnumerator());
        }
    
        IEnumerator StartSequanceEnumerator()
        {
            for (int i = 0; i < _sequanceList.Length; i++)
            {
                if (_sequanceList[i].DelayBeforeExecute > 0)
                 yield return new WaitForSeconds(_sequanceList[i].DelayBeforeExecute);
                _sequanceList[i].Execute();
            }
        }
    #if UNITY_EDITOR
        [Sirenix.OdinInspector.Button]
        private void OrderSequence()
        {
            _sequanceList.OrderBy(x => x.Order);
        }
    #endif
    }
    
    
    [System.Serializable]
    public class ActivateEventAtSequance : ISequenceHandler
    {
    #if UNITY_EDITOR
        [Range(0,byte.MaxValue)]
        [SerializeField] 
        byte _order;
        public byte Order => _order;
    #endif
        [Range(0,byte.MaxValue)]
        [SerializeField]
        float _delayBeforeExecute;
        public float DelayBeforeExecute => _delayBeforeExecute;
    
        [SerializeField]
        UnityEvent _event;
        public void Execute()
        {
            _event?.Invoke();
        }
    }
    
    
    public interface ISequenceHandler
    {
    #if UNITY_EDITOR
        byte Order { get; }
    #endif
        float DelayBeforeExecute { get; }
        void Execute();
    }
}
