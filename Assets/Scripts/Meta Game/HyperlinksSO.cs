using UnityEngine;
using Sirenix.OdinInspector;
[CreateAssetMenu(fileName = "Hyperlinks", menuName = "ScriptableObjects/Hyperlinks")]
public class HyperlinksSO : ScriptableObject
{
    public const int DiscordChannel = 0;
    public const int GooglePlayLink = 1;

    [SerializeField] [InfoBox("0 = Discord\n1 = Google Play Store")]
    string[] _links;
    public void UseLink(int linkNumber)
    {
        Application.OpenURL(_links[linkNumber]);
    }

}
