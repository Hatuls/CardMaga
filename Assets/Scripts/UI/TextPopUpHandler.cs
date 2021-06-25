
using UnityEngine;
namespace Battles.UI
{
    public class TextPopUpHandler : MonoBehaviour
    {
        private static TextPopUpHandler _Instance;
        public static TextPopUpHandler GetInstance => _Instance;

        [SerializeField] RectTransform[] PosOnCanvas;


        [SerializeField] TextPopUpSettingsSO _textPopUpSettingsSO;



        [SerializeField] TextPopUp[] _textsPopups;


        public TextPopUpSettingsSO GetTextSettings => _textPopUpSettingsSO;
        private void Awake()
        {
            _Instance = this;
        }



        private void Start()
        {
            if (_textsPopups != null && _textsPopups.Length > 0)
            {
                for (int i = 0; i < _textsPopups.Length; i++)
                    _textsPopups[i].ResetTextsPopUp();
            }
        }

        public void CreatePopUpText(TextType type, bool isPlayer, string txt)
        {
            if (_textsPopups != null && _textsPopups.Length > 0)
            {

                for (int i = 0; i < _textsPopups.Length; i++)
                {
                    if (_textsPopups[i].gameObject.activeSelf == false)
                    {
                        _textsPopups[i].gameObject.SetActive(true);
                        _textsPopups[i].Create(ref GetTextColor(type), TextPosition(isPlayer), txt);
                        return;
                    }
                }


                Debug.LogError("TextPopUpHandler: couldnt active text pop up because all is used!");
            }
        }


        private Vector2 TextPosition(bool isPlayer)
        {
            return (isPlayer ? PosOnCanvas[0] : PosOnCanvas[1]).anchoredPosition3D;
        }


        private ref Color GetTextColor(TextType type)
        {
      
            switch (type)
            {
                case TextType.NormalDMG:
                    //     ColorUtility.TryParseHtmlString("#FF7200", out color);
                    return ref GetTextSettings.NormalDamage;

                

                case TextType.CritDMG:
                    //    ColorUtility.TryParseHtmlString("#FF2D00", out color);
                    return ref GetTextSettings.CriticalDamage;
                   

                case TextType.Healing:
                    // ColorUtility.TryParseHtmlString("#57FF00", out color);
                    return ref GetTextSettings.Healing;
                 

                case TextType.Money:
                    //   ColorUtility.TryParseHtmlString("#FFC600", out color);
                    return  ref GetTextSettings.Money;
                    

                case TextType.Shield:
                    return  ref GetTextSettings.Shield;
            }

            Debug.LogError("TextType: Type Of Text Was not Assigend on switch\n Set It to normal Damage Color");

            return ref GetTextSettings.NormalDamage; ;
            

        }

    }
}