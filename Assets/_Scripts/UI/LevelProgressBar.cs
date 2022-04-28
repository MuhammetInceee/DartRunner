using UnityEngine;
using UnityEngine.UI;

public class LevelProgressBar : MonoBehaviour
{
    [Header("UI references :")] 
    [SerializeField] private Image uiFillImage;
    [SerializeField] private Text uiStartText;
    [SerializeField] private Text uiEndText;

    [Header("Player & Endline references :")] 
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform endLineTransform;


    private Vector3 _endLinePosition;

    private float _fullDistance;


    private void Start()
    {
        _endLinePosition = endLineTransform.position;
        _fullDistance = GetDistance();
    }

    public void SetLevelTexts(int level)
    {
        uiStartText.text = level.ToString();
        uiEndText.text = (level + 1).ToString();
    }

    private float GetDistance()
    {
        // Slow
        //return Vector3.Distance (playerTransform.position, endLinePosition) ;

        // Fast
        return (_endLinePosition - playerTransform.position).sqrMagnitude;
    }


    private void UpdateProgressFill(float value)
    {
        uiFillImage.fillAmount = value;
    }


    private void Update()
    {
        if (!(playerTransform.position.z <= _endLinePosition.z)) return;
        var newDistance = GetDistance();
        var progressValue = Mathf.InverseLerp(_fullDistance, 0f, newDistance);

        UpdateProgressFill(progressValue);
    }
}