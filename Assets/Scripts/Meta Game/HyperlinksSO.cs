using UnityEngine;
using Sirenix.OdinInspector;
[CreateAssetMenu(fileName = "Hyperlinks", menuName = "ScriptableObjects/Hyperlinks")]
public class HyperlinksSO : ScriptableObject
{
    [SerializeField] [InfoBox("0 = Discord")]
    string[] _links;
    public void UseLink(int linkNumber)
    {
        Application.OpenURL(_links[linkNumber]);
    }

}
