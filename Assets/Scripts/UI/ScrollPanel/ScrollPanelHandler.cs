using System;
using System.Collections.Generic;
using CardMaga.Card;
using CardMaga.UI.Card;
using UnityEngine;

public class ScrollPanelHandler : MonoBehaviour
{
    private List<IShowableUI> _loadedObjects;

    public void LoadObject(params IShowableUI[] objects)
    {
        foreach (var obj in objects)
        {
            obj.Show();
        }
    }   

    public void UpdateObject(params IShowableUI[] objects)
    {
        
    }

    public void UnLoadAllObject()
    {
        
    }

    public void UnLoadObject(params IShowableUI[] objects)
    {
        
    }
}

public interface IShowableUI
{
    void Show();
}
