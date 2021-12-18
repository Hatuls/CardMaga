using UnityEngine;
using UnityEngine.UI;

public class ContentFitterRefresh : MonoBehaviour
{
    [Sirenix.OdinInspector.Button()]
    public void RefreshContentFitters()
    {
        var rectTransform = (RectTransform)transform;
        RefreshContentFitter(rectTransform);
    }

    private void RefreshContentFitter(RectTransform transform)
    {
        if (transform == null || !transform.gameObject.activeSelf)
        {
            return;
        }

        foreach (RectTransform child in transform)
        {
            RefreshContentFitter(child);
        }

        if (transform.TryGetComponent(out LayoutGroup layoutGroup))
        {
            layoutGroup.SetLayoutHorizontal();
            layoutGroup.SetLayoutVertical();
        }
        if (transform.TryGetComponent(out ScrollRect scrollRect))
        {
            scrollRect.SetLayoutHorizontal();
            scrollRect.SetLayoutVertical();
        }
        if (transform.TryGetComponent(out ContentSizeFitter contentSizeFitter))
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform);
        }

    }
}