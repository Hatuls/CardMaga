using CardMaga.UI;

namespace CardMaga.MetaUI
{

    public class MainMenu : BaseUIScreen
    {
        private void Awake()
        {
            UIHistoryManager.OnEmpty += OpenScreen;
            OpenScreen();
        }
        private void OnDestroy()
        {
        UIHistoryManager.OnEmpty -= OpenScreen;
            
        }

    }

}