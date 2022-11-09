using System.Collections.Generic;

namespace CardMaga.UI
{

    public static class UIHistoryManager
    {
        private static Stack<IUIElement> _history = new Stack<IUIElement>();
        private static IUIElement _currentUIElement;

		public static bool IsEmpty => _history.Count == 0;

        public static void Show(IUIElement showable,bool toRemember)
        {
			if (_currentUIElement != null)
			{
				if (toRemember)
					_history.Push(_currentUIElement);
				

				_currentUIElement.Hide();
			}

			showable.Show();

			_currentUIElement = showable;
		}

		public static void ShowLast()
		{
			if (!IsEmpty)
				Show(_history.Pop(), false);

			if(IsEmpty)
				_currentUIElement = null;
		}

		public static void CloseAll()
        {
			while (!IsEmpty)
				ShowLast();
		}


    }

}