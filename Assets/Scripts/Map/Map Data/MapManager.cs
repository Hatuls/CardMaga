using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Map
{

    public class MapsTemplateContainer
    {
       public  Map[] maps;
        public MapsTemplateContainer()
        {
            Debug.Log("Loading Maps From Resources");
            LoadMapsResources();
        }
        private async void LoadMapsResources()
        {
            var texts = Resources.LoadAll<TextAsset>("MapCFG");
            if (texts.Length == 0)
            {
                Debug.LogWarning("No Maps Found!");
                return;
            }
            List<Map> _maps = new List<Map>(texts.Length);

            for (int i = 0; i < texts.Length; i++)
                CreateMap(texts[i].text, _maps);
            

                await Task.Yield();
            maps = _maps.ToArray();
            Debug.Log("<a>Map Loading Complete!</a>");
        }

        private async void CreateMap(string data, List<Map> maps)
        {
            Debug.Log(data);
                var m = JsonUtilityHandler.LoadFromJson<Map>(data);
            await Task.Yield();
            maps.Add(m);

        }
    }



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
#if UNITY_EDITOR
        public SaveManager.FileStreamType saveType;
        [Sirenix.OdinInspector.Button("Save Map Config")]
        public void SaveMapConfig()
        {
            SaveManager.SaveFile(_currentMap, _saveMapCFGName, saveType, false, "txt", "Resources/MapCFG/");
        }
#endif

        [Sirenix.OdinInspector.Button("Load Map Config")]
        public void LoadMapConfig()
        {
         var txt =  Resources.Load<TextAsset>("MapCFG/"+_saveMapCFGName);
            _currentMap = JsonUtilityHandler.LoadFromJson<Map>(txt.text);
      
            _mapView.ShowMap(_currentMap);
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
            Map  map = SaveManager.Load<Map>("Map", SaveManager.FileStreamType.PlayerPref);

            if (map!= null)
            {
               if (map.nodes == null || map.nodes.Count ==0)
               {
                   GenerateNewMap();  
                   return;
               }

                // if (map.path.Any(p => map.GetNode(p).NodeTypeEnum== NodeType.Boss_Enemy))
                int bossYPoint = _mapCFG._nodeLayers.Length - 1;
                if (map.path.Any(p => p.y == bossYPoint))
                { 
                    FinishedMap();
                }
                else
                {
                    _currentMap = map;
                    // player has not reached the boss yet, load the current map
                    _mapView.ShowMap(map);
                }
                return;
            }
            else
            {
                var maps = Factory.GameFactory.Instance.MapsTemplate.maps;
                for (int i = 0; i < maps.Length; i++)
                    maps[i].path.Clear();
                _currentMap = maps[Random.Range(0, maps.Length)];
                _mapView.ShowMap(_currentMap);
            }
            //if (map != null)
            //{


            //    if (map.nodes == null || map.nodes.Count ==0)
            //    {
            //        GenerateNewMap();  
            //        return;
            //    }
            //    // using this instead of .Contains()
            //    if (map.path.Any(p => p.y == map.nodes.Count-1))
            //    {
            //        // player has already reached the boss, generate a new map
            //        GenerateNewMap();
            //    }
            //    else
            //    {
            //        _currentMap = map;
            //        // player has not reached the boss yet, load the current map
            //        _mapView.ShowMap(map);
            //    }
            //    return;

            //}else
            //GenerateNewMap();

        }

        private  void FinishedMap()
        {
            SceneHandler.LoadScene(SceneHandler.ScenesEnum.MainMenuScene);
        }

        private void OnApplicationQuit()
        {
            ResetSavedMap();

          //  SaveMap();
        }
    }

}
