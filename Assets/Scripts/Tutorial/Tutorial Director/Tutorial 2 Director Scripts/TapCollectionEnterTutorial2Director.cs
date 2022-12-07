using CardMaga.UI.Collections;
using TutorialDirector;
using UnityEngine;

public class TapCollectionEnterTutorial2Director : BaseTutorialDirector
{
    [SerializeField] TutorialComboCollectionHandler _tutorialComboCollectionHandler;
    // Start is called before the first frame update
    protected override void MoveDirectorPosition()
    {
        _directorRect.transform.position = _tutorialComboCollectionHandler._enterButtonTransform.transform.position;
        _directorRect.transform.position = new Vector3(_directorRect.transform.position.x, _directorRect.transform.position.y+60, _directorRect.transform.position.z);
       _playableDirector.Play();
    }

    protected override void SubscribeEvent()
    {
        _tutorialComboCollectionHandler.OnCollectionEnterButton += StopDirector;
    }

    protected override void UnsubscribeEvent()
    {
        _tutorialComboCollectionHandler.OnCollectionEnterButton -= StopDirector;
    }
}
