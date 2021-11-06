using UnityEngine;
using System.Threading.Tasks;
using Keywords;
using UnityEditor;
using System.Collections.Generic;

public class CSVToKeywordsSO :CSVAbst
{
    private static bool _isCompleted;
    public async override Task StartCSV(string url)
    {
        _isCompleted = false;

        WebRequests.Get(url, (x) => Debug.Log("CSVToKeywordsSOError: " + x), OnCompleteDownloadingKeywordsCSV);

        do
        {
            await Task.Yield();
        } while (_isCompleted== false);
    }
    private static void OnCompleteDownloadingKeywordsCSV(string csv)
    {
        string[] rows = csv.Replace("\r", "").Split('\n');

        List<KeywordSO> _keywordList = new List<KeywordSO>();
        const int secondRow = 2;

        for (int i = secondRow; i < rows.Length; i++)
        {
            string[] line = rows[i].Replace('"', ' ').Replace('/', ' ').Split(',');
            if (int.TryParse(line[0], out int id))
            {
                Debug.Log(System.Enum.GetNames(  typeof (KeywordTypeEnum)));
                Debug.Log((KeywordTypeEnum)id);
                KeywordSO keyword = ScriptableObject.CreateInstance<KeywordSO>();
                keyword.Init(line);
                AssetDatabase.CreateAsset(keyword, string.Concat("Assets/Resources/KeywordsSO/", $"{keyword.GetKeywordType.ToString()}KeywordSO", ".asset"));
                _keywordList.Add(keyword);
            }
            else
                break;
        }
       var keywor  = _keywordList.ToArray();

      CSVManager._keywordsSO  = ScriptableObject.CreateInstance<KeywordsCollectionSO>();
        CSVManager._keywordsSO.Init(keywor);
        AssetDatabase.CreateAsset(CSVManager._keywordsSO, string.Concat("Assets/Resources/Collection SO/", "KeywordSOCollection", ".asset"));

        AssetDatabase.SaveAssets();
        _isCompleted = true;
    }

}

