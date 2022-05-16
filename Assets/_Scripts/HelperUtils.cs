using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

namespace MuhammetInce.Helpers
{
    public static class HelperUtils
    {
        public static void RotateAround(GameObject obj, float durationSecond, int rotateSide, int rotateAngle)
        {
            var eulerAngles = obj.transform.eulerAngles;
            obj.transform.DORotate(new Vector3(eulerAngles.x, eulerAngles.y,  eulerAngles.z + rotateAngle * rotateSide), durationSecond, RotateMode.FastBeyond360);
        }
    }
}