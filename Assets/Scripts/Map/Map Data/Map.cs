
using System.Collections.Generic;
using System.Linq;

namespace Map
{
    [System.Serializable]
    public class Map : ISaveable
    {
        public List<Node> nodes; // template of the map (the whole map nodes)

        public List<Point> path; // players path

        public string configName;

        public SaveManager.FileStreamType FileStreamType => SaveManager.FileStreamType.PlayerPref;



        public Map(string configName,List<Node> nodes, List<Point> path)
        {
            this.configName = configName;
            this.nodes = nodes;
            this.path = path;
        }
        public Node GetBossNode()
        {
            return nodes.FirstOrDefault(n => n.NodeTypeEnum == NodeType.Boss_Enemy);
        }

        public float DistanceBetweenFirstAndLastLayers()
        {
            var bossNode = GetBossNode();
            var firstLayerNode = nodes.FirstOrDefault(n => n.point.y == 0);

            if (bossNode == null || firstLayerNode == null)
                return 0f;

            return bossNode.position.y - firstLayerNode.position.y;
        }

        public Node GetNode(Point point)
        {
            return nodes.FirstOrDefault(n => n.point.Equals(point));
        }
        public string ToJson()
        =>  Rei.Utilities.JsonUtilityConverter.ConvertToJson(this);
    }
}