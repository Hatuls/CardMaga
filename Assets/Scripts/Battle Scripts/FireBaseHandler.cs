using Firebase;
using Firebase.Analytics;
//#if !UNITY_EDITOR
public static class FireBaseHandler
{
    public static void Init()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        });
    }  

    public static void SendEvent(string eventName, params Parameter[] parameters)
    {
        FirebaseAnalytics.LogEvent(eventName, parameters);
    }

    public static void SendEvent(string eventName)
    {
        FirebaseAnalytics.LogEvent(eventName);
    }


}
//#endif