
using UnityEngine;
using UnityEngine.UI;

public class LoaderScreenImage : MonoBehaviour
{
    [SerializeField] Image img;

    [SerializeField] Sprite[] _sprites;

    void Start()
    {
        img.sprite = _sprites[Random.Range(0, _sprites.Length)];
    }

}
