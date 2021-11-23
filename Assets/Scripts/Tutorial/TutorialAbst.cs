using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class TutorialAbst : MonoBehaviour
{
    [SerializeField]
    GameObject[] _pages;
    int _currentPage;
    public TutorialAbst()
    {
        _currentPage = 0;
    }
    public virtual void StartTutorial()
    {
        gameObject.SetActive(true);
        for (int i = 0; i < _pages.Length; i++)
        {
            _pages[i].SetActive(false);
        }
        _pages[0].SetActive(true);
    }
    public virtual void EndTutorial()
    {
        gameObject.SetActive(false);
    }
    /// <summary>
    /// true = right, false = left
    /// </summary>
    /// <param name="direction"></param>
    public virtual void ChangePageRight(bool direction)
    {
        if(_pages == null)
        {
            throw new Exception("Tutorial pages are null");
        }
        switch (direction)
        {
            case false:
                if(_currentPage == 0)
                    return;
                    SetPages(-1);
                break;
            case true:
                if(_currentPage == _pages.Length)
                    return;
                    SetPages(+1);
                break;
            default:
                throw new Exception("Tutorial page number has error");
        }
    }
    public virtual void SetPages(int numOfChange)
    {
        _pages[_currentPage].SetActive(false);
        _currentPage += numOfChange;
        _pages[_currentPage].SetActive(true);
    }

}
