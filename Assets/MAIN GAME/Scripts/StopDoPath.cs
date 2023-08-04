using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StopDoPath : MonoBehaviour
{
    [Header("Rotate")]
    public bool isRotate;
    public bool isYaw;
    public bool isRoll;
    public bool isPitch;
    private bool isEnd;
    public Tween myPathTween;
    // public float wayPoint;

    [Header("Scale")]
    public bool isScale;
    public Vector3 scaleLimit;
    public Vector3 scaleFrequency;
    Vector3 startScale;
    Vector3 finalScale;
    Tween scaleTween;

    void Start()
    {
        startScale = transform.localScale;

        // Get tween from DOTweenPath and attach callback
        if (this.GetComponent<DOTweenPath>())
        {
            myPathTween = this.GetComponent<DOTweenPath>().GetTween();
        }

        // wayPoint = GetComponent<DOTweenPath>().wps.Count;

        if (isRotate)
        {
            isEnd = true;
            myPathTween.Pause();
        }
        if (isScale)
        {
            finalScale.x = startScale.x + scaleLimit.x;
            finalScale.y = startScale.y + scaleLimit.y;
            finalScale.z = startScale.z + scaleLimit.z;
            scaleTween = transform.DOScale(finalScale, 1.5f).SetLoops(-1, LoopType.Yoyo);
            // transform.DOScaleX(finalScale.x, scaleFrequency.x).SetLoops(-1, LoopType.Yoyo);
            // transform.DOScaleY(finalScale.y, scaleFrequency.y).SetLoops(-1, LoopType.Yoyo);
            // transform.DOScaleZ(finalScale.z, scaleFrequency.z).SetLoops(-1, LoopType.Yoyo);
        }
    }

    public void StopToDrop()
    {
        isRotate = false;
        isScale = false;
        // Debug.Log("STOP");
        myPathTween.Pause();
        scaleTween.Pause();
        StopAllCoroutines();
    }

    public void Update()
    {
        if (isRotate)
        {
            if (isYaw)
            {
                transform.Rotate(Vector3.up * Time.deltaTime * 40.0f);
            }
            if (isRoll)
            {
                transform.Rotate(Vector3.forward * Time.deltaTime * 40.0f);
            }
            if (isPitch)
            {
                transform.Rotate(Vector3.right * Time.deltaTime * 40.0f);
            }
        }
    }
}
