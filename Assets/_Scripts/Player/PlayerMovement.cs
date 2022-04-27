using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Speeds"), Space]
    [SerializeField] private float verticalSpeed;
    [SerializeField] private float horizontalSpeed;

    [Header("Borders"), Space] 
    [SerializeField] private float leftSideBorder;
    [SerializeField] private float rightSideBorder;

    [Header("Player Rotate"), Space] 
    [SerializeField] private float rightLeftRotateAngle;
    [SerializeField] private float rightLeftRotateDuration;

    private Vector3 Pos
    {
        get => transform.position;
        set => transform.position = value;
    }

    public Touch Touch => Input.GetTouch(0);
    private void Update()
    {
        UpdateInit();
    }

    private void UpdateInit()
    {
        VerticalMovement();
        HorizontalMovement();
        BorderMovement();
        
    }

    private void VerticalMovement()
    {
        transform.Translate(0,0, verticalSpeed * Time.deltaTime);
    }

    private void HorizontalMovement()
    {
        if (Input.touchCount <= 0) return;
        

        if (Touch.phase == TouchPhase.Moved)
        {
            Pos = new Vector3(Pos.x + Touch.deltaPosition.x * (horizontalSpeed * Time.deltaTime), Pos.y, Pos.z);
            if (Touch.deltaPosition.x > 0) // right
                // transform.rotation = Quaternion.Euler(0, 20, 0);
                transform.DORotate( new Vector3(0, rightLeftRotateAngle, 0), rightLeftRotateDuration);
            else
                transform.DORotate( new Vector3(0, -rightLeftRotateAngle, 0), rightLeftRotateDuration);
        }

        if (Touch.phase == TouchPhase.Ended)
        {
            transform.DORotate(Vector3.zero, rightLeftRotateDuration);
        }
    }

    private void BorderMovement()
    {
        if (Pos.x < leftSideBorder)
            Pos = new Vector3(leftSideBorder, Pos.y, Pos.z);

        if (Pos.x > rightSideBorder)
            Pos = new Vector3(rightSideBorder, Pos.y, Pos.z);
    }
}
