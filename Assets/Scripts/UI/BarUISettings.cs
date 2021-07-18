using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName ="Bars Settings", menuName = "ScriptableObjects/Settings/Bars")]
public class BarUISettings : ScriptableObject
{
    [TitleGroup("Bars Settings")]

    [TabGroup("Bars Settings/Settings", "Hp Bar")]
    public LeanTweenType LeanTweenEase;
    [TabGroup("Bars Settings/Settings", "Hp Bar")]
    public float DelayTime;
}