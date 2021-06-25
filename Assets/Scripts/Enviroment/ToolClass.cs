using UnityEngine;

public class ToolClass {

    // rotate to look Toward 
    public static Quaternion RotateToLookTowards(Transform rotateFrom, Transform RotateTo)
    => Quaternion.LookRotation(GetDirection(RotateTo.position, rotateFrom.position));
 
    // get random direction
    public static Vector3 GetDirection()
    =>  new Vector3(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f)).normalized;
    // get direction from 2 vectors
    public static Vector3 GetDirection(Vector3 myPos, Vector3 targetPos)
     => (targetPos - myPos).normalized;
}