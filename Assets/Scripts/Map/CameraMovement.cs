using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //when entering scene the floor of the current avilable locations should be around the bottom part of the screen
    // when pressing the left mouse button:
    //1. find the current location of the mouse
    //2. if the movement of the mouse is upwards{move down}
    //3. else if the movement of the mouse is downwards{move up}
    //can not move down or up more than the minimum or maximum of the script

    [SerializeField]
    float _minCamPosition;
    [SerializeField]
    float _maxCamPosition;
    [SerializeField]
    float _camSpeed;
    [SerializeField]
    Camera _camera;

    Vector3 _dragOrigin;



    private void Update()
    {
        MoveCam();
    }
    void MoveCam()
    {
        if(Input.GetMouseButtonDown(0))
        {
            _dragOrigin = _camera.ScreenToWorldPoint(Input.mousePosition);
        }
        if(Input.GetMouseButton(0))
        {
            Vector3 delta = _dragOrigin - _camera.ScreenToWorldPoint(Input.mousePosition);

            Debug.Log($"origin {_dragOrigin} newPos {_camera.ScreenToWorldPoint(Input.mousePosition)} = delta {delta}");

            Vector3 newPos = _camera.transform.position += new Vector3(0, delta.y, 0);
            
            _camera.transform.position = new Vector3 (_camera.transform.position.x, Mathf.Clamp(_camera.transform.position.y, _minCamPosition,_maxCamPosition),_camera.transform.position.z);
            
        }
    }
}
