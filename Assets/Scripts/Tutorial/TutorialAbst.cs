using UnityEngine;

public abstract class TutorialAbst : MonoBehaviour
{
    [SerializeField]
    GameObject[] _pages;
    public int PageLength => _pages.Length;
    public virtual void StartTutorial()
    {
        gameObject.SetActive(true);
        ResetPages();
        _pages[0].SetActive(true);
    }

    public void ResetPages()
    {
        for (int i = 0; i < _pages.Length; i++)
        {
            _pages[i].SetActive(false);
        }
    }

    public virtual void EndTutorial()
    {
        gameObject.SetActive(false);
    }

    public virtual void SetPages(int _currentPage)
    {
        ResetPages();
        if (_currentPage>= 0 && _currentPage< _pages.Length)
              _pages[_currentPage].SetActive(true);
        
    }

}
