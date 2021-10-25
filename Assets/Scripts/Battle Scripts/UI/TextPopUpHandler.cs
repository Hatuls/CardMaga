
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
        [SerializeField] GameObject _textPopUpPrefab;

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

        public void CreatePopUpText(TextType type, Vector2 location, string txt)
        {
            if (_textsPopups != null && _textsPopups.Length > 0)
            {

                for (int i = 0; i < _textsPopups.Length; i++)
                {
                    if (_textsPopups[i] == null)
                        break;

                    if ( _textsPopups[i].gameObject.activeSelf == false)
                    {
                        _textsPopups[i].gameObject.SetActive(true);
                        _textsPopups[i].Create(ref GetTextColor(type), location, txt);
                        return;
                    }
                }

                var go =Instantiate(_textPopUpPrefab, this.transform);
                int index = _textsPopups.Length;
                System.Array.Resize(ref _textsPopups, _textsPopups.Length + 5);
                _textsPopups[index]= go.GetComponent<TextPopUp>();
                _textsPopups[index].Create(ref GetTextColor(type), location, txt);
         //       Debug.LogError("TextPopUpHandler: couldnt active text pop up because all is used!");
            }
        }


        public static Vector2 TextPosition(bool isPlayer)
        {
            return (isPlayer ?
                _Instance.PosOnCanvas[0] : _Instance.PosOnCanvas[1]).anchoredPosition3D;
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