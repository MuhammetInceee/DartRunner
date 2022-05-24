using System.Collections;
using System.Collections.Generic;
using MuhammetInce.Helpers;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PlayerCollision : MonoBehaviour
{
    #region Variables
    
    private PlayerMovement _playerMovement;
    private CameraController _cameraController;
    private readonly int _isFiber = Animator.StringToHash("isFiber");
    private bool _rotateSide;
    private bool _streakRotate;
    private bool _isChangeFiber;
    private float _barFill;

    [Header("Floats")]
    [SerializeField] private float rotateAroundDuration;
    [SerializeField] private float destroyedBonusBalloon;
    
    [Header("Player Speed"), Space]
    [SerializeField] private float increaseSpeedBoost;
    [SerializeField] private float decreaseSpeedBoost;
    [SerializeField] private float minVerSpeed;

    [Header("Material"), Space]
    [SerializeField] private Material playerChangeMat;
    [SerializeField] private Texture rainbowTexture;

    [Header("Player Colors"), Space]
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color redColor;
    [SerializeField] private Color blueColor;
    [SerializeField] private Color greenColor;
    [SerializeField] private Color yellowColor;

    [Header("Integers"), Space]
    public int score;
    [SerializeField] private float increaseFiberSpeed;

    [Header("Obstacle Elements"), Space]
    [SerializeField] private int bounceBack;
    [SerializeField] private float bounceBackDur;
    
    [Header("Streak Elements"), Space]
    public int streakScore;
    [SerializeField] private float streakNeededScore;
    [SerializeField] private int streakLayer;
    [SerializeField] private Image streakBarFillImage;
    [SerializeField] private GameObject fiberEffect;
    [SerializeField] private GameObject fiberEffect2;

    [Header("Canvases"), Space]
    [SerializeField] private GameObject levelEndCanvas;
    [SerializeField] private GameObject tapToStart;

    [Header("Audios"), Space]
    [SerializeField] private AudioSource balloonBurst;

    [Header("Cameras"), Space]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Vector3 fiberCameraPos;
    [SerializeField] private Vector3 fiberCameraRot;
    [SerializeField] private float cameraPosChangeDur;

    [Header("Balloon Burst Elements"), Space] 
    [SerializeField] private GameObject plusOneCanvas;

    [Header("Animations"), Space] 
    [SerializeField] private Animator fiberAnim;

    
    #endregion

    #region Properties
    private float PlayerVerticalSpeed
    {
        get => _playerMovement.verticalSpeed;
        set => _playerMovement.verticalSpeed = value;
    }
    private Vector3 Pos => transform.position;

    private float FiberBarFillAmount => streakNeededScore / 100;

    #endregion

    private void Awake() => AwakeInit();
    private void Update() => UpdateInit();
    private void AwakeInit()
    {
        playerChangeMat.color = defaultColor;
        playerChangeMat.mainTexture = null;
        if (_playerMovement != null) return;
        _playerMovement = gameObject.GetComponent<PlayerMovement>();
        if(_cameraController != null) return;
        _cameraController = mainCamera.GetComponent<CameraController>();
    }
    private void UpdateInit()
    {
        streakBarFillImage.fillAmount = 
            Mathf.Lerp(streakBarFillImage.fillAmount, _barFill, 5 * Time.deltaTime);
        TapToStart();
        BalloonStreakCase();
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
                Obstacle();
                break;
            case "BonusEntry":
                BonusEntry();
                break;
        }
    }
    private void ColorChangeChecker(Collider other)
    {
        var o = other.gameObject;
        gameObject.layer = o.layer;

        PlayerMatChanger();
        DartRotator();
    }
    private void BalloonBurstChecker(Collider other)
    {
        var balloonModel = other.gameObject.transform.GetChild(0).gameObject;
        var balloonHoloModel = other.gameObject.transform.GetChild(1).gameObject;
        var balloonEffect = other.gameObject.transform.GetChild(2).gameObject;

        if (gameObject.layer == other.gameObject.layer || gameObject.layer == streakLayer)
        {
            _barFill += FiberBarFillAmount;
            var position = other.transform.position;
            Instantiate(plusOneCanvas,
                new Vector3(position.x, position.y + 1, position.z), Quaternion.identity);
            balloonEffect.SetActive(true);
            balloonModel.SetActive(false);
            balloonBurst.Play();
            score++;
            PlayerVerticalSpeed += increaseSpeedBoost;
            streakScore++;
            
            if (streakScore < streakNeededScore) return;

        }
        else
        {
            balloonModel.SetActive(false);
            balloonHoloModel.SetActive(true);
            streakScore = 0;
            _barFill = 0;
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
            StartCoroutine(LevelEndCanvasWaiter(0.7f));
        }
        
        destroyedBonusBalloon++;
    }
    private void Obstacle()
    {
        _playerMovement.enabled = false;
        transform.DOMove(new Vector3(Pos.x, Pos.y, Pos.z - bounceBack), bounceBackDur).
            OnComplete(() => _playerMovement.enabled = true);
    }
    private void BonusEntry()
    {
        transform.DORotate(Vector3.zero, _playerMovement.rightLeftRotateDuration);
        transform.DOMove(new Vector3(0, Pos.y, Pos.z + 1), 0.3f);
        mainCamera.transform.parent = gameObject.transform;
        _cameraController.enabled = false;
        mainCamera.transform.DOLocalMove(new Vector3(0,6,-13), cameraPosChangeDur);
        mainCamera.transform.DORotate(new Vector3(15, 0, 0), cameraPosChangeDur);
        _playerMovement.canHorizontal = false;
    }
    private void TapToStart()
    {
        if (!tapToStart.activeInHierarchy) return;
        
        
        _playerMovement.enabled = false;
        if (Input.GetMouseButtonDown(0))
        {
            tapToStart.SetActive(false);
            _playerMovement.enabled = true;
        }
    }
    private void BalloonStreakCase()
    {
        var obj = gameObject;
        if (streakBarFillImage.fillAmount < 1) return;
        if (!_streakRotate)
        {
            fiberAnim.SetBool(_isFiber, true);
            _streakRotate = true;
        }
        obj.layer = streakLayer;
        playerChangeMat.mainTexture = rainbowTexture;
        playerChangeMat.color = Color.white;
        if (!_isChangeFiber)
        {
            _playerMovement.verticalSpeed += increaseFiberSpeed;
            _cameraController._distance.z = Mathf.Lerp(_cameraController._distance.z, _cameraController._distance.z - 4, 100f);
            _cameraController.readyToChange = true;
            _isChangeFiber = true;
        }
        fiberEffect.SetActive(true);
        fiberEffect2.SetActive(true);

    }
    private void DartRotator()
    {
        if (_rotateSide)
        {
            HelperUtils.RotateAround(gameObject, rotateAroundDuration, 1, 180);
            _rotateSide = false;
        }
        else
        {
            HelperUtils.RotateAround(gameObject, rotateAroundDuration, -1, 180);
            _rotateSide = true;
        }
    }

    private IEnumerator LevelEndCanvasWaiter(float delay)
    {
        yield return new WaitForSeconds(delay);
        levelEndCanvas.SetActive(true);
    }

}