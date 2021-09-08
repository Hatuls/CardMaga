using UnityEditor;
using System.IO;
using UnityEngine;
using System;

public class CSVtoSO 
{

    const string _driveLink = "https://docs.google.com/spreadsheets/d/1R1mP6Bk_rplQTWiIapxpgYIezIZWsVI7z-m2up1Ck88/gviz/tq?tqx=out:csv&sheet=1611461659.csv";
    [MenuItem("Google Drive/Update Cards SO")]
    public static void GenerateCards()
    {
        WebRequests.Get(_driveLink, (x) => Debug.Log("Error " + x), OnCompleteDownload);
    }

    private static void OnCompleteDownload(string txt)
    {
      var gos = GameObject.FindGameObjectsWithTag("Web");

        for (int i = gos.Length - 1; i >= 0; i--)
            GameObject.DestroyImmediate(gos[i]);


        SeperateFiles(txt);
    }
    private static void SeperateFiles(string data)
    {
        string[] csv = data.Trim().Split(Environment.NewLine.ToCharArray());
      
        for (int i = 1; i < csv.GetLength(0); i++)
            CreateCard(csv[i].Split(','));

        AssetDatabase.SaveAssets();
    }
    private static void CreateCard(string[] cardSO)
    {
        Cards.CardSO card = ScriptableObject.CreateInstance<Cards.CardSO>();

        AssetDatabase.CreateAsset(card, $"Assets/Resources/Cards SO/{card.name}.asset");

    }
}
