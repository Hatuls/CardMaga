using System;
using GooglePlayGames.BasicApi.Nearby;
using UnityEngine;

#if UNITY_ANDROID

namespace GooglePlayGames.OurUtils
{
    public class NearbyHelperObject : MonoBehaviour
    {
        // our (singleton) instance
        private static NearbyHelperObject instance;

        // timers to keep track of discovery and advertising
        private static double mAdvertisingRemaining;
        private static double mDiscoveryRemaining;

        // nearby client to stop discovery and to stop advertising
        private static INearbyConnectionClient mClient;

        public void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void Update()
        {
            // check if currently advertising
            if (mAdvertisingRemaining > 0)
            {
                mAdvertisingRemaining -= Time.deltaTime;
                if (mAdvertisingRemaining < 0) mClient.StopAdvertising();
            }

            // check if currently discovering
            if (mDiscoveryRemaining > 0)
            {
                mDiscoveryRemaining -= Time.deltaTime;
                if (mDiscoveryRemaining < 0) mClient.StopDiscovery(mClient.GetServiceId());
            }
        }

        public void OnDisable()
        {
            if (instance == this) instance = null;
        }

        public static void CreateObject(INearbyConnectionClient client)
        {
            if (instance != null) return;

            mClient = client;
            if (Application.isPlaying)
            {
                // add an invisible game object to the scene
                var obj = new GameObject("PlayGames_NearbyHelper");
                DontDestroyOnLoad(obj);
                instance = obj.AddComponent<NearbyHelperObject>();
            }
            else
            {
                instance = new NearbyHelperObject();
            }
        }

        private static double ToSeconds(TimeSpan? span)
        {
            if (!span.HasValue) return 0;

            if (span.Value.TotalSeconds < 0) return 0;

            return span.Value.TotalSeconds;
        }

        public static void StartAdvertisingTimer(TimeSpan? span)
        {
            mAdvertisingRemaining = ToSeconds(span);
        }

        public static void StartDiscoveryTimer(TimeSpan? span)
        {
            mDiscoveryRemaining = ToSeconds(span);
        }
    }
}
#endif