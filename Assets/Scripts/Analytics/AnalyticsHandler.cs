using UnityEngine.Analytics;
public static class AnalyticsHandler
{
    public static void SendEvent(string EventName)
    => CheckWarning(Analytics.CustomEvent(EventName));

    public static void SendEvent(string EventName, System.Collections.Generic.Dictionary<string, object> dictionary)
    => CheckWarning(Analytics.CustomEvent(EventName, dictionary));


    private static void CheckWarning(AnalyticsResult data)
    {
        if (data != AnalyticsResult.Ok)
            UnityEngine.Debug.LogError($"Analytic was not sent!\nResult is : {data}");

    }
}
