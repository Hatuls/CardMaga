using Sirenix.OdinInspector;
using UnityEngine;
using Random = System.Random;

public class DiscordButtonAnimtionHandler : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [SerializeField, MinMaxSlider(1,20,true)] private Vector2 _timeToDely;
    private float _timeToWait;
    private float _delay;
    
    
    private void FixedUpdate()
    {
        if (_delay > 0)
        {
            _delay -= Time.fixedDeltaTime;
        }
        else
        {
            _animator.SetTrigger("StartAnimation");
            RestTimer();
        }
    }

    private void RestTimer()
    {
        var random = new Random();

        _delay = random.Next(Mathf.RoundToInt(_timeToDely.x), Mathf.RoundToInt(_timeToDely.y));
    }
}
