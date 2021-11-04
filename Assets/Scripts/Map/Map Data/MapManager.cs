using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Map
{
    public class MapManager : MonoBehaviour
    {
        private static MapManager _instance;
        public static MapManager Instance => _instance;

        Map _currentMap;
        public Map CurrentMap => _currentMap;
        [SerializeField]
        MapView _mapView;

        [SerializeField]
        MapConfig _mapCFG;


        [SerializeField] string _saveMapCFGName;
        [Sirenix.OdinInspector.Button("Save Map Config")]
        public void SaveMapConfig()
        {
            SaveManager.SaveFile(_currentMap, _saveMapCFGName,false, "txt", "Maps/");
        }


        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            else if (_instance != this)
                Destroy(this.gameObject);

       
        }
        private void Start()
        {

            StartMap();
        }

        [Sirenix.OdinInspector.Button()]
        public void ResetSavedMap() => PlayerPrefs.DeleteKey("Map");

        [Sirenix.OdinInspector.Button()]
        public void GenerateMap() => GenerateNewMap();

        private  void GenerateNewMap()
        {
            _currentMap = MapGenerator.GetMap(_mapCFG);
 
            Debug.Log(_currentMap.ToJson());
        

            _mapView.ShowMap(_currentMap);
        }

        public async void SaveMap()
        {
            if (_currentMap == null) return;

            SaveManager.SaveFile(_currentMap, "Map");
            await Task.Yield();
        }
 
        private  void StartMap()
        {
            Map  map = SaveManager.Load<Map>("Map", SaveManager.FileStreamType.FileStream);



            if (map != null)
            {
             

                if (map.nodes == null || map.nodes.Count ==0)
                {
                    GenerateNewMap();  
                    return;
                }
                // using this instead of .Contains()
                if (map.path.Any(p => p.y == map.nodes.Count-1))
                {
                    // player has already reached the boss, generate a new map
                    GenerateNewMap();
                }
                else
                {
                    _currentMap = map;
                    // player has not reached the boss yet, load the current map
                    _mapView.ShowMap(map);
                }
                return;

            }else
            GenerateNewMap();

        }


        private void OnApplicationQuit()
        {
            ResetSavedMap();

          //  SaveMap();
        }
    }

}
