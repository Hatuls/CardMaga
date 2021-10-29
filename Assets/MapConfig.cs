using Map;
using System.Collections.Generic;
using UnityEngine;
namespace UI.Map
{
    [CreateAssetMenu(fileName = "Map Config", menuName = "ScriptableObjects/Map/Map Config")]
    public class MapConfig : ScriptableObject
    {
        [SerializeField]
        NodePointAbstSO[] _nodesSO;


        [SerializeField] int _minNumOfPreBossNodes;
        [SerializeField] int _maxNumOfPreBossNodes;
        public int NumOfPreBossNodes => Random.Range(_minNumOfPreBossNodes, _maxNumOfPreBossNodes + 1);
        [Space]
        [SerializeField] int _minNumOfStartingNodes;
        [SerializeField] int _maxNumOfStartingNodes;
        public int NumOfStartingNodes => Random.Range(_minNumOfStartingNodes, _maxNumOfStartingNodes + 1);
        [Space]
       
        public NodeLayer[] _nodeLayers;

        public NodePointAbstSO GetNodePoint(NodeType node)
        {
            for (int i = 0; i < _nodesSO.Length; i++)
            {
                if (node == _nodesSO[i].PointType)
                    return _nodesSO[i];
            }

            throw new System.Exception($"MapConfig: Node Was not found {node}");
        }
    }


    [System.Serializable]
    public class NodeLayer
    {
        [Header("Node:")]
        [SerializeField] NodeType _mainlyNode;
        [Space]

        [Header("Positioning:")]
        [Range(.5f, 1.5f)]
        [SerializeField] float _minDistanceFromPreviousLayer;
        [Range(.5f,1.5f)]
        [SerializeField] float _maxDistanceFromPreviousLayer;
        [Space]

        [Range(0, 1)]
        [SerializeField] float _randomizeXMinPosition;
        [Range(0, 1)]
        [SerializeField] float _randomizeXMaxPosition;
        [Space]
        [Header("Node Randomization:")]
        [Tooltip("0 - mean it always be the mainlyNode")]
        [Range(0, 1)]
        [SerializeField] float _randomizeNode;

        [Space]
        [Range(1, 4)]
        [SerializeField] int _minAmountOfNodes = 1;
        [Range(1, 4)]
        [SerializeField] int _maxAmountOfNodes = 4;
        public int RandomAmountOfPoints => Random.Range(_minAmountOfNodes, _maxAmountOfNodes+1);
        public NodeType MainlyNode { get => _mainlyNode; }
        public float MinDistanceFromPreviousLayer { get => _minDistanceFromPreviousLayer; }
        public float MaxDistanceFromPreviousLayer { get => _maxDistanceFromPreviousLayer; }
        public float RandomizeNode { get => _randomizeNode; }
        public float RandomizeXPosition { get => Random.Range(-_randomizeXMinPosition,_randomizeXMaxPosition); }
    }
}