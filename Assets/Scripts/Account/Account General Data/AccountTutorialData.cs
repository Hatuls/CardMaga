using UnityEngine;

public class AccountTutorialData 
{
    private int _tutorialProgress;
    private bool _isCompletedTutorial;

    public bool IsCompletedTutorial
    {
        get => _isCompletedTutorial;
    }

    public int TutorialProgress
    {
        get => _tutorialProgress;
    }

    public AccountTutorialData(int tutorialProgress, bool isCompletedTutorial)
    {
        _isCompletedTutorial = isCompletedTutorial;
        _tutorialProgress = tutorialProgress;
    }

    public void AssignedData(int tutorialProgress, bool isCompletedTutorial)
    {
        _isCompletedTutorial = isCompletedTutorial;
        _tutorialProgress = tutorialProgress;
    }
}
