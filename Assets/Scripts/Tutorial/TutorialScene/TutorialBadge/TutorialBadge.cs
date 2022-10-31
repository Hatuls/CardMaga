using System;
using UnityEngine;
using UnityEngine.UI;
using CardMaga.Input;

[Serializable]
public class TutorialBadge : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Color _offColor = Color.red;
    [SerializeField] private Color _onColor = Color.green;
    [SerializeField] private Color _openColor = Color.blue;
    [SerializeField] private TouchableItem _Input;

    private bool _IsCompleted;

    public void Init()
    {
        _IsCompleted = false;
        image.color = _offColor;
    }

    public void Completed()
    {
        image.color = _onColor;
        _IsCompleted = true;
    }

    public void Open()
    {
        image.color = _openColor;
        _Input.UnLock();
    }
}
