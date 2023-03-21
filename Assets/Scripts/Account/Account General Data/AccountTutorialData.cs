[System.Serializable]
public class AccountTutorialData
{
    public const string PlayFabKeyName = "Tutorial";

    public bool IsTutorialRewardTaken;
    public bool IsCompletedTutorial;
    public int TutorialProgress;

    public void AssignedData(int tutorialProgress, bool isCompletedTutorial)
    {
        IsCompletedTutorial = isCompletedTutorial;
        TutorialProgress = tutorialProgress;
    }

    public void UpdateToNextTutorial()
    {
        TutorialProgress++;
    }

    public void Reset()
        => AssignedData(0, false);

}
