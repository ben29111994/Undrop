using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MaskController : MonoBehaviour
{
    public static MaskController Instance;
    public GameObject maskObject;
    public Material mask1Material;

    public Color redColor;
    public Color blueColor;

    private void Awake()
    {
        Instance = this;
    }

    public void FillBlueColorGround(Vector3 startPosition)
    {
        mask1Material.color = blueColor;
        startPosition.y = maskObject.transform.position.y;
        maskObject.transform.position = startPosition;
        Vector3 targetScale = new Vector3(50.0f, 0.01f, 50.0f);
        maskObject.transform.DOScale(targetScale, 1.2f).SetEase(Ease.Flash);
    }

    public void FillRedColorGround(Vector3 startPosition)
    {
        mask1Material.color = redColor;
        startPosition.y = maskObject.transform.position.y;
        maskObject.transform.position = startPosition;
        Vector3 targetScale = new Vector3(50.0f, 0.01f, 50.0f);
        maskObject.transform.DOScale(targetScale, 1.2f).SetEase(Ease.Flash);
    }
}
