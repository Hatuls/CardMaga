using System.Linq;
using CardMaga;

using UnityEngine;
namespace CardMaga.Map
{


    [RequireComponent(typeof(LineRenderer))]
    public class MapLine : MonoBehaviour
    {
        [SerializeField] Material _nyanMat;
        [SerializeField] LineRenderer _lineRenderer;
        [SerializeField] Gradient _notAttainableColor;
        [SerializeField] Gradient _attainableColor;
        public void ConnectLines(Vector3 fromPoint, Vector3 toPoint)
        {
            if (_lineRenderer == null)
                _lineRenderer = GetComponent<LineRenderer>();

            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPositions(new Vector3[2] { fromPoint, toPoint });
        }

        public void SetColor(NodeStates nodeStates)
        {
            if (_lineRenderer == null)
                _lineRenderer = GetComponent<LineRenderer>();
            // Debug.Log(nodeStates);
            //var matList = _lineRenderer.materials.ToList();
            _lineRenderer.colorGradient = (nodeStates == NodeStates.Visited) ? _attainableColor : _notAttainableColor;
            _lineRenderer.startColor = (nodeStates == NodeStates.Visited) ? _attainableColor.colorKeys[0].color : _notAttainableColor.colorKeys[0].color;
            _lineRenderer.endColor = (nodeStates == NodeStates.Visited) ? _attainableColor.colorKeys[0].color : _notAttainableColor.colorKeys[0].color;


            //_lineRenderer.materials = matList.ToArray();
            _lineRenderer.UpdateGIMaterials();
        }

    }
}