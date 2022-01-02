using DesignPattern;
using Rewards;
using System;
using System.Linq;
using UnityEngine;

namespace Map
{
    public class MapPlayerTracker : MonoBehaviour, IObserver
    {
        public bool lockAfterSelecting = false;
        // public float enterNodeDelay = 1f;
        public MapManager mapManager;
        public MapView view;

        public static MapPlayerTracker Instance;

        [SerializeField] ObserverSO _observerSO;
        [SerializeField]
        ActsEnum _currentAct = ActsEnum.ActOne;
        [SerializeField] CameraMovement _cameraMovement;

        [SerializeField]
        ActDifficultySO[] _actDiffucltys;
        ActDifficultySO this[ActsEnum _currentAct]
            => _actDiffucltys.FirstOrDefault(x => x.Act == _currentAct);
#if UNITY_EDITOR
        [Sirenix.OdinInspector.Button()]
        private void LoadActDiffucltys()
        => _actDiffucltys = Resources.LoadAll<ActDifficultySO>("Maps/Acts Diffuclty");

#endif

        public bool Locked { get; set; }
        public ActsEnum CurrentAct { get => _currentAct; set => _currentAct = value; }

        private void Awake()
        {
            Instance = this;
        }

        public void SelectNode(NodeMap mapNode)
        {
            if (Locked) return;

            // Debug.Log("Selected node: " + mapNode.Node.point);
            // _observerSO.Notify(this);
            if (mapManager.CurrentMap.path.Count == 0)
            {
                // player has not selected the node yet, he can select any of the nodes with y = 0
                if (mapNode.NodeData.point.y == 0)
                    SendPlayerToNode(mapNode);
                else
                    PlayWarningThatNodeCannotBeAccessed();
            }
            else
            {
                var currentPoint = mapManager.CurrentMap.path[mapManager.CurrentMap.path.Count - 1];
                var currentNode = mapManager.CurrentMap.GetNode(currentPoint);

                if (currentNode != null && currentNode.outgoing.Any(point => point.Equals(mapNode.NodeData.point)))
                    SendPlayerToNode(mapNode);
                else
                    PlayWarningThatNodeCannotBeAccessed();
            }
        }

        private void SendPlayerToNode(NodeMap mapNode)
        {
            Locked = lockAfterSelecting;
            mapManager.CurrentMap.path.Add(mapNode.NodeData.point);
            mapManager.SaveMap();

            //    view.SetLineColors();
            EnterNode(mapNode);
        }

        private void Start()
        {
            if (_actDiffucltys == null || _actDiffucltys.Length == 0)
                _actDiffucltys = Resources.LoadAll<ActDifficultySO>("Maps/Acts Diffuclty");

            Instance = this;
            view.SetAttainableNodes();
        }

        private static void EnterNode(NodeMap mapNode)
        {
            // we have access to blueprint name here as well
            Debug.Log("Entering node of type: " + mapNode.NodeData.NodeTypeEnum);
            // load appropriate scene with context based on nodeType:
            // or show appropriate GUI over the map: 
            // if you choose to show GUI in some of these cases, do not forget to set "Locked" in MapPlayerTracker back to false

            SendDataAnalytic(mapNode);

            switch (mapNode.NodeData.NodeTypeEnum)
            {

                case NodeType.Basic_Enemy:
                case NodeType.Boss_Enemy:
                case NodeType.Elite_Enemy:
                    var nodeData = mapNode.NodeData;
                    Factory.GameFactory.Instance.EventPointFactoryHandler.GetEventPoint(nodeData.NodeTypeEnum).ActivatePoint(Instance[Instance._currentAct][nodeData.point.y]);
                    break;


                case NodeType.Chest:
                case NodeType.QuestionMark:
                case NodeType.Rest_Area:
                case NodeType.Dojo:
                    Factory.GameFactory.Instance.EventPointFactoryHandler.GetEventPoint(mapNode.NodeData.NodeTypeEnum).ActivatePoint();
                    break;

                default:
                    Debug.LogError(mapNode.NodeData.NodeTypeEnum + "Is Not Valid Node Point!");
                    break;
            }

            Instance._cameraMovement.LastVisitedNodeY = mapNode.transform.position.y;

        }

        private static void SendDataAnalytic(NodeMap mapNode)
        {
            var configName = Instance.mapManager.CurrentMap.configName;
            var nodeType = mapNode.NodeData.NodeTypeEnum.ToString();
            var position = mapNode.NodeData.point.ToString();


            AnalyticsHandler.SendEvent("entering_node", new System.Collections.Generic.Dictionary<string, object> {
                {"map", configName },
                {"node" ,nodeType },
                {"location" ,position },
            });

            FireBaseHandler.SendEvent("entering_node",
                new Firebase.Analytics.Parameter("map", configName),
                new Firebase.Analytics.Parameter("node", nodeType),
                new Firebase.Analytics.Parameter("location", position)
                );
        }

        private void PlayWarningThatNodeCannotBeAccessed()
        {
            Debug.Log("Selected node cannot be accessed");
        }

        public void OnNotify(IObserver Myself)
        {
          //  throw new NotImplementedException();
        }
    }


    //[Serializable]
    //public class ActsData
    //{
    //  public  MapData this[ActsEnum a]
    //   => _acts.FirstOrDefault(x=> x.ActsEnum == a);

    //    [SerializeField]
    //    MapData[] _acts;

    //    public MapData[] MapData => _acts;
    //}
    //[Serializable]
    //public class MapData
    //{
    //    public ActsEnum ActsEnum;
    //    public Map Map;
    //}
}