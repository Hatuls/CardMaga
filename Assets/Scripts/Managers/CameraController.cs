using System;
using UnityEngine;

public class CameraController : MonoSingleton<CameraController>
{
   //[SerializeField] Camera _camera;
   //[SerializeField] Canvas _canvas;
   // [SerializeField] RectTransform _parentCanvas;
   // [SerializeField] LeanTweenType[] _cameraShakes;
    [SerializeField] GameObject IntroCinematic;
    [SerializeField] GameObject MoveCameraAngle;
    [SerializeField] GameObject ShakeAtPlayer;
    [SerializeField] GameObject ShakeAtMiddle;
    [SerializeField] GameObject ShakeAtEnemy;

    private void Start()
    {
        MoveCameraAngle.SetActive(false);
        ShakeAtEnemy.SetActive(false);
        ShakeAtMiddle.SetActive(false);
        ShakeAtPlayer.SetActive(false);

        IntroCinematic.SetActive(true);

        //wait till animation ends
        //IntroCinematic.SetActive(false);
        //MoveCameraAngle set to 1 (middle position)
    }
    public void MoveCameraAnglePos(int index)
    {
        //0 is player
        //1 is middle
        //2 is Enemy
    }
    public void Shake(int moveCameraAngleIndex)
    {
        //0 is shake at player
        //1 is shake at middle
        //2 is shake at Enemy
    }
    //Vector3 startPos;
    //public Camera GetCamera => _camera;
    public override void Init()
    {
        //_camera = Camera.main;
        //startPos = transform.position;
    }

    //public static void ShakeCamera()
    //{
    //    float rotation;
    //    do
    //    {
    //        rotation = UnityEngine.Random.Range(-4f, 4f);
    //    } while (Mathf.Abs(rotation) < 2.5f);


    //    LeanTween.rotateZ(Instance.gameObject,
    //        rotation,
    //        0.5f)
    //        .setEase(Instance.GetRandomCameraShakeCurve()).
    //        setOnComplete(() => LeanTween.rotateZ(Instance.gameObject, 0, 0.2f).setEase(Instance.GetRandomCameraShakeCurve()));
    //}

    //private LeanTweenType GetRandomCameraShakeCurve()
    //{
    //    if (_cameraShakes == null || _cameraShakes.Length == 0)
    //        return LeanTweenType.easeInSine;

    //    return _cameraShakes[UnityEngine.Random.Range(0, _cameraShakes.Length)];
    //}

    //void ZoomOnObject(in Transform target,in float howMuchZoom) {  }
}
