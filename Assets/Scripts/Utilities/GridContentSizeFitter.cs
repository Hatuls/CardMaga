using UnityEngine;
using UnityEngine.UI;

public class GridContentSizeFitter : MonoBehaviour
{
    private void OnEnable()
    {
        RefreshSize();
    }

    public void RefreshSize()
    {
        SetRectVerticalSize(GetAmountOfActiveObjects());
    }
    int GetAmountOfActiveObjects()
    {
#if UNITY_EDITOR
        Debug.Log($"Total children are: {_holderRect.childCount}");
#endif
        int activeObjects = 0;
        for (int i = 0; i < _holderRect.childCount; i++)
        {
            if(_holderRect.GetChild(i).gameObject.activeSelf == true)
            {
                activeObjects++;
            }
        }
#if UNITY_EDITOR
        Debug.Log($"Active children are: {activeObjects}");
#endif
        return activeObjects;
    }

    [SerializeField]
    GridLayoutGroup _gridLayoutGroup;
    [SerializeField]
    RectTransform _holderRect;
    void SetRectVerticalSize(int amountOfObjects)
    {
        if(amountOfObjects == 0)
        {
            return;
        }

        int childNum = 0;
        float hightCount = Mathf.Ceil(amountOfObjects / (float)_gridLayoutGroup.constraintCount);
#if UNITY_EDITOR
        Debug.Log($"length amount is:{ hightCount}");
#endif
        for (int i = 0; i < hightCount; i++)
        {
            for (int r = 0; r < _gridLayoutGroup.constraintCount; r++)
            {
                if(amountOfObjects == childNum)
                {
#if UNITY_EDITOR
                    Debug.Log("no more children to activate");
#endif
                    continue;
                }
                childNum++;
            }
        }

        float finalHight = (_gridLayoutGroup.cellSize.y * hightCount) + (hightCount*_gridLayoutGroup.spacing.y)
            + _gridLayoutGroup.padding.top + _gridLayoutGroup.padding.bottom;
        _holderRect.sizeDelta = new Vector2(_holderRect.rect.width, finalHight);
    }
}
