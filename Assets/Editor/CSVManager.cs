using System;
using UnityEditor;
using UnityEngine;

public class CSVManager
{
    static Keywords.KeywordSO[] _keywordsSO;
    static Sprite[] cardsPictures;

    const string _driveURLOfCardSO = "https://docs.google.com/spreadsheets/d/1R1mP6Bk_rplQTWiIapxpgYIezIZWsVI7z-m2up1Ck88/export?format=csv&gid=1611461659";
    const string _driveURLOfRecipeSO = "https://docs.google.com/spreadsheets/d/1R1mP6Bk_rplQTWiIapxpgYIezIZWsVI7z-m2up1Ck88/export?format=csv&gid=371699274";
    const string _driveURLOfCharacterSO = "https://docs.google.com/spreadsheets/d/1R1mP6Bk_rplQTWiIapxpgYIezIZWsVI7z-m2up1Ck88/export?format=csv&gid=945070348";

    static string[] csv;
    [MenuItem("Google Drive/Update All")]
    public static void Update()
    {
        WebRequests.Get(_driveURLOfCardSO, (x) => Debug.Log("Error " + x), OnCompleteDownloadingCardCSV);
    }
    private static void OnCompleteDownloadingCardCSV(string txt)
    {
        DestroyWebGameObjects();

        _keywordsSO = Resources.LoadAll<Keywords.KeywordSO>("KeywordsSO");
        float timeout = 1000000;
        float timer = 0;
        while (_keywordsSO == null && timer < timeout)
        {
            _keywordsSO = Resources.LoadAll<Keywords.KeywordSO>("KeywordsSO");
            timer += 0.5f;
        }
        if (_keywordsSO == null)
            Debug.LogError("Keywords SO is null!!");

        cardsPictures = Resources.LoadAll<Sprite>("Art/CardsPictures");
        timer = 0;
        while (cardsPictures == null && timer < timeout)
        {
            cardsPictures = Resources.LoadAll<Sprite>("Art/CardsPictures");
            timer += 0.5f;
        }
        if (cardsPictures == null)
            Debug.LogError("CardPictures is null!!");
    }


    private static void DestroyWebGameObjects()
    {
        var gos = GameObject.FindGameObjectsWithTag("Web");

        for (int i = gos.Length - 1; i >= 0; i--)
            GameObject.DestroyImmediate(gos[i]);
    }
}
