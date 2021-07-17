using UnityEngine;
namespace Art
{

    public class Palette : ScriptableObject
    {

    }





    [System.Serializable]
    internal class ColorSettings
    {
        [SerializeField]
        Color[] _colors;

        public Color[] Colors => _colors;
    }
}