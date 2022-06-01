using ReiTools.TokenMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Map
{

    public class MapsLoader
    {
          public  Map[] Maps;
  
        public MapsLoader(MonoBehaviour _mb , Action OnCompleteLoading = null)
        {
            Debug.Log("Loading Maps From Resources");
            _mb.StartCoroutine(LoadMapsResources(OnCompleteLoading));
        }
        private IEnumerator LoadMapsResources(Action OnComplete)
        {
            var texts = Resources.LoadAll<TextAsset>("MapCFG");
            if (texts.Length == 0)
            {
                Debug.LogWarning("No Maps Found!");
                yield break;
            }
            List<Map> _maps = new List<Map>(texts.Length);

            for (int i = 0; i < texts.Length; i++)
                 yield return CreateMap(texts[i].text, _maps);


            Maps = _maps.ToArray();
            Debug.Log("<a>Map Loading Complete!</a>");
            OnComplete?.Invoke();
        }

        private IEnumerator CreateMap(string data, List<Map> maps)
        {
            Debug.Log(data);
                var m = JsonUtilityHandler.LoadFromJson<Map>(data);
            yield return null;
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
        [SerializeField]
        string _saveMapCFGName;




       static string _folderPath = "Map/";
       static string _fileName = "Map";
        static SaveManager.FileStreamType saveType = SaveManager.FileStreamType.FileStream;


        public MapsLoader MapsLoader { get; private set; }




#if UNITY_EDITOR
        [Sirenix.OdinInspector.Button("Save Map Config")]
        public void SaveMapConfig()
        {
            SaveManager.SaveFile(CurrentMap, _saveMapCFGName, saveType, false, "txt", "Resources/MapCFG/");
        }

        [Sirenix.OdinInspector.Button("Load Map Config")]
        public void LoadMapConfig()
        {
         var txt =  Resources.Load<TextAsset>("MapCFG/"+_saveMapCFGName);
            CurrentMap = JsonUtilityHandler.LoadFromJson<Map>(txt.text);
      
            _mapView.ShowMap(CurrentMap);
        }
#endif




        private void LoadMaps(ITokenReciever token)
        {
            IDisposable _token = token.GetToken();
            MapsLoader = new MapsLoader(this, _token.Dispose);
        }

   

        [Sirenix.OdinInspector.Button()]
        public static void ResetSavedMap() => Account.AccountManager.Instance.BattleData.Map = null; 

        [Sirenix.OdinInspector.Button()]
        public void GenerateMap() => GenerateNewMap();

        private void GenerateNewMap()
        {

            CurrentMap = MapGenerator.GetMap(_mapCFG);

#if UNITY_EDITOR
            Debug.Log(CurrentMap.ToJson());
#endif

            _mapView.ShowMap(CurrentMap);
        }

        public void SaveMap()
        {
            if (CurrentMap == null)
            return;
            Account.AccountManager.Instance.SaveAccount();
        }
        private void FinishedMap()
        {
            _endScreen.FinishGame();
        }
        private void StartMap()
        {
            _soundSO.PlaySound();
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
                var maps = MapsLoader.Maps;
                for (int i = 0; i < maps.Length; i++)
                    maps[i].path.Clear();
                CurrentMap = maps[UnityEngine.Random.Range(0, maps.Length)];
                _mapView.ShowMap(CurrentMap);
            }
     
        }


        #region Mono behaviour Callbacks


        private void OnApplicationQuit()
        {
            SaveMap();
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                SceneHandler.OnBeforeSceneShown += this.LoadMaps;
                SceneHandler.OnSceneStart += StartMap;
            }
            else if (_instance != this)
                Destroy(this.gameObject);


        }
   
        private void OnDestroy()
        {
            SceneHandler.OnSceneStart -= StartMap;
            SceneHandler.OnBeforeSceneShown -= this.LoadMaps;
        }
        #endregion
    }

}
