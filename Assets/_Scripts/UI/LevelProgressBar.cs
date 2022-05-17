using UnityEngine;
using UnityEngine.UI;

public class LevelProgressBar : MonoBehaviour
{
    [Header("UI references :")] 
    [SerializeField] private Image uiFillImage;

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

    private float GetDistance()
    {
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