using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppsFlyerTrackerCallbacks : MonoBehaviour
{
    public static AppsFlyerTrackerCallbacks Instance;

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
}
