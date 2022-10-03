using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class TutorialBadge 
{
    [SerializeField] private Image image;
    
    private Color _offColor = Color.red;
    private Color _onColor = Color.green;

    public void Init()
    {
        image.color = _offColor;
    }

    public void TurnOn()
    {
        image.color = _onColor;
    }
}
