using Unity.Events;
using UnityEngine;
using UnityEngine.Events;
[System.Serializable]
public class IntSerializedEvent : UnityEvent<int> { }
public class TutorialPage : MonoBehaviour
{
    [SerializeField]
    GameObject[] _pages;

    [SerializeField] IntSerializedEvent OnPageChanged;
    [SerializeField] UnityEvent OnFinalPage;
    
    public int PageLength => _pages.Length;
    public void StartTutorial()
    {
        gameObject.SetActive(true);
        SetPages(0);
    }

    public void ResetPages()
    {
        for (int i = 0; i < _pages.Length; i++)
            _pages[i].SetActive(false);
    }

    public void EndTutorial()
    {

        gameObject.SetActive(false);
    }

    public virtual void SetPages(int _currentPage)
    {
        ResetPages();

        if (_currentPage >= 0 && _currentPage < _pages.Length)
        {
            OnPageChanged?.Invoke(_currentPage);
            OpenPage(_currentPage);
      
        }else
                OnFinalPage?.Invoke();
    }
    private void OpenPage(int page) => _pages[page].SetActive(true);
}
