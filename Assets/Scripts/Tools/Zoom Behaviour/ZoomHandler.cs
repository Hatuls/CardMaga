using DG.Tweening;
using System;

namespace CardMaga.UI
{
    /// <summary>
    /// Object that hold reference to a IZoomable
    /// </summary>
    public interface IZoomableObject
    {
        IZoomable ZoomHandler { get; }
    }
    /// <summary>
    /// The Object itself that apply the zooming
    /// </summary>
    public interface IZoomable : IMotionable
    {
        event Action OnZoomInCompleted;
        event Action OnZoomOutCompleted;
        Sequence ZoomingIn();
        Sequence ZoomingOut();
    }
    public static class ZoomHandler
    {
        private static IZoomable _currentZoomed;
        private static bool IsAnimating = false;

        /// <summary>
        /// Return to default state
        /// </summary>
        public static void ForceReset()
        {
            if (_currentZoomed != null)
            {
                IsAnimating = false;
                _currentZoomed.ForceReset();
                _currentZoomed = null;
            }
        }

        /// <summary>
        /// Taking the Zoomable object from the holder
        /// cutPreviousAnimation: will prevent waiting to zoom out
        /// </summary>
        /// <param name="zoomableObject"></param>
        public static Sequence ZoomIn(this IZoomableObject zoomableObject,bool cutPreviousAnimation = false)
        => zoomableObject.ZoomHandler.ZoomIn(cutPreviousAnimation);

        /// <summary>
        /// Zoom In Object
        /// And assigning the current zoom active
        /// And cut any previous animation
        /// </summary>
        /// <param name="zoomable"> set default to true</param>
        public static Sequence ZoomIn(this IZoomable zoomable)
        => zoomable.ZoomIn(true);


        /// <summary>
        /// Zoom In Object
        /// And assigning the current zoom active      
        /// 
        /// </summary>
        /// <param name="zoomable"></param>
        /// <param name="cutPreviousAnimation"></param>
        public static Sequence ZoomIn(this IZoomable zoomable, bool cutPreviousAnimation)
        {
            

            if (cutPreviousAnimation || IsAnimating || _currentZoomed == null)
            {
                // Instant set a new zoomed in object
                ForceReset();
                _currentZoomed = zoomable;
                Zoom();
                return _currentZoomed.Sequence;
            }

       
            // Zooming in after animation
            Sequence zoomOutSequence = _currentZoomed.ZoomOut();

            _currentZoomed = zoomable;

            zoomOutSequence.AppendCallback(Zoom);


            return _currentZoomed.Sequence;
        }

        public static Sequence ZoomOut(this IZoomable zoomable)
        {
            IsAnimating = true;
            
            return zoomable.ZoomingOut()
                 .OnComplete(ForceReset);
        }
        //Zooming in
        private static void Zoom()
        {

             _currentZoomed.ZoomingIn().OnComplete(FinishedAnimation);


            IsAnimating = true;

        }
        private static void FinishedAnimation()
        {
            IsAnimating = false;
        }
    }
}
