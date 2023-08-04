using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GemDoMove : MonoBehaviour
{
    public GameObject endObject;
    public GameObject startObject;

    Vector3 startPosition;
    Vector3 endPosition;
    Vector3 randomPositon;
    float timeStep1;
    float timeStep2;

    public void ActiveGemDoMove()
    {
        //GetComponent<RectTransform>().anchoredPosition = startPos;
        startPosition = startObject.transform.position;
        endPosition = endObject.transform.position;       
        randomPositon = startPosition + (Vector3)Random.insideUnitCircle * 80.0f;
        timeStep1 = Random.Range(0.4f, 0.6f);
        timeStep2 = Random.Range(0.5f, 0.7f);

        gameObject.SetActive(true);

        ScaleStep();
        MoveStep();
    }

    private void ScaleStep()
    {
        transform.localScale = Vector3.zero * 0.2f;

        transform.DOScale(Vector3.one, timeStep1).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            transform.DOScale(Vector3.one * 0.1f, timeStep2).SetEase(Ease.InQuad);
        });
    }

    private void MoveStep()
    {
        transform.DOMove(randomPositon, timeStep1).SetEase(Ease.OutQuad).OnComplete(()=>
        {
            transform.DOMove(endPosition, timeStep2).SetEase(Ease.InQuad).OnComplete(OnComplete);
        });
    }

    public void OnComplete()
    {
        gameObject.SetActive(false);
    }
}
