using System;
using UnityEngine;

namespace CardMaga.UI
{
    public interface IInitializable
    {
        event Action OnInitializable;
        void Init();
    }
    public interface IShowable
    {
        event Action OnShow;
        event Action OnHide;

        void Show();
        void Hide();
    }
    public interface IUIElement : IShowable, IInitializable
    {
        RectTransform RectTransform { get; }
    }

    
    
}