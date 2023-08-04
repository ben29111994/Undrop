using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalyticsManager : MonoBehaviour
{
    // public static AnalyticsManager instance;

    // public bool isEnable;

    // [HideInInspector] public EventType eventType;

    // public enum EventType
    // {
    //     StartEvent,
    //     CompleteEvent,
    //     FailEvent,
    //     EndEvent
    // }

    // private void Awake()
    // {
    //     instance = this;
    // }

    // // private void Start()
    // // {
    // //     Invoke("DeleteAll", 1f);
    // // }

    // private void DeleteAll()
    // {
    //     PlayerPrefs.DeleteAll();
    //     PlayerPrefs.Save();
    // }

    // public void CallEvent(EventType _eventType)
    // {
    //     if (isEnable == false) return;

    //     StartCoroutine(C_CallEvent(_eventType));
    // }

    // private IEnumerator C_CallEvent(EventType _eventType)
    // {
    //     yield return new WaitForSeconds(1.0f);

    //     switch (_eventType)
    //     {
    //         case EventType.StartEvent:
    //             FacebookAnalytics.instance.LogStartEvent();
    //             AppsflyerManager.Instance.LogStartEvent();
    //             FlurryManager.Instance.LogStartEvent();
    //             break;
    //         // case EventType.CompleteEvent:
    //         //     FacebookAnalytics.instance.LogCompleteEvent();
    //         //     break;
    //         // case EventType.FailEvent:
    //         //     FacebookAnalytics.instance.LogFailEvent();
    //         //     break;
    //         case EventType.EndEvent:

    //             FacebookAnalytics.instance.LogEndEvent();
    //             AppsflyerManager.Instance.LogEndEvent();
    //             FlurryManager.Instance.LogEndEvent();
    //             break;
    //     }
    // }
}
