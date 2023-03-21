using UnityEngine;

[CreateAssetMenu(fileName = "New tip SO", menuName = "ScriptableObjects/Tips/New Tip SO")]
public class TipsScriptableObject : ScriptableObject
{
    [SerializeField] private string[] _tips;
    
    public int GetLength
    {
        get => _tips.Length;
    }

    public string GetTip(int tipIndex)
    {
        return _tips[tipIndex];
    }
}
