using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Characters.Stats;

public class StaminaUI : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI _text;
    [SerializeField] Image _backgroundImage;
    [SerializeField] Image _decorate;
    UnityAction<int> manaSet;

    private void Start()
    {
        StaminaHandler.StaminaUI = this;
    
    }
    public void SetText(int stamina) => _text.text =(stamina).ToString();
}
