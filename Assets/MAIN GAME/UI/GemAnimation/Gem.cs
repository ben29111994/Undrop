using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Gem : MonoBehaviour
{
    Vector3 startPosition;
    Vector3 endPosition;
    Vector3 randomPositon;
    float timeStep1;
    float timeStep2;

    public void ActiveAnimation(Vector3 targetPosition)
    {
        startPosition = transform.position;
        endPosition = targetPosition;
        randomPositon = startPosition + (Vector3)Random.insideUnitCircle * 70.0f;
        timeStep1 = Random.Range(0.2f, 0.4f);
        timeStep2 = Random.Range(0.3f, 0.5f);

        gameObject.SetActive(true);

        transform.DORotate(Vector3.forward * Random.Range(15, 30), 0.4f);
        ScaleStep();
        MoveStep();
    }

    private void ScaleStep()
    {
        transform.localScale = Vector3.zero * Random.Range(0.1f, 0.4f);

        transform.DOScale(Vector3.one, timeStep1).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            transform.DOScale(Vector3.zero, timeStep2).SetEase(Ease.InQuad);
        });
    }

    private void MoveStep()
    {
        transform.DOMove(randomPositon, timeStep1).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            transform.DOMove(endPosition, timeStep2).SetEase(Ease.InQuad).OnComplete(OnComplete);
        });
    }

    public void OnComplete()
    {
        gameObject.SetActive(false);
    }
}
