using UnityEngine.Analytics;
public static class UnityAnalyticHandler
{
    public static void SendEvent(string EventName)
    {
#if !UNITY_EDITOR 
        CheckWarning(Analytics.CustomEvent(EventName));
#endif
    }

    public static void SendEvent(string EventName, System.Collections.Generic.Dictionary<string, object> dictionary)
    {
#if !UNITY_EDITOR
        CheckWarning(Analytics.CustomEvent(EventName, dictionary));
#endif
    }
    private static void CheckWarning(AnalyticsResult data)
    {
        if (data != AnalyticsResult.Ok)
            UnityEngine.Debug.LogError($"Analytic was not sent!\nResult is : {data}");

    }
}
