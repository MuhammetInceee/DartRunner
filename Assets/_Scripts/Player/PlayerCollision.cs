using System;
using System.Runtime.Remoting.Messaging;
using MuhammetInce.Helpers;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private bool _rotateSide;
    private PlayerMovement _playerMovement;
    
    [Header("Floats")] 
    [SerializeField] private float rotateAroundDuration;
    [SerializeField] private float destroyedBonusBalloon;
    
    [Header("Player Speed"),Space]
    [SerializeField] private float increaseSpeedBoost;
    [SerializeField] private float decreaseSpeedBoost;
    [SerializeField] private float minVerSpeed;
    
    [Header("Material"), Space] 
    [SerializeField] private Material balloonHoloMat;
    [SerializeField] private Material redMat;
    [SerializeField] private Material blueMat;
    [SerializeField] private Material yellowMat;
    [SerializeField] private Material greenMat;
    
    [Header("Scores"), Space] 
    public int score;

    // Properties
    private Renderer PlayerRenderer => GetComponent<Renderer>();
    private float PlayerVerticalSpeed
    {
        get => _playerMovement.verticalSpeed;
        set => _playerMovement.verticalSpeed = value;
    }

    private Material PlayerMat
    {
        get => PlayerRenderer.material;
        set => PlayerRenderer.material = value;
    }

    private void Awake()
    {
        AwakeInit();
    }

    private void AwakeInit()
    {
        if(_playerMovement != null) return;
        _playerMovement = gameObject.GetComponent<PlayerMovement>();
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
            case "BonusBalloon":
                BonusBalloon();
                break;
        }
    }

    private void ColorChangeChecker(Collider other)
    {
        if (_rotateSide)
        {
            HelperUtils.RotateAround(gameObject, rotateAroundDuration, 1);
            _rotateSide = false;
        }
        else
        {
            HelperUtils.RotateAround(gameObject, rotateAroundDuration, -1);
            _rotateSide = true;
        }
        gameObject.layer = other.gameObject.layer;
        PlayerMatChanger();
    }

    private void BalloonBurstChecker(Collider other)
    {
        if (gameObject.layer == other.gameObject.layer)
        {
            // TODO
            // other.GetComponent<MeshRenderer>().material.SetVector("CutOff Height", new Vector4(3,0,0,0));
            score++;
            PlayerVerticalSpeed += increaseSpeedBoost;
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

    private void BonusBalloon()
    {
        PlayerVerticalSpeed -= decreaseSpeedBoost;
        
        if (PlayerVerticalSpeed <= minVerSpeed)
            _playerMovement.enabled = false;

        destroyedBonusBalloon++;
    }
}