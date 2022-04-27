using UnityEngine;
using DG.Tweening;

namespace MuhammetInce.Helpers
{
    public static class HelperUtils
    {
        public static void RotateAround(GameObject obj, float durationSecond, int rotateSide)
        {
            obj.transform.DORotate(new Vector3(0, 0,  obj.transform.eulerAngles.z + 180 * rotateSide), durationSecond, RotateMode.FastBeyond360);
        }
    }
}