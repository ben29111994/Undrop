using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppsflyerManager : MonoBehaviour
{
    public string devKey;
    public string appID;
    public bool isEnable;

    public static AppsflyerManager Instance;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }

    void Start()
    {
        /* Mandatory - set your AppsFlyer’s Developer key. */
        AppsFlyer.setAppsFlyerKey(devKey);
        /* For detailed logging */
        /* AppsFlyer.setIsDebug (true); */
#if UNITY_IOS
        /* Mandatory - set your apple app ID
        NOTE: You should enter the number only and not the "ID" prefix */
        AppsFlyer.setAppID(appID);
        AppsFlyer.getConversionData();
        AppsFlyer.trackAppLaunch();
#elif UNITY_ANDROID
     /* For getting the conversion data in Android, you need to add the "AppsFlyerTrackerCallbacks" listener.*/
     AppsFlyer.init (devKey,"AppsFlyerTrackerCallbacks");
#endif

        Debug.Log("Initialize Appsflyer");

    }


}
