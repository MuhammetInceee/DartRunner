using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject target;
    public Vector3 _distance;
    public bool readyToChange;    
    private Vector3 Pos
    {
        get => transform.position;
        set => transform.position = value;
    }
    private Vector3 TargetPos
    {
        get => target.transform.position;
        set => target.transform.position = value;
    }
    private void Start()
    {
        _distance = Pos - TargetPos;
    }
    private void LateUpdate()
    {
        Pos = new Vector3((TargetPos.x + _distance.x), Pos.y, (TargetPos.z + _distance.z));

        if (readyToChange)
        {
            CameraHigher();
            readyToChange = false;
        }
    }

    private void CameraHigher() => Pos = Pos + new Vector3(0, 2, 0);
}
