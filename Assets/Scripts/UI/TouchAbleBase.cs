using Unity.Events;
using UnityEngine;
using UnityEngine.EventSystems;
[RequireComponent (typeof(CanvasGroup))]
public abstract class TouchAbleBase : MonoBehaviour, IPointerClickHandler , IPointerDownHandler , IPointerUpHandler , IDragHandler, IBeginDragHandler, IEndDragHandler
{

    [SerializeField]
    TouchAbleEvent _SetInput;

    private CanvasGroup _cg;

    public CanvasGroup CanvasGroup
    {
        get
        {
            if (_cg == null)
                _cg = GetComponent<CanvasGroup>();

            return _cg;
        }
    }

    #region On Drag
    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        _SetInput?.Raise(this);
    }

    public virtual void OnDrag(PointerEventData eventData)
    {

    }
    public virtual  void OnEndDrag(PointerEventData eventData)
    {
        _SetInput?.Raise(null);
    }
    #endregion











    #region  On Pointer

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        _SetInput?.Raise(this);
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        _SetInput?.Raise(this);
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        _SetInput?.Raise(null);
    }
    #endregion
}
