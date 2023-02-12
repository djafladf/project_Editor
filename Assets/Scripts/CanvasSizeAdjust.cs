using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSizeAdjust : MonoBehaviour
{
    public Camera mainCamera;
    public Canvas canvas;

    private void Awake()
    {
        if (!mainCamera)
        {
            mainCamera = Camera.main;
        }
        canvas = GetComponent<Canvas>();
    }

    private void Update()
    {
        float aspectRatio = mainCamera.aspect;
        float height = mainCamera.orthographicSize * 2;
        float width = height * aspectRatio;

        RectTransform canvasRectTransform = canvas.GetComponent<RectTransform>();
        canvasRectTransform.sizeDelta = new Vector2(width, height);
    }
}
