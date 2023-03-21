using Firebase;
using Firebase.Analytics;
using ReiTools.TokenMachine;
using System;
using UnityEngine;

public static class FireBaseHandler
{
    static Parameter[] _additionalParameters = new Parameter[]
    {
        new Parameter("device",SystemInfo.deviceModel),
        new Parameter("currentTime", DateTime.Now.ToString())
    };
    public static void Init()
    {
       
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(
            task =>
        {
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        });

    }

    public static void SendEvent(string eventName, params Parameter[] parameters)
    {

        FirebaseAnalytics.LogEvent(eventName, AddAdditionalParameters(parameters));
    }
    private static Parameter[] AddAdditionalParameters(params Parameter[] parameters)
    {
        Parameter[] newArray = new Parameter[_additionalParameters.Length + parameters.Length];
        Array.Copy(_additionalParameters, newArray, _additionalParameters.Length);
        Array.Copy(parameters, 0, newArray, _additionalParameters.Length, parameters.Length);
        return newArray;
    }
    public static void SendEvent(string eventName)
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {

            FirebaseAnalytics.LogEvent(eventName);
        });
    }


}
