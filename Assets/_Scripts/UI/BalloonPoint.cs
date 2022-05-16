using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonPoint : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Camera mainCamera;
    private void Start() => StartInit();
    private void Update() => UpdateInit();

    private void StartInit()
    {
        if (canvas == null)
            canvas = transform.parent.GetComponent<Canvas>();
        if(mainCamera == null)
            mainCamera = Camera.main;

        canvas.worldCamera = mainCamera;
    }

    private void UpdateInit()
    {
        
    }
}
