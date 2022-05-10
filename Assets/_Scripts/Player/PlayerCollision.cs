using System;
using System.Data.SqlClient;
using System.Runtime.Remoting.Messaging;
using MuhammetInce.Helpers;
using UnityEngine;
using DG.Tweening;

public class PlayerCollision : MonoBehaviour
{
    private bool _rotateSide;
    private PlayerMovement _playerMovement;

    [Header("Floats")]
    [SerializeField] private float rotateAroundDuration;
    [SerializeField] private float destroyedBonusBalloon;

    [Header("Player Speed"), Space]
    [SerializeField] private float increaseSpeedBoost;
    [SerializeField] private float decreaseSpeedBoost;
    [SerializeField] private float minVerSpeed;

    [Header("Material"), Space]
    [SerializeField] private Material playerChangeMat;

    [Header("Player Colors"), Space]
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color redColor;
    [SerializeField] private Color blueColor;
    [SerializeField] private Color greenColor;
    [SerializeField] private Color yellowColor;

    [Header("Scores"), Space]
    public int score;

    [Header("Canvases"), Space]
    [SerializeField] private GameObject levelEndCanvas;
    [SerializeField] private GameObject _tapToStart;

    [Header("Audios"), Space]
    [SerializeField] private AudioSource balloonBurst;

    // Properties
    private float PlayerVerticalSpeed
    {
        get => _playerMovement.verticalSpeed;
        set => _playerMovement.verticalSpeed = value;
    }

    private void Awake() => AwakeInit();

    private void Update() => UpdateInit();

    private void AwakeInit()
    {
        playerChangeMat.color = defaultColor;
        if (_playerMovement != null) return;
        _playerMovement = gameObject.GetComponent<PlayerMovement>();
    }

    private void UpdateInit()
    {
        TapToStart();
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
                BonusBalloon(other);
                break;
            case "Obstacle":
                Obstacle(other);
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
        var balloonModel = other.gameObject.transform.GetChild(0).gameObject;
        var balloonHoloModel = other.gameObject.transform.GetChild(1).gameObject;
        var balloonEffect = other.gameObject.transform.GetChild(2).gameObject;

        if (gameObject.layer == other.gameObject.layer)
        {
            balloonEffect.SetActive(true);
            balloonModel.SetActive(false);
            balloonBurst.Play();

            score++;
            PlayerVerticalSpeed += increaseSpeedBoost;
        }
        else
        {
            balloonModel.SetActive(false);
            balloonHoloModel.SetActive(true);
        }
    }

    private void PlayerMatChanger()
    {
        playerChangeMat.color = gameObject.layer switch
        {
            6 => blueColor,
            7 => redColor,
            8 => yellowColor,
            9 => greenColor,
            _ => playerChangeMat.color
        };
    }

    private void BonusBalloon(Collider other)
    {
        PlayerVerticalSpeed -= decreaseSpeedBoost;
        balloonBurst.Play();

        var balloonModel = other.gameObject.transform.GetChild(0).gameObject;
        var balloonEffect = other.gameObject.transform.GetChild(2).gameObject;

        balloonEffect.SetActive(true);
        balloonModel.SetActive(false);

        if (PlayerVerticalSpeed <= minVerSpeed)
        {
            _playerMovement.enabled = false;
            levelEndCanvas.SetActive(true);
        }
        
        destroyedBonusBalloon++;
    }

    private void Obstacle(Collider other)
    {
        
    }
    
    private void TapToStart()
    {
        if (_tapToStart.activeInHierarchy)
        {
            _playerMovement.enabled = false;

            if (Input.GetMouseButtonDown(0))
            {
                _tapToStart.SetActive(false);
                _playerMovement.enabled = true;
            }
                
        }
    }
}