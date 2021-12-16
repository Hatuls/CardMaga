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

        public Map CurrentMap { get => Account.AccountManager.Instance.BattleData.Map; set => Account.AccountManager.Instance.BattleData.Map = value; }
        [SerializeField]
        MapView _mapView;
        [SerializeField]
        EndRunScreen _endScreen;
        [SerializeField]
        MapConfig _mapCFG;

        //[SerializeField]
        //MapData _mapTracker;
        [SerializeField]
        SoundEventSO _soundSO;
        [SerializeField] string _saveMapCFGName;
       static string _folderPath = "Map/";
       static string _fileName = "Map";
        static SaveManager.FileStreamType saveType = SaveManager.FileStreamType.FileStream;
#if UNITY_EDITOR
        [Sirenix.OdinInspector.Button("Save Map Config")]
        public void SaveMapConfig()
        {
            SaveManager.SaveFile(CurrentMap, _saveMapCFGName, saveType, false, "txt", "Resources/MapCFG/");
        }
#endif

        [Sirenix.OdinInspector.Button("Load Map Config")]
        public void LoadMapConfig()
        {
         var txt =  Resources.Load<TextAsset>("MapCFG/"+_saveMapCFGName);
            CurrentMap = JsonUtilityHandler.LoadFromJson<Map>(txt.text);
      
            _mapView.ShowMap(CurrentMap);
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
            _soundSO.PlaySound();
           StartMap();
        }

        [Sirenix.OdinInspector.Button()]
        public static void ResetSavedMap() => Account.AccountManager.Instance.BattleData.Map = null; //PlayerPrefs.DeleteKey("Map");

        [Sirenix.OdinInspector.Button()]
        public void GenerateMap() => GenerateNewMap();

        private  void GenerateNewMap()
        {
            CurrentMap = MapGenerator.GetMap(_mapCFG);
 
            Debug.Log(CurrentMap.ToJson());
        

            _mapView.ShowMap(CurrentMap);
        }

        public async void SaveMap()
        {
            if (CurrentMap == null) return;

            Account.AccountManager.Instance.SaveAccount();
            await Task.Yield();
        }
 
        private  void StartMap()
        {

            Debug.Log("Map!");
                          Map map = Account.AccountManager.Instance.BattleData.Map;

            if (map!= null && map.configName != "" && map.configName != null)
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
                    CurrentMap = map;
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
                CurrentMap = maps[Random.Range(0, maps.Length)];
                _mapView.ShowMap(CurrentMap);
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
            _endScreen.FinishGame();
            
        }

        private void OnApplicationQuit()
        {
             SaveMap();

        }
    }

}
