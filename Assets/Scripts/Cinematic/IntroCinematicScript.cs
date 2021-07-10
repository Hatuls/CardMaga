using Unity.Events;
using UnityEngine;

public class IntroCinematicScript : MonoBehaviour
{
    [SerializeField] IntEvent _changeAngle;
    [SerializeField] GameObject _introCinematic;


    public void EndOfCinematic() {
        _introCinematic.SetActive(false);
        _changeAngle?.Raise(1); 
    }
}
