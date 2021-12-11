using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class MinSizeFitter : UIBehaviour
{
    #region Field
    [SerializeField] RectTransform _rectTransform;
    #endregion

    #region Property
    public RectTransform RectTransform
    {
        get
        {
            if (_rectTransform == null)
                throw new System.Exception("MinVerticalSizeFitter rect is null");
            return _rectTransform;
        }
    }
    DrivenRectTransformTracker Tracker;
    #endregion
    #region Methods
    private void OnEnable()
    {
        SetDirty();
    }
    private void OnDisable()
    {
        Tracker.Clear();
    }
    private void HandleSelfFittingAlongAxis(int axis)
    {
        Tracker.Add(this, RectTransform,
            (axis == 0 ? DrivenTransformProperties.AnchorMaxX : DrivenTransformProperties.AnchorMaxY) |
            (axis == 0 ? DrivenTransformProperties.SizeDeltaX : DrivenTransformProperties.SizeDeltaY));

        // Set anchor max to same as anchor min along axis
        Vector2 anchorMax = RectTransform.anchorMax;
        anchorMax[axis] = RectTransform.anchorMin[axis];
        RectTransform.anchorMax = anchorMax;

        Vector2 sizeDelta = RectTransform.sizeDelta;
        sizeDelta[axis] = LayoutUtility.GetMinSize(_rectTransform, axis);
        RectTransform.sizeDelta = sizeDelta;


    }
    public void SetLayoutHorizontal()
    {
        Tracker.Clear();
        HandleSelfFittingAlongAxis(0);
    }

    public void SetLayoutVertical()
    {
        HandleSelfFittingAlongAxis(1);
    }
    protected void SetDirty()
    {
        if (!IsActive())
            return;

        LayoutRebuilder.MarkLayoutForRebuild(RectTransform);
    }
    #endregion
}
