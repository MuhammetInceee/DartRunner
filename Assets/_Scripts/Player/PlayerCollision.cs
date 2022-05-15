using MuhammetInce.Helpers;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PlayerCollision : MonoBehaviour
{
    #region Variables
    
    private PlayerMovement _playerMovement;
    private CameraController _cameraController;
    private bool _rotateSide;
    private bool _streakRotate;

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

    [Header("Obstacle Elements"), Space]
    [SerializeField] private int bounceBack;
    [SerializeField] private float bounceBackDur;
    
    [Header("Streak Elements"), Space]
    public int streakScore;
    [SerializeField] private int streakNeededScore;
    [SerializeField] private int streakLayer;
    [SerializeField] private Text streakCounterText;

    [Header("Canvases"), Space]
    [SerializeField] private GameObject levelEndCanvas;
    [SerializeField] private GameObject tapToStart;

    [Header("Audios"), Space]
    [SerializeField] private AudioSource balloonBurst;

    [Header("Cameras"), Space] 
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float cameraPosChangeDur;
    
    #endregion

    #region Properties
    private float PlayerVerticalSpeed
    {
        get => _playerMovement.verticalSpeed;
        set => _playerMovement.verticalSpeed = value;
    }
    private Vector3 Pos => transform.position;

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
        streakCounterText.text = "+ " + streakScore;
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
            balloonEffect.SetActive(true);
            balloonModel.SetActive(false);
            balloonBurst.Play();
            score++;
            PlayerVerticalSpeed += increaseSpeedBoost;
            streakScore++;

            if (!streakCounterText.gameObject.activeInHierarchy)
            {
                streakCounterText.gameObject.SetActive(true);
            }

            if (streakScore < streakNeededScore) return;
            if(streakCounterText.gameObject.activeInHierarchy)
                streakCounterText.gameObject.SetActive(false);

        }
        else
        {
            balloonModel.SetActive(false);
            balloonHoloModel.SetActive(true);
            streakScore = 0;
            streakCounterText.gameObject.SetActive(false);
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
        streakCounterText.gameObject.SetActive(false);
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
        if (streakScore < streakNeededScore) return;
        if (!_streakRotate)
        {
            HelperUtils.RotateAround(obj, rotateAroundDuration, 1, 359);
            _streakRotate = true;
        }
        obj.layer = streakLayer;
        playerChangeMat.mainTexture = rainbowTexture;
        playerChangeMat.color = Color.white;

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
}