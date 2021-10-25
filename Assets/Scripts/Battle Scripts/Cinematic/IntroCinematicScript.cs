using Unity.Events;
using UnityEngine;

public class IntroCinematicScript : MonoBehaviour
{
    [SerializeField] IntEvent _changeAngle;
    [SerializeField] GameObject _introCinematic;
    [SerializeField] GameObject _CameraMovement;


    public void EndOfCinematic() {
        _introCinematic.SetActive(false);
        _CameraMovement.SetActive(true);
        _changeAngle?.Raise(1); 
    }
}
