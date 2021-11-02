
using UnityEngine;
[RequireComponent(typeof(LineRenderer))]
public class MapLine : MonoBehaviour
{
    [SerializeField] LineRenderer _lineRenderer;
    [SerializeField] Color _clr;
    public void ConnectLines( Vector3 fromPoint,  Vector3 toPoint)
    {
        if (_lineRenderer == null)
            _lineRenderer = GetComponent<LineRenderer>();

        _lineRenderer.startColor = _clr;
        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPositions(new Vector3[2] { fromPoint, toPoint }); 
    }
}