using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MuhammetInce.Helpers;
using UnityEngine;
public class BalloonPoint : MonoBehaviour
{
    private RectTransform _rectTransform;
    
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

        _rectTransform = gameObject.GetComponent<RectTransform>();

        canvas.worldCamera = mainCamera;
    }

    private void UpdateInit()
    {
        GoingUp();
    }

    private void GoingUp()
    {
        _rectTransform.Translate(0,1 * Time.deltaTime, 0);
        _rectTransform.DOScale(new Vector3(0, 0, 0), 1f);
        StartCoroutine(OnePlusWaiter());
    }
    
    private IEnumerator OnePlusWaiter()
    {
        yield return new WaitForSeconds(1f);
        gameObject.transform.root.gameObject.SetActive(false);
    }
}
