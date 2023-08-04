using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemAnimation : MonoBehaviour
{
    public List<GemDoMove> gemDoMove = new List<GemDoMove>();

    public static GemAnimation Instance;
    public RectTransform CanvasRect;

    private void Awake()
    {
        Instance = this;
    }
    void OnEnable() {
        ActiveGemAnimation();
    }

    private void Update()
    {
       if (Input.GetMouseButtonDown(1))
       {
           ActiveGemAnimation();
       }
    }

    public void ActiveGemAnimation()
    {
        for(int i = 0; i < gemDoMove.Count; i++)
        {
            // Vector2 point = PositionWorldSpaceToCanvasOverLay(startPosition);
            gemDoMove[i].ActiveGemDoMove();
        }
    }

    private Vector2 PositionWorldSpaceToCanvasOverLay(Vector3 positionWorldSpace)
    {
        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(positionWorldSpace);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));
        return WorldObject_ScreenPosition;
    }
}
