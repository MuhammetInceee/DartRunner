using System;
using MuhammetInce.Helpers;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [Header("Floats"), Space] 
    [SerializeField] private float rotateAroundDuration;

    [Header("Material"), Space] 
    [SerializeField] private Material balloonHoloMat;
    [SerializeField] private Material redMat;
    [SerializeField] private Material blueMat;
    [SerializeField] private Material yellowMat;
    [SerializeField] private Material greenMat;

    [Header("Booleans"), Space] 
    [SerializeField] private bool rotateSide;

    [Header("Scores"), Space] 
    [SerializeField] private int score;


    // Properties
    private Renderer PlayerRenderer => GetComponent<Renderer>();

    private Material PlayerMat
    {
        get => PlayerRenderer.material;
        set => PlayerRenderer.material = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "ColorChanger":
                ColorChangeChecker(other);
                break;
            case "Balloon":
                BalloonBurstChecker(other);
                break;
        }
    }

    private void ColorChangeChecker(Collider other)
    {
        if (rotateSide)
        {
            HelperUtils.RotateAround(gameObject, rotateAroundDuration, 1);
            rotateSide = false;
        }
        else
        {
            HelperUtils.RotateAround(gameObject, rotateAroundDuration, -1);
            rotateSide = true;
        }
        gameObject.layer = other.gameObject.layer;
        PlayerMatChanger();
    }

    private void BalloonBurstChecker(Collider other)
    {
        if (gameObject.layer == other.gameObject.layer)
        {
            // TODO
            // Balloon Burst with Shader
            // Score Added
            
        }
        else
        {
            other.GetComponent<Renderer>().material = balloonHoloMat;
        }
    }

    private void PlayerMatChanger()
    {
        switch (gameObject.layer)
        {
            case 6:
                PlayerMat = blueMat;
                break;
            case 7:
                PlayerMat = redMat;
                break;
            case 8:
                PlayerMat = yellowMat;
                break;
            case 9:
                PlayerMat = greenMat;
                break;
        }
    }
}