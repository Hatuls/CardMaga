using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour
{
    [SerializeField] private TouchableItem input;
    void Start()
    {
        // input.OnClick += OnClick;
        // input.OnPoinrUp += OnUp;
        // input.OnHold += OnHold;
        // input.OnBeginHold += OnStartHold;
        // input.OnEndHold += OnEndHold;
        // input.OnPointDown += OnDown;
    }

    private void OnClick(PointerEventData eventData)
    {
        Debug.Log("Clik");
    }

    private void OnUp(PointerEventData eventData)
    {
        Debug.Log("Up");
    }

    private void OnDown(PointerEventData eventData)
    {
        Debug.Log("Down");
    }

    private void OnHold(PointerEventData eventData)
    {
        Debug.Log("Hold");
    }

    private void OnStartHold(PointerEventData eventData)
    {
        Debug.Log("StartHold");
    }

    private void OnEndHold(PointerEventData eventData)
    {
        Debug.Log("EndHold");
    }
}
